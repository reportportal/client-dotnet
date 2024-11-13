using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a response object for a test item.
    /// </summary>
    public class TestItemResponse
    {
        /// <summary>
        /// Gets or sets the ID of the test item.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the UUID of the test item.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the parent ID of the test item.
        /// </summary>
        [JsonPropertyName("parent")]
        public long? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the test item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the test item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start time of the test item.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the test item.
        /// </summary>
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the list of retries for the test item.
        /// </summary>
        public IList<TestItemResponse> Retries { get; set; }

        /// <summary>
        /// Gets or sets the status of the test item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the type of the test item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<TestItemType>))]
        public TestItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the issue associated with the test item.
        /// </summary>
        public Issue Issue { get; set; }

        /// <summary>
        /// Gets or sets the path names of the test item.
        /// </summary>
        public PathNames PathNames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test item has children.
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Gets or sets the parameters of the test item.
        /// </summary>
        public IList<KeyValuePair<string, string>> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the unique ID of the test item.
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the code reference for the test item.
        /// </summary>
        [JsonPropertyName("codeRef")]
        public string CodeReference { get; set; }

        /// <summary>
        /// Gets or sets the attributes of the test item.
        /// </summary>
        public IList<ItemAttribute> Attributes { get; set; }
    }

    /// <summary>
    /// Represents the path names of a test item.
    /// </summary>
    public class PathNames
    {
        /// <summary>
        /// Gets or sets the launch path name model.
        /// </summary>
        public LaunchPathNameModel LaunchPathName { get; set; }

        /// <summary>
        /// Gets or sets the list of item path name models.
        /// </summary>
        public IList<ItemPathNameModel> ItemPaths { get; set; }

        /// <summary>
        /// Represents the launch path name model.
        /// </summary>
        public class LaunchPathNameModel
        {
            /// <summary>
            /// Gets or sets the name of the launch path.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the number of the launch path.
            /// </summary>
            public long Number { get; set; }
        }

        /// <summary>
        /// Represents the item path name model.
        /// </summary>
        public class ItemPathNameModel
        {
            /// <summary>
            /// Gets or sets the ID of the item path.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Gets or sets the name of the item path.
            /// </summary>
            public string Name { get; set; }
        }
    }

    /// <summary>
    /// Represents an issue associated with a test item.
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Gets or sets the type of the issue.
        /// </summary>
        [JsonPropertyName("issueType")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the comment of the issue.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the issue is auto-analyzed.
        /// </summary>
        public bool AutoAnalyzed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore the analyzer for the issue.
        /// </summary>
        public bool IgnoreAnalyzer { get; set; }

        /// <summary>
        /// Gets or sets the list of external system issues associated with the issue.
        /// </summary>
        public IList<ExternalSystemIssue> ExternalSystemIssues { get; set; }
    }

    /// <summary>
    /// Represents an external system issue associated with an issue.
    /// </summary>
    public class ExternalSystemIssue
    {
        /// <summary>
        /// Gets or sets the submit date of the external system issue.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Gets or sets the submitter of the external system issue.
        /// </summary>
        public string Submitter { get; set; }

        /// <summary>
        /// Gets or sets the system ID of the external system issue.
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// Gets or sets the ticket ID of the external system issue.
        /// </summary>
        public string TicketId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the external system issue.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the BTS project of the external system issue.
        /// </summary>
        public string BtsProject { get; set; }

        /// <summary>
        /// Gets or sets the BTS URL of the external system issue.
        /// </summary>
        public string BtsUrl { get; set; }
    }
}
