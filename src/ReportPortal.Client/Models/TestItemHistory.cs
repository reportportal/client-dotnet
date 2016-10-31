using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using Newtonsoft.Json;

namespace ReportPortal.Client.Models
{
    public class TestItemHistory
    {
        [JsonProperty("launchNumber")]
        public long LaunchNumber { get; set; }

        [JsonProperty("launchId")]
        public string LaunchId { get; set; }

        [JsonProperty("startTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartTime { get; set; }

        [JsonProperty("launchStatus")]
        [JsonConverter(typeof(StatusConverter))]
        public Status LaunchStatus { get; set; }

        [JsonProperty("resources")]
        public List<TestItem> Resources { get; set; }
    }
}
