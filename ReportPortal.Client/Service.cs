using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using ReportPortal.Client.Extentions;

namespace ReportPortal.Client
{
    /// <summary>
    /// Class to interact with common Report Portal services. Provides possibility to manage almost of service's entities.
    /// </summary>
    public partial class Service
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="project">A project to manage.</param>
        /// <param name="password">A password for user. Can be UID given from user's profile page.</param>
        /// <param name="messageHandler">The HTTP handler to use for sending all requests.</param>
        public Service(Uri uri, string project, string password, HttpMessageHandler messageHandler)
        {
            _httpClient = new HttpClient(messageHandler);
            _httpClient.BaseAddress = uri;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + password);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Reporter");
            BaseUri = uri;
            Project = project;

#if NET45
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
#endif
        }

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="project">A project to manage.</param>
        /// <param name="password">A password for user. Can be UID given from user's profile page.</param>
        public Service(Uri uri, string project, string password)
            : this(uri, project, password, new RetryWithExponentialBackoffHttpClientHandler(3))
        {
        }

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="project">A project to manage.</param>
        /// <param name="password">A password for user. Can be UID given from user's profile page.</param>
        /// <param name="proxy">Proxy for all HTTP requests.</param>
        public Service(Uri uri, string project, string password, IWebProxy proxy)
            : this(uri, project, password, new RetryWithExponentialBackoffHttpClientHandler(3, proxy))
        {
        }

        /// <summary>
        /// Get or set project name to interact with.
        /// </summary>
        public string Project { get; set; }

        public Uri BaseUri { get; set; }
    }

    public class RetryWithExponentialBackoffHttpClientHandler : DelegatingHandler
    {
        public int MaxRetries { get; private set; }

        public RetryWithExponentialBackoffHttpClientHandler(int maxRetries)
            : this(maxRetries, new HttpClientHandler())
        {
        }

        public RetryWithExponentialBackoffHttpClientHandler(int maxRetries, IWebProxy proxy)
            : this(maxRetries, new HttpClientHandler { Proxy = proxy })
        {
        }

        public RetryWithExponentialBackoffHttpClientHandler(int maxRetries, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            MaxRetries = maxRetries;
        }

        private List<HttpStatusCode> ResponseStatusCodesForRetrying => new List<HttpStatusCode> { HttpStatusCode.InternalServerError, HttpStatusCode.NotImplemented, HttpStatusCode.BadGateway, HttpStatusCode.ServiceUnavailable, HttpStatusCode.GatewayTimeout, HttpStatusCode.HttpVersionNotSupported };

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);

                    if (!ResponseStatusCodesForRetrying.Contains(response.StatusCode))
                    {
                        return response;
                    }
                    else
                    {
                        response.VerifySuccessStatusCode();
                    }
                }

                catch (Exception exp) when (exp is TaskCanceledException || exp is HttpRequestException)
                {
                    if (i < MaxRetries - 1)
                    {
                        await Task.Delay((int)Math.Pow(2, i + MaxRetries) * 1000);
                    }

                    else
                    {
                        throw;
                    }
                }
            }

            return response;
        }
    }
}
