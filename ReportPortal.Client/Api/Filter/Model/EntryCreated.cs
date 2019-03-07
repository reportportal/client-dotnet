using System.Runtime.Serialization;

namespace ReportPortal.Client.Api.Filter.Model
{
    [DataContract]
    public class EntryCreated
    {
        /// <summary>
        /// ID of created entry
        /// </summary>
        [DataMember(Name= "id")]
        public string Id { get; set; }
    }
}
