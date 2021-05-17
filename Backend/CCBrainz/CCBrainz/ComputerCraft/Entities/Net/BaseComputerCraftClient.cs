using CCBrainz.Http;
using CCBrainz.Http.Websocket.Entitites;
using CCBrainz.Websocket.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public bool IsAuthorized
           => MinecraftUser != null;

        public BaseComputerCraftClient(HttpListenerWebSocketContext context, ComputercraftHello hello)
        {
            this.Socket = context.WebSocket;
            this.ConnectionType = ConnectionType.ComputerCraft;
            this.MinecraftUser = hello.Owner;
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
