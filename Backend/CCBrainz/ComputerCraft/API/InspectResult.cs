using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.ComputerCraft.API
{
    public class InspectResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("data")]
        public InspectData Data { get; set; }
    }

    public class InspectData
    {
        [JsonProperty("state")]
        public Dictionary<string, object> State { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, object> Tags { get; set; }

        [JsonProperty("name")]
        public string BlockName { get; set; }
    }
}
