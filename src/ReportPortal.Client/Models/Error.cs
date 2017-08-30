using System.Runtime.Serialization;

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
        [DataMember(Name = "error_code", IsRequired = true)]
        public int Code { get; set; }

        /// <summary>
        /// Detailed message of error.
        /// </summary>
        [DataMember(Name = "message", IsRequired = true)]
        public string Message { get; set; }
    }
}
