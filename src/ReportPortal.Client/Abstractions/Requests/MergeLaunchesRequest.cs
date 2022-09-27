using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
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
        [JsonConverter(typeof(JsonStringEnumConverterEx<LaunchMode>))]
        public LaunchMode Mode { get; set; }

        /// <summary>
        /// Date time when the launch is executed.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Date time when the launch is finished.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Tags for merged launch.
        /// </summary>
        public List<long> Launches { get; set; }

        /// <summary>
        /// Type of launches merge.
        /// </summary>
        public string MergeType { get; set; }
    }
}
