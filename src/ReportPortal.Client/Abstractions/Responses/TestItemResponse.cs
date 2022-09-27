using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class TestItemResponse
    {
        public long Id { get; set; }

        public string Uuid { get; set; }

        [JsonPropertyName("parent")]
        public long? ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime StartTime { get; set; }

        [JsonConverter(typeof(NullableDateTimeUnixEpochConverter))]
        public DateTime? EndTime { get; set; }

        [JsonPropertyName("retry")]
        public bool IsRetry { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverterEx<TestItemType>))]
        public TestItemType Type { get; set; }

        public Issue Issue { get; set; }

        public PathNames PathNames { get; set; }

        public bool HasChildren { get; set; }

        public List<KeyValuePair<string, string>> Parameters { get; set; }

        public string UniqueId { get; set; }

        /// <summary>
        /// Code reference for test. Example: namespace + classname + methodname
        /// </summary>
        [JsonPropertyName("codeRef")]
        public string CodeReference { get; set; }

        /// <summary>
        /// Test item attributes.
        /// </summary>
        public IEnumerable<ItemAttribute> Attributes { get; set; }
    }

    public class PathNames
    {
        public LaunchPathNameModel LaunchPathName { get; set; }

        public IList<ItemPathNameModel> ItemPaths { get; set; }

        public class LaunchPathNameModel
        {
            public string Name { get; set; }

            public long Number { get; set; }
        }

        public class ItemPathNameModel
        {
            public long Id { get; set; }

            public string Name { get; set; }
        }
    }

    public class Issue
    {
        [JsonPropertyName("issueType")]
        public string Type { get; set; }

        public string Comment { get; set; }

        public bool AutoAnalyzed { get; set; }

        public bool IgnoreAnalyzer { get; set; }

        public List<ExternalSystemIssue> ExternalSystemIssues { get; set; }
    }

    public class ExternalSystemIssue
    {
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime SubmitDate { get; set; }

        public string Submitter { get; set; }

        public string SystemId { get; set; }

        public string TicketId { get; set; }

        public string Url { get; set; }

        public string BtsProject { get; set; }

        public string BtsUrl { get; set; }
    }
}
