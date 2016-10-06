using System;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request to finish specified test item.
    /// </summary>
    public class FinishTestItemRequest
    {
        /// <summary>
        /// Date time when test item is finished.
        /// </summary>
        [JsonProperty("end_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// A result of test item.
        /// </summary>
        [JsonConverter(typeof(StatusConverter))]
        public Status Status { get; set; }

        /// <summary>
        /// A issue of test item if execution was proceeded with error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Issue Issue { get; set; }
    }
}
