using ReportPortal.Client.Converters;
using System;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish execution of specified launch.
    /// </summary>
    public class FinishLaunchRequest
    {
        /// <summary>
        /// Date time when launch execution is finished.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime EndTime { get; set; }
    }
}
