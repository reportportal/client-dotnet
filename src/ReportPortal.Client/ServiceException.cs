using System;
using System.Net;
using System.Net.Http;

namespace ReportPortal.Client
{
    /// <summary>
    /// Occurs when server cannot process a request.
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ServiceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="httpStatusCode">Response HTTP status code.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="responseBody">Response body.</param>
        public ServiceException(string message, HttpStatusCode httpStatusCode, Uri requestUri, HttpMethod httpMethod, string responseBody)
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
            HttpMethod = httpMethod;
            ResponseBody = responseBody;

            _message = $"{message}\n {httpStatusCode} ({(int)httpStatusCode}) {httpMethod} {requestUri}\n {responseBody}";
        }

        private readonly string _message;

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
        
        /// <inheritdoc/>
        public override string Message => _message;
    }
}
