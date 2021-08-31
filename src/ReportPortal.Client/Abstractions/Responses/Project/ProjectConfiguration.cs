using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Information about project.
    /// </summary>
    [DataContract]
    public class ProjectConfiguration
    {
        [DataMember(Name = "subTypes")]
        public ProjectDefectSubTypesContainer DefectSubTypes { get; set; }
    }
}
