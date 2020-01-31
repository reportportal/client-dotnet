using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using System.Runtime.Serialization;
using ReportPortal.Client.Models;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class TestItemModel
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
        public string StartTimeString { get; set; }

        public DateTime StartTime
        {
            get => DateTimeConverter.ConvertTo(StartTimeString);
            set => StartTimeString = DateTimeConverter.ConvertFrom(value);
        }

        [DataMember(Name = "endTime")]
        public string EndTimeString { get; set; }

        public DateTime? EndTime
        {
            get => EndTimeString == null? (DateTime?)null: DateTimeConverter.ConvertTo(EndTimeString);
            set => EndTimeString = DateTimeConverter.ConvertFrom(value.GetValueOrDefault());
        }

        [DataMember(Name = "status")]
        public string StatusString { get; set; }

        [DataMember(Name = "retry")]
        public bool IsRetry { get; set; }

        public Status Status 
        { 
            get => EnumConverter.ConvertTo<Status>(StatusString);
            set => StatusString = EnumConverter.ConvertFrom(value);
        }

        [DataMember(Name = "type")]
        public string TypeString { get; set; }

        public TestItemType Type 
        { 
            get => EnumConverter.ConvertTo<TestItemType>(TypeString);
            set => TypeString = EnumConverter.ConvertFrom(value);
        }

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
        /// Test item attributes.
        /// </summary>
        [DataMember(Name = "attributes")]
        public IEnumerable<Attribute> Attributes { get; set; }

        [DataContract]
        public class Attribute
        {
            [DataMember(Name = "key")]
            public string Key { get; set; }

            [DataMember(Name = "value")]
            public string Value { get; set; }

            [DataMember(Name = "system")]
            public bool IsSystem { get; set; }
        }
    }

    [DataContract]
    public class PathNames
    {
        [DataMember(Name = "launchPathName")]
        public LaunchPathNameModel LaunchPathName { get; set; }

        [DataContract]
        public class LaunchPathNameModel
        {
            public string Name { get; set; }

            public long Number { get; set; }
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
        public string SubmitDateString { get; set; }

        public DateTime SubmitDate
        {
            get => DateTimeConverter.ConvertTo(SubmitDateString);
            set => SubmitDateString = DateTimeConverter.ConvertFrom(value);
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
