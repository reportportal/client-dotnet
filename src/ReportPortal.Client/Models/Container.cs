using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class LaunchesContainer
    {
        [DataMember(Name = "content")]
        public List<Launch> Launches { get; set; } 

        [DataMember(Name = "page")]
        public Page Page { get; set; }
    }

    [DataContract]
    public class TestItemsContainer
    {
        [DataMember(Name = "content")]
        public List<TestItem> TestItems { get; set; }

        [DataMember(Name = "page")]
        public Page Page { get; set; }
    }

    [DataContract]
    public class LogItemsContainer
    {
        [DataMember(Name = "content")]
        public List<LogItem> LogItems { get; set; }

        [DataMember(Name = "page")]
        public Page Page { get; set; }
    }

    [DataContract]
    public class Page
    {
        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "totalElements")]
        public int TotalElements { get; set; }

        [DataMember(Name = "totalPages")]
        public int TotalPages { get; set; }

        [DataMember(Name = "number")]
        public int Number { get; set; }
    }

    [DataContract]
    public class UserFilterContainer
    {
        [DataMember(Name = "content")]
        public IEnumerable<UserFilter> FilterElements { get; set; }

        [DataMember(Name = "page")]
        public Page Page { get; set; }
    }
}
