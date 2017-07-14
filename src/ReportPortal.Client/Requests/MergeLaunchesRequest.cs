using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a content of request for service to create new launch.
    /// </summary>
    public class MergeLaunchesRequest
    {
        /// <summary>
        /// A short name of launch.
        /// </summary>
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
        /// Date time when the launch is finished.
        /// </summary>
        [JsonProperty("end_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Tags for merged launch.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Tags for merged launch.
        /// </summary>
        public List<string> Launches { get; set; }

        /// <summary>
        /// Type of launches merge.
        /// </summary>
        [JsonProperty("merge_type")]
        public string MergeType { get; set; }
    }
}
