using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request for creating user filters.
    /// </summary>
    public class CreateUserFilterRequest
    {
        /// <summary>
        /// Gets or sets the name of the user filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the user filter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of conditions to filter data.
        /// </summary>
        public IEnumerable<Condition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets the list of parameters of selection.
        /// </summary>
        public IEnumerable<FilterOrder> Orders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filter is shared.
        /// </summary>
        [JsonPropertyName("share")]
        public bool IsShared { get; set; }

        /// <summary>
        /// Gets or sets the owner of the filter.
        /// </summary>
        [DataMember(Name = "owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the user filter type enum.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<UserFilterType>))]
        public UserFilterType UserFilterType { get; set; }
    }
}
