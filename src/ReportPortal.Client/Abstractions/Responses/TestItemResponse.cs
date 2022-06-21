using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class TestItemResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }

        [DataMember(Name = "parent")]
        public string ParentId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "startTime")]
        private string StartTimeString { get; set; }

        public DateTime StartTime => DateTimeConverter.ConvertTo(StartTimeString);

        [DataMember(Name = "endTime")]
        private string EndTimeString { get; set; }

        public DateTime? EndTime => EndTimeString == null ? (DateTime?)null : DateTimeConverter.ConvertTo(EndTimeString);

        [DataMember(Name = "retry")]
        public bool IsRetry { get; set; }

        [DataMember(Name = "status")]
        private string StatusString { get; set; }

        public Status Status => EnumConverter.ConvertTo<Status>(StatusString);

        [DataMember(Name = "type")]
        private string TypeString { get; set; }

        public TestItemType Type => EnumConverter.ConvertTo<TestItemType>(TypeString);

        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }

        [DataMember(Name = "issue")]
        public Issue Issue { get; set; }

        [DataMember(Name = "pathNames")]
        public PathNames PathNames { get; set; }

        [DataMember(Name = "hasChildren")]
        public bool HasChildren { get; set; }

        [DataMember(Name = "parameters")]
        public List<KeyValuePair<string, string>> Parameters { get; set; }

        [DataMember(Name = "uniqueId")]
        public string UniqueId { get; set; }

        /// <summary>
        /// Code reference for test. Example: namespace + classname + methodname
        /// </summary>
        [DataMember(Name = "codeRef")]
        public string CodeReference { get; set; }

        /// <summary>
        /// Test item attributes.
        /// </summary>
        [DataMember(Name = "attributes")]
        public IEnumerable<ItemAttribute> Attributes { get; set; }
    }

    [DataContract]
    public class PathNames
    {
        [DataMember(Name = "launchPathName")]
        public LaunchPathNameModel LaunchPathName { get; set; }

        [DataMember(Name = "itemPaths")]
        public IList<ItemPathNameModel> ItemPaths { get; set; }

        [DataContract]
        public class LaunchPathNameModel
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "number")]
            public long Number { get; set; }
        }

        [DataContract]
        public class ItemPathNameModel
        {
            [DataMember(Name = "id")]
            public long Id { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }
        }
    }

    [DataContract]
    public class Issue
    {
        [DataMember(Name = "issueType")]
        public string Type { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "autoAnalyzed")]
        public bool AutoAnalyzed { get; set; }

        [DataMember(Name = "ignoreAnalyzer")]
        public bool IgnoreAnalyzer { get; set; }

        [DataMember(Name = "externalSystemIssues")]
        public List<ExternalSystemIssue> ExternalSystemIssues { get; set; }
    }

    [DataContract]
    public class ExternalSystemIssue
    {
        [DataMember(Name = "submitDate")]
        private string SubmitDateString { get; set; }

        public DateTime SubmitDate => DateTimeConverter.ConvertTo(SubmitDateString);

        [DataMember(Name = "submitter")]
        public string Submitter { get; set; }

        [DataMember(Name = "systemId")]
        public string SystemId { get; set; }

        [DataMember(Name = "ticketId")]
        public string TicketId { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "btsProject")]
        public string BtsProject { get; set; }

        [DataMember(Name = "btsUrl")]
        public string BtsUrl { get; set; }
    }
}
