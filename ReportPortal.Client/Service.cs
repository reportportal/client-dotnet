﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using ReportPortal.Client.Extentions;
using System.Collections.Generic;

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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
        }

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="project">A project to manage.</param>
        /// <param name="password">A password for user. Can be UID given from user's profile page.</param>
        public Service(Uri uri, string project, string password)
            : this(uri, project, password, new RetryWithExponentialBackoffHttpClientHandler())
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
            : this(uri, project, password, new RetryWithExponentialBackoffHttpClientHandler(proxy))
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
        public RetryWithExponentialBackoffHttpClientHandler()
            : this(new HttpClientHandler())
        {
        }

        public RetryWithExponentialBackoffHttpClientHandler(IWebProxy proxy)
            : this(new HttpClientHandler { Proxy = proxy })
        {
        }

        public RetryWithExponentialBackoffHttpClientHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            var serverErrorResponseCodes = new List<HttpStatusCode> { HttpStatusCode.InternalServerError, HttpStatusCode.NotImplemented, HttpStatusCode.BadGateway, HttpStatusCode.ServiceUnavailable, HttpStatusCode.GatewayTimeout, HttpStatusCode.HttpVersionNotSupported };
            int loopCount = 3;

            for (int i = 0; i < loopCount; i++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);
                    if (!serverErrorResponseCodes.Contains(response.StatusCode))
                    {
                        return response;
                    }

                    await Task.Delay((int)Math.Pow(2, i + loopCount) * 1000);
                }

                catch (Exception exp) when (exp is TaskCanceledException || exp is HttpRequestException)
                {
                    if (i < loopCount - 1)
                    {
                        await Task.Delay((int) Math.Pow(2, i + loopCount) * 1000);
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