using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish specified test item.
    /// </summary>
    public class FinishTestItemRequest
    {
        /// <summary>
        /// A long description of test item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date time when test item is finished.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime EndTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// A result of test item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status Status { get; set; } = Status.Passed;

        /// <summary>
        /// A issue of test item if execution was proceeded with error.
        /// </summary>
        public Issue Issue { get; set; }

        /// <summary>
        /// Sets attributes when finishing test item.
        /// </summary>
        /// <value>List of attributes.</value>
        public IList<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Retry status indicator.
        /// </summary>
        [JsonPropertyName("retry")]
        public bool IsRetry { get; set; }

        /// <summary>
        /// Test Item to be marked as retry.
        /// </summary>
        public string RetryOf { get; set; }
    }
}
