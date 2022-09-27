using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Filtering
{
    public enum FilterOperation
    {
        [JsonPropertyName("eq")]
        Equals,
        [JsonPropertyName("ne")]
        NotEquals,
        [JsonPropertyName("cnt")]
        Contains,
        [JsonPropertyName("!cnt")]
        NotContains,
        [JsonPropertyName("ex")]
        Exists,
        [JsonPropertyName("in")]
        In,
        [JsonPropertyName("!in")]
        NotIn,
        [JsonPropertyName("gt")]
        GreaterThan,
        [JsonPropertyName("gte")]
        GreaterThanOrEquals,
        [JsonPropertyName("lt")]
        LowerThan,
        [JsonPropertyName("lte")]
        LowerThanOrEquals,
        [JsonPropertyName("btw")]
        Between,
        [JsonPropertyName("size")]
        Size,
        [JsonPropertyName("has")]
        Has
    }
    public class Filter
    {
        public Filter(FilterOperation operation, string field, object value, params object[] values)
        {
            Operation = operation;
            Field = field;
            Values = new List<object> { value };
            Values.AddRange(values);
        }

        public FilterOperation Operation { get; set; }

        public string Field { get; set; }

        public List<object> Values { get; set; }
    }
}
