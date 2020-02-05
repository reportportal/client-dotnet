using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Filtering
{
    public enum FilterOperation
    {
        [DataMember(Name = "eq")]
        Equals,
        [DataMember(Name = "ne")]
        NotEquals,
        [DataMember(Name = "cnt")]
        Contains,
        [DataMember(Name = "!cnt")]
        NotContains,
        [DataMember(Name = "ex")]
        Exists,
        [DataMember(Name = "in")]
        In,
        [DataMember(Name = "!in")]
        NotIn,
        [DataMember(Name = "gt")]
        GreaterThan,
        [DataMember(Name = "gte")]
        GreaterThanOrEquals,
        [DataMember(Name = "lt")]
        LowerThan,
        [DataMember(Name = "lte")]
        LowerThanOrEquals,
        [DataMember(Name = "btw")]
        Between,
        [DataMember(Name = "size")]
        Size,
        [DataMember(Name = "has")]
        Has
    }
    public class Filter
    {
        public Filter(FilterOperation operation, string field, object value, params object[] values)
        {
            Operation = operation;
            Field = field;
            Values = new List<object> {value};
            Values.AddRange(values);
        }

        public FilterOperation Operation { get; set; }

        public string Field { get; set; }

        public List<object> Values { get; set; } 
    }
}
