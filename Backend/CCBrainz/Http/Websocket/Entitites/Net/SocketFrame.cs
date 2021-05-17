using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.Websocket.Net
{
    public class SocketFrame
    {
        [JsonProperty("op")]
        public ReservedOpCodes OpCode { get; set; }

        [JsonProperty("d")]
        public object Payload { get; set; }

        public T GetPayload<T>()
            => (Payload as JToken).ToObject<T>();

        public static SocketFrame ToFrame(ReservedOpCodes code, object payload)
            => new SocketFrame() { OpCode = code, Payload = payload };
    }
}
