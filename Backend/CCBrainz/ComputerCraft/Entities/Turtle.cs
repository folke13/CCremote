using CCBrainz.Websocket.Net;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace CCBrainz.ComputerCraft
{
    public class Turtle : BaseComputerCraftClient
    {
        public Turtle(HttpListenerWebSocketContext context, ComputercraftHello hello)
            : base(context, hello)
        {

        }

    }
}
