using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Resources;
using System;
#if NET45
using System.Net;
#endif
using System.Net.Http;

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
        /// <param name="token">A token for user. Can be UID given from user's profile page.</param>
        /// <param name="httpClientFactory">Factory object to create an instance of <see cref="HttpClient"/>.</param>
        public Service(Uri uri, string projectName, string token, IHttpClientFactory httpClientFactory = null)
        {
            ProjectName = projectName;

            if (httpClientFactory == null)
            {
                httpClientFactory = new HttpClientFactory(uri, token);
            }

            _httpClient = httpClientFactory.Create();

#if NET45
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
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
        /// Get or set project name to interact with.
        /// </summary>
        public string ProjectName { get; }

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
