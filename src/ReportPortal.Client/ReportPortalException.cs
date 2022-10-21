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
        /// <param name="httpStatusCode">Response HTTP status code.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="responseBody">Response body.</param>
        public ReportPortalException(HttpStatusCode httpStatusCode, Uri requestUri, HttpMethod httpMethod, string responseBody)
            : base($"Response status code does not indicate success: {httpStatusCode} ({(int)httpStatusCode}) {httpMethod} {requestUri}")
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
            HttpMethod = httpMethod;
            ResponseBody = responseBody;
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

        /// <summary>
        /// Gets response body.
        /// </summary>
        public string ResponseBody { get; }
    }
}
