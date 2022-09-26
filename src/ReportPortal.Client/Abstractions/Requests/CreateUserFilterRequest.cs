using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request for creating of user filters
    /// </summary>
    public class CreateUserFilterRequest
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
        /// Indicates if filter is shared.
        /// </summary>
        [JsonPropertyName("share")]
        public bool IsShared { get; set; }

        /// <summary>
        /// Owner of the filter.
        /// </summary>
        [DataMember(Name = "owner")]
        public string Owner { get; set; }

        /// <summary>
        /// User filter type enum.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<UserFilterType>))]
        public UserFilterType UserFilterType { get; set; }
    }
}