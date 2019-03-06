using System.Runtime.Serialization;

namespace ReportPortal.Client.Common.Model
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "msg")]
        public string Info { get; set; }
    }
}
