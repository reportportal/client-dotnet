using Newtonsoft.Json;

namespace ReportPortal.Client.Models
{
    /// <summary>
    /// Wraps a response from web service in case if something is wrong.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Code of error.
        /// </summary>
        [JsonProperty("error_code", Required = Required.Always)]
        public int Code { get; set; }

        /// <summary>
        /// Detailed message of error.
        /// </summary>
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }
}
