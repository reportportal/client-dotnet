using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ReportPortal.Client.Api.Filter;
using ReportPortal.Client.Api.Launch;
using ReportPortal.Client.Api.Log;
using ReportPortal.Client.Api.Project;
using ReportPortal.Client.Api.TestItem;
using ReportPortal.Client.Api.User;
using ReportPortal.Client.Extention;

namespace ReportPortal.Client
{
    /// <summary>
    /// Class to interact with common Report Portal services. Provides possibility to manage almost of service's entities.
    /// </summary>
    public sealed class ReportPortalClient : IReportPortalClient
    {
        private readonly HttpClient _httpClient;

        public ILaunchApiClient Launch => new LaunchApiClient(_httpClient, BaseUri, ProjectName);
        public ILogApiClient Log => new LogApiClient(_httpClient, BaseUri, ProjectName);
        public IProjectApiClient Project => new ProjectApiClient(_httpClient, BaseUri, ProjectName);
        public ITestItemApiClient TestItem => new TestItemApiClient(_httpClient, BaseUri, ProjectName);
        public IUserApiClient User => new UserApiClient(_httpClient, BaseUri, ProjectName);
        public IFilterApiClient Filter => new FilterApiClient(_httpClient, BaseUri, ProjectName);

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="project">A project to manage.</param>
        /// <param name="password">A password for user. Can be UID given from user's profile page.</param>
        /// <param name="messageHandler">The HTTP handler to use for sending all requests.</param>
        public ReportPortalClient(Uri uri, string project, string password, HttpMessageHandler messageHandler)
        {
            _httpClient = new HttpClient(messageHandler);
            
            if (!uri.LocalPath.ToUpperInvariant().Contains("API/V1"))
            {
                uri = uri.Append("api/v1");
            }
            _httpClient.BaseAddress = uri;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + password);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Reporter");
            BaseUri = uri;
            ProjectName = project;

#if NET45
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#endif
        }

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="project">A project to manage.</param>
        /// <param name="password">A password for user. Can be UID given from user's profile page.</param>
        public ReportPortalClient(Uri uri, string project, string password)
            : this(uri, project, password, new RetryWithExponentialBackoffHttpClientHandler(3))
        {
        }

        /// <summary>
        /// Get or set project name to interact with.
        /// </summary>
        public string ProjectName { get; set; }

        public Uri BaseUri { get; set; }
    }
}
