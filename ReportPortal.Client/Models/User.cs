using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "full_name")]
        public string Fullname { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "assigned_projects")]
        public IDictionary<string, ProjectAssigment> AssignedProjects { get; set; }
    }

    [DataContract]
    public class ProjectAssigment
    {
        [DataMember(Name = "projectRole")]
        public string ProjectRoleString { get; set; }

        public ProjectRole ProjectRole
        {
            get => EnumConverter.ConvertTo<ProjectRole>(ProjectRoleString);
            set => ProjectRoleString = EnumConverter.ConvertFrom(value);
        }
    }

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
