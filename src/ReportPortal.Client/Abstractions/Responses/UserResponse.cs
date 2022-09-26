using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class UserResponse
    {
        public string Fullname { get; set; }

        public string Email { get; set; }

        public IDictionary<string, ProjectAssigment> AssignedProjects { get; set; }
    }

    public class ProjectAssigment
    {
        [JsonPropertyName("projectRole")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<ProjectRole>))]
        public ProjectRole ProjectRole { get; set; }
    }
}
