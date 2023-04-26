using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class UserFilterResponse
    {
        /// <summary>
        /// ID of user filter.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Description of user filter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of conditions to filter data.
        /// </summary>
        public IList<Condition> Conditions { get; set; }

        /// <summary>
        /// Name of user filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// list of parameters of selection.
        /// </summary>
        public IList<FilterOrder> Orders { get; set; }

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

        /// <summary>
        /// Owner of user filter.
        /// </summary>
        public string Owner { get; set; }
    }

    public class Condition
    {
        [JsonPropertyName("condition")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<FilterOperation>))]
        public FilterOperation UserFilterCondition { get; set; }

        /// <summary>
        /// Field to filter by.
        /// </summary>
        public string FilteringField { get; set; }

        /// <summary>
        /// Value to filter by.
        /// </summary>
        public string Value { get; set; }
    }

    public class FilterOrder
    {
        /// <summary>
        /// Is ascendant order.
        /// </summary>
        [JsonPropertyName("isAsc")]
        public bool Asc { get; set; }

        /// <summary>
        /// A column to sort by.
        /// </summary>
        public string SortingColumn { get; set; }
    }
}
