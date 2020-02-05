using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class Content<T>
    {
        [DataMember(Name = "content")]
        public IEnumerable<T> Items { get; set; }

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
}
