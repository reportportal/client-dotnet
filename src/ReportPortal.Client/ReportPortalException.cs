using System;

namespace ReportPortal.Client
{
    /// <summary>
    /// Occurs when server cannot process a request.
    /// </summary>
    public class ReportPortalException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ReportPortalException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">Reference to other error.</param>
        public ReportPortalException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
