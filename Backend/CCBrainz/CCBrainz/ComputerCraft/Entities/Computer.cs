using CCBrainz.Websocket.Net;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace CCBrainz.ComputerCraft
{
    public class Computer : BaseComputerCraftClient
    {
        public Computer(HttpListenerWebSocketContext context, ComputercraftHello hello)
            : base(context, hello)
        {

        }
    }
}
