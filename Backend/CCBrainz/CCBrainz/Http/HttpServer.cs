using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace CCBrainz.Http
{
    public enum ConnectionType
    {
        ComputerCraft,
        Web
    }


    public class HttpServer
    {
        public event Func<HttpListenerWebSocketContext, ConnectionType, Task> OnClient;

        private HttpListener _listener;
        
        public HttpServer(int port)
        {
            _listener = new HttpListener();

#if DEBUG
            _listener.Prefixes.Add($"http://localhost:{port}/cc/socket/");
            _listener.Prefixes.Add($"http://localhost:{port}/web/socket/");
#else
           _listener.Prefixes.Add($"http://*:{port}/cc/socket");
           _listener.Prefixes.Add($"http://*:{port}/web/socket");
#endif

            _listener.Start();

            _ = Task.Run(async () => await HandleRequest().ConfigureAwait(false));
        }


        private async Task HandleRequest()
        {
            while (_listener.IsListening)
            {
                var context = await _listener.GetContextAsync().ConfigureAwait(false);
                _ = Task.Run(async () => await HandleContext(context).ConfigureAwait(false));
            }
        }

        private async Task HandleContext(HttpListenerContext context)
        {
            using(var response = context.Response)
            {
                if (!context.Request.IsWebSocketRequest)
                {
                    response.StatusCode = 400;
                    return;
                }

                ConnectionType conType;

                switch (context.Request.RawUrl)
                {
                    case "/cc/socket/":
                        conType = ConnectionType.ComputerCraft;
                        break;
                    case "/web/socket/":
                        conType = ConnectionType.Web;
                        break;
                    default:
                        response.StatusCode = 400;
                        return;
                }

                var socket = await context.AcceptWebSocketAsync(null).ConfigureAwait(false);

                _ = Task.Run(async () =>
                {
                    var task = OnClient?.Invoke(socket, conType);
                    if (task != null)
                        await task.ConfigureAwait(false);
                });
            }
        }
    }
}
