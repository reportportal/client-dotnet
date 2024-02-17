using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Represents the roles that a user can have in a project.
    /// </summary>
    public enum ProjectRole
    {
        /// <summary>
        /// The user has the role of a project manager.
        /// </summary>
        [JsonPropertyName("PROJECT_MANAGER")]
        ProjectManager,

        /// <summary>
        /// The user has the role of a member.
        /// </summary>
        [JsonPropertyName("MEMBER")]
        Member,

        /// <summary>
        /// The user has the role of an operator.
        /// </summary>
        [JsonPropertyName("OPERATOR")]
        Operator,

        /// <summary>
        /// The user has the role of a customer.
        /// </summary>
        [JsonPropertyName("CUSTOMER")]
        Customer
    }
}
