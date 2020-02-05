using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Extentions;

namespace ReportPortal.Client
{
    /// <inheritdoc/>
    public partial class Service : IClientService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="projectName">A project to manage.</param>
        /// <param name="token">A password for user. Can be UID given from user's profile page.</param>
        public Service(Uri uri, string projectName, string token) : this(uri, projectName, token, null)
        {

        }

        /// <summary>
        /// Constructor to initialize a new object of service.
        /// </summary>
        /// <param name="uri">Base URI for REST service.</param>
        /// <param name="projectName">A project to manage.</param>
        /// <param name="token">A password for user. Can be UID given from user's profile page.</param>
        /// <param name="proxy">Proxy for all HTTP requests.</param>
        public Service(Uri uri, string projectName, string token, IWebProxy proxy)
        {
            if (proxy != null)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                    UseProxy = true
                };

                _httpClient = new HttpClient(httpClientHandler);
            }
            else
            {
                _httpClient = new HttpClient();
            }

            if (!uri.LocalPath.ToLowerInvariant().Contains("api/v1"))
            {
                uri = uri.Append("api/v1");
            }
            _httpClient.BaseAddress = uri;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Reporter");

            if (proxy != null)
            { }

            BaseUri = uri;
            ProjectName = projectName;
            Token = token;

#if NET45
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#endif
        }

        /// <summary>
        /// Timeout for http requests.
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                return _httpClient.Timeout;
            }
            set
            {
                _httpClient.Timeout = value;
            }
        }

        /// <summary>
        /// Get or set project name to interact with.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Base api uri for http requests.
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// User token to interact with api.
        /// </summary>
        public string Token { get; set; }

        public ILaunchResource Launch
        {
            get
            {
                return new ServiceLaunchResource(_httpClient, BaseUri, ProjectName);
            }
        }

        public ITestItemResource TestItem
        {
            get
            {
                return new ServiceTestItemResource(_httpClient, BaseUri, ProjectName);
            }
        }

        public ILogItemResource LogItem
        {
            get
            {
                return new ServiceLogItemResource(_httpClient, BaseUri, ProjectName);
            }
        }

        public IUserResource User
        {
            get
            {
                return new ServiceUserResource(_httpClient, BaseUri, ProjectName);
            }
        }

        public IUserFilterResource UserFilter
        {
            get
            {
                return new ServiceUserFilterResource(_httpClient, BaseUri, ProjectName);
            }
        }

        public IProjectResource Project
        {
            get
            {
                return new ServiceProjectResource(_httpClient, BaseUri, ProjectName);
            }
        }
    }
}
