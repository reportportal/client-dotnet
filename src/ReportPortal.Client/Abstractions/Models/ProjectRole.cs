using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    public enum ProjectRole
    {
        [JsonPropertyName("PROJECT_MANAGER")]
        ProjectManager,
        [JsonPropertyName("MEMBER")]
        Member,
        [JsonPropertyName("OPERATOR")]
        Operator,
        [JsonPropertyName("CUSTOMER")]
        Customer
    }
}
