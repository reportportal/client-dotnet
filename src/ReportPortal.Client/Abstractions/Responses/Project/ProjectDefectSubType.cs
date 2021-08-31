using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    [DataContract]
    public class ProjectDefectSubType
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }

        [DataMember(Name = "locator")]
        public string Locator { get; set; }

        [DataMember(Name = "longName")]
        public string LongName { get; set; }

        [DataMember(Name = "shortName")]
        public string ShortName { get; set; }
    }
}
