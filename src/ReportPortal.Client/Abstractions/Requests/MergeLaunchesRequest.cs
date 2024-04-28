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
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the launch is finished.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the tags for the merged launch.
        /// </summary>
        public List<long> Launches { get; set; }

        /// <summary>
        /// Gets or sets the type of launches merge.
        /// </summary>
        public string MergeType { get; set; }
    }
}
