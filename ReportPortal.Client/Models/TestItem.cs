using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using Newtonsoft.Json;

namespace ReportPortal.Client.Models
{
    public class TestItem
    {
        public string Id { get; set; }

        [JsonProperty("parent")]
        public string ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonProperty("start_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartTime { get; set; }

        [JsonProperty("end_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? EndTime { get; set; }

        [JsonConverter(typeof(StatusConverter))]
        public Status Status { get; set; }

        [JsonConverter(typeof(TestItemTypeConverter))]
        public TestItemType Type { get; set; }

        public List<string> Tags { get; set; }

        public Issue Issue { get; set; }

        [JsonProperty("path_names")]
        public Dictionary<string,string> PathNames { get; set; }

        [JsonProperty("has_childs")]
        public bool HasChilds { get; set; }
    }

    public class Issue
    {
        [JsonProperty("issue_type")]
        [JsonConverter(typeof(IssueTypeConverter))]
        public IssueType Type { get; set; }
        
        public string Comment { get; set; }

        [JsonProperty("externalSystemIssues")]
        public List<ExternalSystemIssue> ExternalSystemIssues { get; set; }
    }

    public class ExternalSystemIssue
    {
        [JsonProperty("submitDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? SubmitDate { get; set; }

        public string Submitter { get; set; }

        [JsonProperty("systemId")]
        public string SystemId { get; set; }

        [JsonProperty("ticketId")]
        public string TicketId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
