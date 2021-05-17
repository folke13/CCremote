using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.ComputerCraft.API
{
    public class TransferResult
    {
        [JsonProperty("nonse")]
        public string CommandNonse { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }
    }
}
