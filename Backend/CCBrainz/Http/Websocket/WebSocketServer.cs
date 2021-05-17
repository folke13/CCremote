using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using CCBrainz.Http.Websocket.Entitites;
using CCBrainz.ComputerCraft;
using System.Threading;
using Newtonsoft.Json;
using CCBrainz.Websocket.Net;

namespace CCBrainz.Http.Websocket
{
    public class WebSocketServer
    {
        public event Func<Turtle, Task> OnTurtleConnected;
        public event Func<Computer, Task> OnComputerConnected;

        public const int InitialConnectionTimeout = 2500;

        internal List<IWebsocketClient> Clients = new List<IWebsocketClient>();

        private HttpServer _server;

        public WebSocketServer(int port)
        {
            _server = new HttpServer(port);

            _server.OnClient += _server_OnClient;
        }

        private async Task _server_OnClient(HttpListenerWebSocketContext arg1, ConnectionType arg2)
        {
            // Let's define the initial cancelation source, basically a timeout for the first hello packet
            var cancelSource = new CancellationTokenSource();
            cancelSource.CancelAfter(InitialConnectionTimeout);
            try
            {
                // Next, let's recieve the first frame
                var buffer = new byte[1024];
                var receiveResult = await arg1.WebSocket.ReceiveAsync(buffer, cancelSource.Token);

                if(receiveResult == null)
                    goto close;

                if (receiveResult.MessageType != WebSocketMessageType.Text)
                    goto close;

                // Let's read the data
                string json = Encoding.UTF8.GetString(buffer);
                var socketFrame = JsonConvert.DeserializeObject<SocketFrame>(json);

                // Handle both web and computer craft clients
                if(arg2 == ConnectionType.ComputerCraft)
                {
                    // Get the parsed hello packet
                    var computercraftHelloPacket = ComputercraftHello.FromFrame(socketFrame);

                    // Switch the client type
                    switch (computercraftHelloPacket.ClientType)
                    {
                        case "turtle":
                            {
                                var turtle = new Turtle(arg1, computercraftHelloPacket);
                                AddClient(turtle);
                                var task = OnTurtleConnected?.Invoke(turtle);
                                if (task != null)
                                    await task;
                                break;
                            }
                        case "computer":
                            {
                                var computer = new Computer(arg1, computercraftHelloPacket);
                                AddClient(computer);
                                var task = OnComputerConnected?.Invoke(computer);
                                if (task != null)
                                    await task;

                                break;
                            }
                        default:
                            Console.WriteLine($"Unknown client type: ({computercraftHelloPacket.ClientType})");
                            goto close;
                    }
                    return;
                }
                else
                {
                    // Handle web
                    return;
                }

            }
            catch (TaskCanceledException)
            {
                goto close;
            }
            catch (Exception x)
            {
                // Handle
                Console.Error.WriteLine(x);
                return;
            }

            close: await arg1.WebSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "No hello", CancellationToken.None);
        }


        private void AddClient(IWebsocketClient client)
        {
            Console.WriteLine($"New {client.ConnectionType} client: {client.MinecraftUser}");
            client.SendAsync(new SocketFrame()
            {
                OpCode = ReservedOpCodes.HelloAccepted,
            }).GetAwaiter().GetResult();
            Clients.Add(client);
            _ = Task.Run(async () => await HandleClient(client).ConfigureAwait(false));
        }

        private async Task HandleClient(IWebsocketClient client)
        {
            using (var socket = client.Socket)
            {
                var cancelSource = new CancellationTokenSource();

                while(socket.State == WebSocketState.Open)
                {
                    try
                    {
                        byte[] buffer = new byte[1024];

                        var receiveResult = await socket.ReceiveAsync(buffer, cancelSource.Token);

                        switch (receiveResult.MessageType)
                        {
                            case WebSocketMessageType.Text:
                                {
                                    string json = Encoding.UTF8.GetString(buffer);
                                    var frame = JsonConvert.DeserializeObject<SocketFrame>(json);

                                    var task = client.ProcessEventAsync(frame);
                                    await task.ConfigureAwait(false);

                                    if (task.Exception != null)
                                    {
                                        Console.Error.WriteLine(task.Exception);
                                    }
                                }
                                break;
                            case WebSocketMessageType.Binary:
                                {
                                    // Maybe use the receiveResult.Count to construct a new buffer?
                                    var task = client.ProcessBinaryAsync(buffer, receiveResult.EndOfMessage);

                                    await task.ConfigureAwait(false); 
                                    
                                    if (task.Exception != null)
                                    {
                                        Console.Error.WriteLine(task.Exception);
                                    }
                                }
                                break;

                            case WebSocketMessageType.Close:
                                {
                                    Console.WriteLine($"Socket closed with: {receiveResult.CloseStatus} - {receiveResult.CloseStatusDescription}");

                                    await client.DisconnectAsync();
                                    return;
                                }
                               
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine("Recieved canceled, Removing client");
                        await socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Failed read", CancellationToken.None);
                        Clients.Remove(client);
                    }
                    catch (Exception x)
                    {
                        Console.Error.WriteLine($"Failed on client read: {x}");
                    }
                }

                Console.WriteLine($"Got socket status {socket.State}, removing");
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Non-open state", CancellationToken.None);
                Clients.Remove(client);
            }
        }
    }
}
