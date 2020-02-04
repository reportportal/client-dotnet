using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class UserFilterCreatedResponse
    {
        /// <summary>
        /// ID of created user filter
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
