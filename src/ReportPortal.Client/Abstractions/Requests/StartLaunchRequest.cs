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
        /// Gets or sets the short name of the launch.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the launch is executed under debugging.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<LaunchMode>))]
        public LaunchMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the launch is executed.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether to start a new launch or use an existing one.
        /// </summary>
        [JsonPropertyName("rerun")]
        public bool IsRerun { get; set; }

        /// <summary>
        /// Gets or sets the UUID of the existing launch to use.
        /// </summary>
        [JsonPropertyName("rerunOf")]
        public string RerunOfLaunchUuid { get; set; }

        /// <summary>
        /// Gets or sets the launch attributes.
        /// </summary>
        public IList<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the specific launch UUID.
        /// </summary>
        public string Uuid { get; set; }
    }
}
