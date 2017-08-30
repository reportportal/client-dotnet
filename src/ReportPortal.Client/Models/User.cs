using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "full_name")]
        public string Fullname { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
