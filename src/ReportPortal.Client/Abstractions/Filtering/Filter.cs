using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Filtering
{
    /// <summary>
    /// Represents the available filter operations.
    /// </summary>
    public enum FilterOperation
    {
        /// <summary>
        /// Represents the "eq" filter operation, which checks for equality.
        /// </summary>
        [JsonPropertyName("eq")]
        Equals,

        /// <summary>
        /// Represents the "ne" filter operation, which checks for inequality.
        /// </summary>
        [JsonPropertyName("ne")]
        NotEquals,

        /// <summary>
        /// Represents the "cnt" filter operation, which checks if a value contains a specified substring.
        /// </summary>
        [JsonPropertyName("cnt")]
        Contains,

        /// <summary>
        /// Represents the "!cnt" filter operation, which checks if a value does not contain a specified substring.
        /// </summary>
        [JsonPropertyName("!cnt")]
        NotContains,

        /// <summary>
        /// Represents the "ex" filter operation, which checks if a value exists.
        /// </summary>
        [JsonPropertyName("ex")]
        Exists,

        /// <summary>
        /// Represents the "in" filter operation, which checks if a value is in a specified list.
        /// </summary>
        [JsonPropertyName("in")]
        In,

        /// <summary>
        /// Represents the "!in" filter operation, which checks if a value is not in a specified list.
        /// </summary>
        [JsonPropertyName("!in")]
        NotIn,

        /// <summary>
        /// Represents the "gt" filter operation, which checks if a value is greater than a specified value.
        /// </summary>
        [JsonPropertyName("gt")]
        GreaterThan,

        /// <summary>
        /// Represents the "gte" filter operation, which checks if a value is greater than or equal to a specified value.
        /// </summary>
        [JsonPropertyName("gte")]
        GreaterThanOrEquals,

        /// <summary>
        /// Represents the "lt" filter operation, which checks if a value is lower than a specified value.
        /// </summary>
        [JsonPropertyName("lt")]
        LowerThan,

        /// <summary>
        /// Represents the "lte" filter operation, which checks if a value is lower than or equal to a specified value.
        /// </summary>
        [JsonPropertyName("lte")]
        LowerThanOrEquals,

        /// <summary>
        /// Represents the "btw" filter operation, which checks if a value is between two specified values.
        /// </summary>
        [JsonPropertyName("btw")]
        Between,

        /// <summary>
        /// Represents the "size" filter operation, which checks if the size of a value meets a specified condition.
        /// </summary>
        [JsonPropertyName("size")]
        Size,

        /// <summary>
        /// Represents the "has" filter operation, which checks if a value has a specified property.
        /// </summary>
        [JsonPropertyName("has")]
        Has,

        /// <summary>
        /// Represents the "!has" filter operation, which checks if a value does not have a specified property.
        /// </summary>
        [JsonPropertyName("!has")]
        NotHas
    }

    /// <summary>
    /// Represents a filter used for querying data.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="operation">The filter operation.</param>
        /// <param name="field">The field to filter on.</param>
        /// <param name="value">The value to filter by.</param>
        /// <param name="values">Additional values to filter by.</param>
        public Filter(FilterOperation operation, string field, object value, params object[] values)
        {
            Operation = operation;
            Field = field;
            Values = new List<object> { value };
            Values.AddRange(values);
        }

        /// <summary>
        /// Gets or sets the filter operation.
        /// </summary>
        public FilterOperation Operation { get; set; }

        /// <summary>
        /// Gets or sets the field to filter on.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the values to filter by.
        /// </summary>
        public List<object> Values { get; set; }
    }
}
