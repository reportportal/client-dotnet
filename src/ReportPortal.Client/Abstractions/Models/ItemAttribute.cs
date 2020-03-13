using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    [DataContract]
    public class ItemAttribute
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "system")]
        public bool IsSystem { get; set; }
    }
}
