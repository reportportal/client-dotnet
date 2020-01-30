using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class LogItemCreatedResponse
    {
        [DataMember(Name = "id")]
        public string Uuid { get; set; }
    }
}
