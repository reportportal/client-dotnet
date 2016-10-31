using System.Collections.Generic;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request to finish specified launch.
    /// </summary>
    public class UpdateLaunchRequest
    {
        /// <summary>
        /// Update tags for launch.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Description of launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        [JsonConverter(typeof(LaunchModeConverter))]
        public LaunchMode Mode { get; set; }
    }
}
