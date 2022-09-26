using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Information about project.
    /// </summary>
    public class ProjectResponse
    {
        [JsonPropertyName("projectId")]
        public long Id { get; set; }

        [JsonPropertyName("projectName")]
        public string Name { get; set; }

        public ProjectConfiguration Configuration { get; set; }
    }
}
