using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.Websocket.Net
{
    public class ComputercraftHello
    {
        [JsonProperty("client")]
        public string ClientType { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        public static ComputercraftHello FromFrame(SocketFrame frame)
        {
            try
            {
                return (frame.Payload as JToken).ToObject<ComputercraftHello>();
            }
            catch
            {
                return null;
            }
        }
    }
}
