using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a content of request for service to create new test item in progress state.
    /// </summary>
    public class StartTestItemRequest
    {
        /// <summary>
        /// ID of parent launch to create new test item.
        /// </summary>
        public string LaunchUuid { get; set; }

        private string _name;

        /// <summary>
        /// A short name of test item.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get { return _name; } set { _name = StringTrimmer.Trim(value, 1024); } }

        /// <summary>
        /// A long description of test item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date time when new test item is created.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// A type of test item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<TestItemType>))]
        public TestItemType Type { get; set; }

        /// <summary>
        /// Retry status indicator.
        /// </summary>
        [JsonPropertyName("retry")]
        public bool IsRetry { get; set; }

        /// <summary>
        /// Test Item to be marked as retry.
        /// </summary>
        public string RetryOf { get; set; }

        /// <summary>
        /// A list of parameters.
        /// </summary>
        public IList<KeyValuePair<string, string>> Parameters { get; set; }

        /// <summary>
        /// A test item unique id.
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        /// Test Case ID.
        /// </summary>
        public string TestCaseId { get; set; }

        /// <summary>
        /// Code reference for test. Example: namespace + classname + methodname
        /// </summary>
        [JsonPropertyName("codeRef")]
        public string CodeReference { get; set; }

        /// <summary>
        /// Define if test item has stats. If false - considered as nested step.
        /// </summary>
        public bool HasStats { get; set; } = true;

        /// <summary>
        /// Test item attributes.
        /// </summary>
        public IList<ItemAttribute> Attributes { get; set; }
    }
}
