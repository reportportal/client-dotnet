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
        /// <param name="requestBody">Request body.</param>
        /// <param name="responseBody">Response body.</param>
        public ReportPortalException(HttpStatusCode httpStatusCode, Uri requestUri, HttpMethod httpMethod, string requestBody, string responseBody)
            : base($"Response status code does not indicate success: {httpStatusCode} ({(int)httpStatusCode})")
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
            HttpMethod = httpMethod;
            RequestBody = requestBody;
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
        /// Gets request body.
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// Gets response body.
        /// </summary>
        public string ResponseBody { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            

            return string.Join("\n", $"{GetType()}: {Message}",
                $"Request Uri: {RequestUri}",
                $"HTTP Method: {HttpMethod}",
                $"Request Body: {RequestBody ?? "<empty>"}",
                $"Response Body: {ResponseBody}",
                $"{StackTrace}");
        }
    }
}
