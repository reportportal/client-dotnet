using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "message")]
        public string Info { get; set; }
    }
}
