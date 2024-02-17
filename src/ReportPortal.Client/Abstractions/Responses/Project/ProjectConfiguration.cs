using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Information about project.
    /// </summary>
    public class ProjectConfiguration
    {
        /// <summary>
        /// Gets or sets the container for project defect subtypes.
        /// </summary>
        [JsonPropertyName("subTypes")]
        public ProjectDefectSubTypesContainer DefectSubTypes { get; set; }
    }
}
