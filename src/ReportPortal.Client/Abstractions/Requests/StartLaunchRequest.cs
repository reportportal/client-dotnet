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
    public class StartLaunchRequest
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
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Don't start new launch and use some existing.
        /// </summary>
        [JsonPropertyName("rerun")]
        public bool IsRerun { get; set; }

        /// <summary>
        /// Don't start new launch and use some existing with specific launch UUID.
        /// </summary>
        [JsonPropertyName("rerunOf")]
        public string RerunOfLaunchUuid { get; set; }

        /// <summary>
        /// Launch attributes.
        /// </summary>
        public IList<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Set specific launch UUID.
        /// </summary>
        public string Uuid { get; set; }
    }
}
