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
        /// Gets or sets the list of attributes to update for the launch.
        /// </summary>
        public List<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the description of the launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the mode in which the launch is executed.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<LaunchMode>))]
        public LaunchMode Mode { get; set; }
    }
}
