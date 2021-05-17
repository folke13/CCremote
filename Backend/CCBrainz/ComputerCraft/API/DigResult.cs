using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.ComputerCraft.API
{
    public class DigResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
