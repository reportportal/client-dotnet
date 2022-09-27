using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish specified launch.
    /// </summary>
    public class UpdateLaunchRequest
    {
        /// <summary>
        /// Update attributes for launch.
        /// </summary>
        public List<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Description of launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<LaunchMode>))]
        public LaunchMode Mode { get; set; }
    }
}
