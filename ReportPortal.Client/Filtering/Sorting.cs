using System.Collections.Generic;
using System.ComponentModel;

namespace ReportPortal.Client.Filtering
{
    public enum SortDirection
    {
        [Description("ASC")]
        Ascending,
        [Description("DESC")]
        Descending
    }
    public class Sorting
    {
        public Sorting(List<string> byFields, SortDirection direction = SortDirection.Ascending)
        {
            Fields = byFields;
            Direction = direction;
        }

        public List<string> Fields { get; set; }
        public SortDirection Direction { get; set; }
    }
}
