using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class ResponsesList<T>
    {
        [DataMember(Name = "responses")]
        public IEnumerable<T> Items { get; set; }
    }
}
