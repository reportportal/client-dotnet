using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a response containing user filter information.
    /// </summary>
    public class UserFilterResponse
    {
        /// <summary>
        /// Gets or sets the ID of the user filter.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the user filter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of conditions to filter data.
        /// </summary>
        public IList<Condition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets the name of the user filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of parameters of selection.
        /// </summary>
        public IList<FilterOrder> Orders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filter is shared.
        /// </summary>
        [JsonPropertyName("share")]
        public bool IsShared { get; set; }

        /// <summary>
        /// Gets or sets the user filter type.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<UserFilterType>))]
        public UserFilterType UserFilterType { get; set; }

        /// <summary>
        /// Gets or sets the owner of the user filter.
        /// </summary>
        public string Owner { get; set; }
    }

    /// <summary>
    /// Represents a condition used for filtering data.
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Gets or sets the user filter condition.
        /// </summary>
        [JsonPropertyName("condition")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<FilterOperation>))]
        public FilterOperation UserFilterCondition { get; set; }

        /// <summary>
        /// Gets or sets the field to filter by.
        /// </summary>
        public string FilteringField { get; set; }

        /// <summary>
        /// Gets or sets the value to filter by.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Represents the order of filtering.
    /// </summary>
    public class FilterOrder
    {
        /// <summary>
        /// Gets or sets a value indicating whether the order is ascendant.
        /// </summary>
        [JsonPropertyName("isAsc")]
        public bool Asc { get; set; }

        /// <summary>
        /// Gets or sets the column to sort by.
        /// </summary>
        public string SortingColumn { get; set; }
    }
}
