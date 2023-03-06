using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Resources;
using System;
using System.Net.Http;

namespace ReportPortal.Client
{
    /// <inheritdoc cref="IClientService"/>
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

            Launch = new ServiceLaunchResource(_httpClient, ProjectName);
            AsyncLaunch = new ServiceAsyncLaunchResource(_httpClient, ProjectName);
            TestItem = new ServiceTestItemResource(_httpClient, ProjectName);
            AsyncTestItem = new ServiceAsyncTestItemResource(_httpClient, ProjectName);
            LogItem = new ServiceLogItemResource(_httpClient, ProjectName);
            User = new ServiceUserResource(_httpClient, ProjectName);
            UserFilter = new ServiceUserFilterResource(_httpClient, ProjectName);
            Project = new ServiceProjectResource(_httpClient, ProjectName);
        }

        /// <summary>
        /// Gets current project name to interact with.
        /// </summary>
        public string ProjectName { get; }

        /// <inheritdoc cref="ILaunchResource"/>
        public ILaunchResource Launch { get; }

        /// <inheritdoc cref="IAsyncLaunchResource"/>
        public IAsyncLaunchResource AsyncLaunch { get; }

        /// <inheritdoc cref="ITestItemResource"/>
        public ITestItemResource TestItem { get; }

        /// <inheritdoc cref="IAsyncTestItemResource"/>
        public IAsyncTestItemResource AsyncTestItem { get; }

        /// <inheritdoc cref="ILogItemResource"/>
        public ILogItemResource LogItem { get; }

        /// <inheritdoc cref="IUserResource"/>
        public IUserResource User { get; }

        /// <inheritdoc cref="IUserFilterResource"/>
        public IUserFilterResource UserFilter { get; }

        /// <inheritdoc cref="IProjectResource"/>
        public IProjectResource Project { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
