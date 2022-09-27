using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Information about project.
    /// </summary>
    public class ProjectConfiguration
    {
        [JsonPropertyName("subTypes")]
        public ProjectDefectSubTypesContainer DefectSubTypes { get; set; }
    }
}
