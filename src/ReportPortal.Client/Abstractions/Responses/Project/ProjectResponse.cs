using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Represents the response containing information about a project.
    /// </summary>
    public class ProjectResponse
    {
        /// <summary>
        /// Gets or sets the ID of the project.
        /// </summary>
        [JsonPropertyName("projectId")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        [JsonPropertyName("projectName")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration of the project.
        /// </summary>
        public ProjectConfiguration Configuration { get; set; }
    }
}
