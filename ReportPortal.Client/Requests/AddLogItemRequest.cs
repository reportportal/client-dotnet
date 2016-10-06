using System;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request for logging messages into Report Portal.
    /// </summary>
    public class AddLogItemRequest
    {
        /// <summary>
        /// ID of test item to add new logs.
        /// </summary>
        [JsonProperty("item_id")]
        public string TestItemId { get; set; }

        /// <summary>
        /// Date time of log item.
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Time { get; set; }

        /// <summary>
        /// A level of log item.
        /// </summary>
        [JsonConverter(typeof(LogLevelConverter))]
        public LogLevel Level { get; set; }

        /// <summary>
        /// Message of log item.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        /// <summary>
        /// Specify an attachment of log item.
        /// </summary>
        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public Attach Attach { get; set; }
    }
}
