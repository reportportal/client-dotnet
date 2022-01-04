using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Information about project.
    /// </summary>
    [DataContract]
    public class ProjectResponse
    {
        [DataMember(Name = "projectId")]
        public string Id { get; set; }

        [DataMember(Name = "projectName")]
        public string Name { get; set; }

        [DataMember(Name = "configuration")]
        public ProjectConfiguration Configuration { get; set; }
    }
}
