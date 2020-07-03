using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class LogItemsCreatedResponse
    {
        [DataMember(Name = "responses")]
        public IList<LogItemCreatedResponse> LogItems { get; set; }
    }

    [DataContract]
    public class LogItemCreatedResponse
    {
        [DataMember(Name = "id")]
        public string Uuid { get; set; }
    }
}
