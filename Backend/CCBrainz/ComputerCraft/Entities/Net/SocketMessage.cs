using CCBrainz.Websocket.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.ComputerCraft
{
    public sealed class SocketMessage
    {
        public CCOpCode OpCode { get; }

        public object Payload { get; }

        public SocketMessage(SocketFrame frame)
        {
            this.OpCode = (CCOpCode)frame.OpCode;

            this.Payload = frame.Payload;
        }

        public T ToType<T>()
            => (Payload as JToken).ToObject<T>();
    }
}
