using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a content of request for service to create new launch.
    /// </summary>
    public class StartLaunchRequest
    {
        /// <summary>
        /// A short name of launch.
        /// </summary>
        [JsonConverter(typeof(TrimmingConverter), 256)]
        public string Name { get; set; }

        /// <summary>
        /// Description of launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        [JsonConverter(typeof(LaunchModeConverter))]
        public LaunchMode Mode { get; set; }

        /// <summary>
        /// Date time when the launch is executed.
        /// </summary>
        [JsonProperty("start_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Mark the launch with tags.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    }
}
