using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request for updating of user filters
    /// </summary>
    public class UpdateUserFilterRequest
    {
        /// <summary>
        /// Name of user filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of user filter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of conditions to filter data.
        /// </summary>
        public IEnumerable<Condition> Conditions { get; set; }

        /// <summary>
        /// List of parameters of selection.
        /// </summary>
        public IEnumerable<FilterOrder> Orders { get; set; }

        /// <summary>
        /// Is filter shared.
        /// </summary>
        [JsonPropertyName("share")]
        public bool IsShared { get; set; }

        /// <summary>
        /// User filter type enum.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<UserFilterType>))]
        public UserFilterType UserFilterType { get; set; }
    }
}