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
        /// Gets or sets a long description of the test item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the test item is finished.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime EndTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the result of the test item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status Status { get; set; } = Status.Passed;

        /// <summary>
        /// Gets or sets the issue of the test item if the execution was proceeded with an error.
        /// </summary>
        public Issue Issue { get; set; }

        /// <summary>
        /// Gets or sets the attributes when finishing the test item.
        /// </summary>
        /// <value>The list of attributes.</value>
        public IList<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the retry status indicator.
        /// </summary>
        [JsonPropertyName("retry")]
        public bool IsRetry { get; set; }

        /// <summary>
        /// Gets or sets the Test Item to be marked as a retry.
        /// </summary>
        public string RetryOf { get; set; }

        /// <summary>
        /// Gets or sets the UUID of the parent launch.
        /// </summary>
        public string LaunchUuid { get; set; }
    }
}
