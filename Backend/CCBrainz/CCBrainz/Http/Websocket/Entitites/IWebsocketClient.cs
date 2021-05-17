using CCBrainz.Websocket.Net;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.Http.Websocket.Entitites
{
    interface IWebsocketClient
    {
        WebSocket Socket { get; }

        ConnectionType ConnectionType { get; }

        bool IsAuthorized { get; }

        /// <summary>
        ///     The username of the owner of this socket
        /// </summary>
        string MinecraftUser { get; }

        Task SendAsync(SocketFrame data);

        Task ProcessEventAsync(SocketFrame frame);

        Task ProcessBinaryAsync(byte[] frame, bool endOfMessage);

        Task DisconnectAsync();
    }
}
