using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Filtering
{
    /// <summary>
    /// Represents the sort direction.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Represents the ascending sort direction.
        /// </summary>
        [JsonPropertyName("ASC")]
        Ascending,

        /// <summary>
        /// Represents the descending sort direction.
        /// </summary>
        [JsonPropertyName("DESC")]
        Descending
    }

    /// <summary>
    /// Represents the sorting criteria.
    /// </summary>
    public class Sorting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sorting"/> class with the specified fields and direction.
        /// </summary>
        /// <param name="byFields">The list of fields to sort by.</param>
        /// <param name="direction">The sort direction.</param>
        public Sorting(List<string> byFields, SortDirection direction = SortDirection.Ascending)
        {
            Fields = byFields;
            Direction = direction;
        }

        /// <summary>
        /// Gets or sets the list of fields to sort by.
        /// </summary>
        public List<string> Fields { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public SortDirection Direction { get; set; }
    }
}
