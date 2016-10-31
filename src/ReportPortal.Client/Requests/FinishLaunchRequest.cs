using System;
using ReportPortal.Client.Converters;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request to finish execution of specified launch.
    /// </summary>
    public class FinishLaunchRequest
    {
        /// <summary>
        /// Date time when launch execution is finished.
        /// </summary>
        [JsonProperty("end_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime EndTime { get; set; }
    }
}
