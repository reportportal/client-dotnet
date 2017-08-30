using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    public class Message
    {
        [DataMember(Name = "msg")]
        public string Info { get; set; }
    }
}
