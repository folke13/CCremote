using CCBrainz.ComputerCraft.API;
using CCBrainz.Http;
using CCBrainz.Http.Websocket.Entitites;
using CCBrainz.Websocket.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CCBrainz.ComputerCraft
{
    public abstract class BaseComputerCraftClient : IWebsocketClient
    {
        protected event Func<SocketMessage, Task> MessageRecieved;
        protected event Func<byte[], bool, Task> BinaryRecieved;

        public string MinecraftUser { get; }
        
        public WebSocket Socket { get; }

        public ConnectionType ConnectionType { get; }

        public int ComputerId { get; }

        public bool IsAuthorized
           => MinecraftUser != null;

        private List<AsyncCommand> ActiveCommands = new List<AsyncCommand>();
        private Dictionary<string, TaskCompletionSource<object[]>> ActiveBatchCommands = new Dictionary<string, TaskCompletionSource<object[]>>();

        public BaseComputerCraftClient(HttpListenerWebSocketContext context, ComputercraftHello hello)
        {
            this.Socket = context.WebSocket;
            this.ConnectionType = ConnectionType.ComputerCraft;
            this.MinecraftUser = hello.Owner;
            this.ComputerId = hello.ComputerId;
        }

        public async Task<TResult> SendCommandAsync<TResult>(CCOpCode code, object payload = null)
        {
            var command = new AsyncCommand(code, payload);

            SendAsync(command.ToSocketFrame()).GetAwaiter().GetResult();

            ActiveCommands.Add(command);

            var result = await command.GetResult<TResult>();

            return result.Completed ? result.Value : GetCustomDefault<TResult>();
        }

        public async Task<object[]> SendBatchCommandsAsync(params (CCOpCode code, object payload)[] commands)
        {
            List<AsyncCommand> asyncCommands = new List<AsyncCommand>();

            foreach(var item in commands)
            {
                asyncCommands.Add(new AsyncCommand(item.code, item.payload));
            }

            var id = asyncCommands.First().CommandNonse;
            SocketFrame BatchSocketFrame = new SocketFrame()
            {
                OpCode = ReservedOpCodes.BatchCommand,
                Payload = new
                {
                    id = id,
                    cmds = asyncCommands.Select(x => x.Payload).ToArray()
                }
            };

            var source = new TaskCompletionSource<object[]>();

            ActiveBatchCommands.Add(id, source);

            await SendAsync(BatchSocketFrame);

            var result = await Task.WhenAny(source.Task, Task.Delay(5000));

            if (result == source.Task)
            {
                return source.Task.Result;
            }
            else return null;
        }

        private TResult GetCustomDefault<TResult>()
        {
            var type = typeof(TResult);

            if (type == typeof(int))
                return (TResult)((object)-1);

            else return default(TResult);
        }

        public async Task ProcessEventAsync(SocketFrame frame)
        {
            var task = MessageRecieved?.Invoke(new SocketMessage(frame));

            if(task != null)
            {
                await task.ConfigureAwait(false);
                if (task.Exception != null)
                {
                    Console.Error.WriteLine(task.Exception);
                }
            }

            if(frame.OpCode == ReservedOpCodes.CommandResponse)
            {
                var result = frame.GetPayload<CommandResponse>();

                var command = ActiveCommands.FirstOrDefault(x => x.CommandNonse == result.CommandNonse);

                if (command == null)
                    return;

                command.SetResult(command.Payload);
                ActiveCommands.Remove(command);
            }
            else if(frame.OpCode == ReservedOpCodes.BatchCommandResult)
            {
                var result = frame.GetPayload<CommandResponse>();
                var command = ActiveBatchCommands.FirstOrDefault(x => x.Key == result.CommandNonse);

                if (command.Key == null)
                    return;

                command.Value.SetResult((object[])result.Result);
                ActiveBatchCommands.Remove(command.Key);
            }
        }
        public async Task ProcessBinaryAsync(byte[] frame, bool endOfMessage)
        {
            var task = BinaryRecieved?.Invoke(frame, endOfMessage);

            if(task != null)
            {
                await task.ConfigureAwait(false);
                if(task.Exception != null)
                {
                    Console.Error.WriteLine(task.Exception);
                }
            }
        }

        /// <summary>
        ///     Disposes this connection and disconnects the cc computer
        /// </summary>
        public Task DisconnectAsync()
            => DisconnectAsync(null, CancellationToken.None);

        public Task DisconnectAsync(string reason)
            => DisconnectAsync(reason, CancellationToken.None);
        public Task DisconnectAsync(string reason, CancellationToken token)
            => Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, token);

        public Task SendAsync(SocketFrame payload)
            => SendAsync(payload, CancellationToken.None);

        public Task SendAsync(SocketFrame payload, CancellationToken token)
        {
            // Get the buffer
            var json = JsonConvert.SerializeObject(payload);
            var bytes = Encoding.UTF8.GetBytes(json);

            return Socket.SendAsync(bytes, WebSocketMessageType.Text, true, token);
        }

        public Task SendBinaryAsync(byte[] buffer)
            => SendBinaryAsync(buffer, CancellationToken.None);

        public Task SendBinaryAsync(byte[] buffer, CancellationToken token)
            => Socket.SendAsync(buffer, WebSocketMessageType.Binary, true, token);
    }
}
