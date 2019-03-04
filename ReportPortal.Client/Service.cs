using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ReportPortal.Client.Clients;
using ReportPortal.Client.Extentions;

namespace ReportPortal.Client
{
    /// <summary>
    /// Class to interact with common Report Portal services. Provides possibility to manage almost of service's entities.
    /// </summary>
    public class Service
    {
        private readonly HttpClient _httpClient;

        public LaunchClient LaunchClient => new LaunchClient(_httpClient, BaseUri, Project);
        public LogItemClient LogItemClient => new LogItemClient(_httpClient, BaseUri, Project);
        public ProjectClient ProjectClient => new ProjectClient(_httpClient, BaseUri, Project);
        public TestItemClient TestItemClient => new TestItemClient(_httpClient, BaseUri, Project);
        public UserClient UserClient => new UserClient(_httpClient, BaseUri, Project);
        public UserFilterClient UserFilterClient => new UserFilterClient(_httpClient, BaseUri, Project);

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
            Project = project;

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
        public Service(Uri uri, string project, string password)
            : this(uri, project, password, new RetryWithExponentialBackoffHttpClientHandler(3))
        {
        }

        /// <summary>
        /// Get or set project name to interact with.
        /// </summary>
        public string Project { get; set; }

        public Uri BaseUri { get; set; }
    }
}
