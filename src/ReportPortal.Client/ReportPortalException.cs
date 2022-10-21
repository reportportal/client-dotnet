using System;
using System.Net;
using System.Net.Http;

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

        /// <summary>
        /// Initializes a new instance of <see cref="ReportPortalException"/> class.
        /// </summary>
        /// <param name="statusCode">Response status code.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="method">HTTP request method.</param>
        /// <param name="innerException">Reference to other error.</param>
        public ReportPortalException(HttpStatusCode statusCode, Uri requestUri, HttpMethod method, Exception innerException)
            : base($"Response status code does not indicate success: {statusCode} ({(int)statusCode}) {method} {requestUri}", innerException)
        {
            HttpStatusCode = statusCode;
            RequestUri = requestUri;
            HttpMethod = method;
        }

        /// <summary>
        /// Gets HTTP status code.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        /// Gets request uri.
        /// </summary>
        public Uri RequestUri { get; }

        /// <summary>
        /// Gets HTTP method.
        /// </summary>
        public HttpMethod HttpMethod { get; }
    }
}
