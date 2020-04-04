using System.Collections.Generic;
using Newtonsoft.Json;

namespace Covid.Models.SupportModels
{
    public partial class CountryTimeLine
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("provinces")]
        public string[] Provinces { get; set; }

        [JsonProperty("timeline")]
        public Timeline Timeline { get; set; }
    }

    public partial class Timeline
    {
        [JsonProperty("cases")]
        public Dictionary<string, long> Cases { get; set; }

        [JsonProperty("deaths")]
        public Dictionary<string, long> Deaths { get; set; }

        [JsonProperty("recovered")]
        public Dictionary<string, long> Recovered { get; set; }
    }
}