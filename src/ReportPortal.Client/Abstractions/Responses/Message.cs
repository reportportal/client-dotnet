using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "message")]
        public string Info { get; set; }
    }
}
