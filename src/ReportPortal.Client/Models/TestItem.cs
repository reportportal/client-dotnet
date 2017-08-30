using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    public class TestItem
    {
        public string Id { get; set; }

        [DataMember(Name = "parent")]
        public string ParentId { get; set; }

        public string Name { get; set; }

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

        public Status Status { get; set; }

        public TestItemType Type { get; set; }

        public List<string> Tags { get; set; }

        public Issue Issue { get; set; }

        [DataMember(Name = "path_names")]
        public Dictionary<string,string> PathNames { get; set; }

        [DataMember(Name = "has_childs")]
        public bool HasChilds { get; set; }
    }

    public class Issue
    {
        [DataMember(Name = "issue_type")]
        public IssueType Type { get; set; }
        
        public string Comment { get; set; }

        [DataMember(Name = "externalSystemIssues")]
        public List<ExternalSystemIssue> ExternalSystemIssues { get; set; }
    }

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

        public string Submitter { get; set; }

        [DataMember(Name = "systemId")]
        public string SystemId { get; set; }

        [DataMember(Name = "ticketId")]
        public string TicketId { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
