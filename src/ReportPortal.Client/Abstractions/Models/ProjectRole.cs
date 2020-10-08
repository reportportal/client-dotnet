using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    public enum ProjectRole
    {
        [DataMember(Name = "PROJECT_MANAGER")]
        ProjectManager,
        [DataMember(Name = "MEMBER")]
        Member,
        [DataMember(Name = "OPERATOR")]
        Operator,
        [DataMember(Name = "CUSTOMER")]
        Customer
    }
}
