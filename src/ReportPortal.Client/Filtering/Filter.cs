using System.Collections.Generic;
using System.ComponentModel;

namespace ReportPortal.Client.Filtering
{
    public enum FilterOperation
    {
        [Description("eq")]
        Equals,
        [Description("!eq")]
        NotEquals,
        [Description("cnt")]
        Contains,
        [Description("!cnt")]
        NotContains,
        [Description("ex")]
        Exists,
        [Description("in")]
        In,
        [Description("!in")]
        NotIn,
        [Description("gt")]
        GreaterThan,
        [Description("gte")]
        GreaterThanOrEquals,
        [Description("lt")]
        LowerThan,
        [Description("lte")]
        LowerThanOrEquals,
        [Description("btw")]
        Between,
        [Description("size")]
        Size,
        [Description("has")]
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
