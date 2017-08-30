using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class TestItem
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "parent")]
        public string ParentId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "start_time")]
        public string StartTimeString { get; set; }

        public DateTime StartTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(StartTimeString);
            }
            set
            {
                StartTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        [DataMember(Name = "end_time")]
        public string EndTimeString { get; set; }

        public DateTime EndTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(EndTimeString);
            }
            set
            {
                EndTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        [DataMember(Name = "status")]
        public string StatusString { get; set; }

        public Status Status { get { return EnumConverter.ConvertTo<Status>(StatusString); } set { StatusString = EnumConverter.ConvertFrom(value); } }

        [DataMember(Name = "type")]
        public string TypeString { get; set; }

        public TestItemType Type { get { return EnumConverter.ConvertTo<TestItemType>(TypeString); } set { TypeString = EnumConverter.ConvertFrom(value); } }

        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }

        [DataMember(Name = "issue")]
        public Issue Issue { get; set; }

        [DataMember(Name = "path_names")]
        public Dictionary<string, string> PathNames { get; set; }

        [DataMember(Name = "has_childs")]
        public bool HasChilds { get; set; }
    }

    [DataContract]
    public class Issue
    {
        [DataMember(Name = "issue_type")]
        public string TypeString { get; set; }

        public IssueType Type { get { return EnumConverter.ConvertTo<IssueType>(TypeString); } set { TypeString = EnumConverter.ConvertFrom(value); } }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "externalSystemIssues")]
        public List<ExternalSystemIssue> ExternalSystemIssues { get; set; }
    }

    [DataContract]
    public class ExternalSystemIssue
    {
        [DataMember(Name = "submitDate")]
        public string SubmitDateString { get; set; }

        public DateTime SubmitDate
        {
            get
            {
                return DateTimeConverter.ConvertTo(SubmitDateString);
            }
            set
            {
                SubmitDateString = DateTimeConverter.ConvertFrom(value);
            }
        }

        [DataMember(Name = "submitter")]
        public string Submitter { get; set; }

        [DataMember(Name = "systemId")]
        public string SystemId { get; set; }

        [DataMember(Name = "ticketId")]
        public string TicketId { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
