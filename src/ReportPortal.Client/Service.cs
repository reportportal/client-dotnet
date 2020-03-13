using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Resources;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ReportPortal.Client
{
    /// <inheritdoc/>
    public partial class Service : IClientService, IDisposable
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


            _httpClient.BaseAddress = uri.Normalize();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Reporter");

            ProjectName = projectName;
            Token = token;

#if NET45
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#endif

            Launch = new ServiceLaunchResource(_httpClient, ProjectName);
            TestItem = new ServiceTestItemResource(_httpClient, ProjectName);
            LogItem = new ServiceLogItemResource(_httpClient, ProjectName);
            User = new ServiceUserResource(_httpClient, ProjectName);
            UserFilter = new ServiceUserFilterResource(_httpClient, ProjectName);
            Project = new ServiceProjectResource(_httpClient, ProjectName);
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
        public string ProjectName { get; }

        /// <summary>
        /// User token to interact with api.
        /// </summary>
        public string Token { get; }

        /// <inheritdoc/>
        public ILaunchResource Launch { get; }

        /// <inheritdoc/>
        public ITestItemResource TestItem { get; }

        /// <inheritdoc/>
        public ILogItemResource LogItem { get; }

        /// <inheritdoc/>
        public IUserResource User { get; }

        /// <inheritdoc/>
        public IUserFilterResource UserFilter { get; }

        /// <inheritdoc/>
        public IProjectResource Project { get; }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
