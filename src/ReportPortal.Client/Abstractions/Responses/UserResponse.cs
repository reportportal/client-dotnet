using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a response containing user information.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the assigned projects of the user.
        /// </summary>
        public IDictionary<string, ProjectAssigment> AssignedProjects { get; set; }
    }

    /// <summary>
    /// Represents a project assignment for a user.
    /// </summary>
    public class ProjectAssigment
    {
        /// <summary>
        /// Gets or sets the role of the project assignment.
        /// </summary>
        [JsonPropertyName("projectRole")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<ProjectRole>))]
        public ProjectRole ProjectRole { get; set; }
    }
}
