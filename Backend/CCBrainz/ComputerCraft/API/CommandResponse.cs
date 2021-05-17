using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.ComputerCraft.API
{
    public sealed class CommandResponse
    {
        [JsonProperty("nonse")]
        public string CommandNonse { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }
    }
}
