using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.ComputerCraft.API
{
    public class ItemDetail
    {
        [JsonProperty("name")]
        public string ItemName { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
