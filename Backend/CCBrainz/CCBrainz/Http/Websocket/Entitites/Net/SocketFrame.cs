using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.Websocket.Net
{
    public class SocketFrame
    {
        [JsonProperty("op")]
        public OpCode OpCode { get; set; }

        [JsonProperty("d")]
        public object Payload { get; set; }

        public static SocketFrame ToFrame(OpCode code, object payload)
            => new SocketFrame() { OpCode = code, Payload = payload };
    }
}
