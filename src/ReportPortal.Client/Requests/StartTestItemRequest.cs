using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a content of request for service to create new test item in progress state.
    /// </summary>
    public class StartTestItemRequest
    {
        /// <summary>
        /// ID of parent launch to create new test item.
        /// </summary>
        [JsonProperty("launch_id")]
        public string LaunchId { get; set; }

        /// <summary>
        /// A short name of test item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A long description of test item.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Date time when new test item is created.
        /// </summary>
        [JsonProperty("start_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// A type of test item.
        /// </summary>
        [JsonConverter(typeof(TestItemTypeConverter))]
        public TestItemType Type { get; set; }

        /// <summary>
        /// A list of tags.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
