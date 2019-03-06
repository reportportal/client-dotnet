using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Common.Model.Filtering
{
    public enum SortDirection
    {
        [DataMember(Name = "ASC")]
        Ascending,
        [DataMember(Name = "DESC")]
        Descending
    }
    public class Sorting
    {
        public Sorting(List<string> byFields, SortDirection direction = SortDirection.Ascending)
        {
            Fields = byFields;
            Direction = direction;
        }

        public List<string> Fields { get; }
        public SortDirection Direction { get; set; }
    }
}
