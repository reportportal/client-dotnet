using ReportPortal.Client;
using ReportPortal.Client.Extentions;
using ReportPortal.Shared.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ReportPortal.Shared.Reporter.Http
{
    /// <summary>
    /// Class to create <see cref="HttpClient"/> instance based on <see cref="IConfiguration"/> object.
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="HttpClientFactory"/> class.
        /// </summary>
        /// <param name="configuration">Flatten configuration values.</param>
        /// <param name="httpClientHandler">Inner <see cref="HttpClientHandler"/> to use by <see cref="HttpClient"/>.</param>
        public HttpClientFactory(IConfiguration configuration, HttpClientHandler httpClientHandler)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            if (httpClientHandler is null) throw new ArgumentNullException(nameof(httpClientHandler));

            Configuration = configuration;
            HttpClientHandler = httpClientHandler;
        }

        /// <summary>
        /// Flatten configuration values.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Inner http client handler to use.
        /// </summary>
        protected HttpClientHandler HttpClientHandler { get; }

        /// <summary>
        /// Parses all well known configuration values and returns new instance of <see cref="HttpClient"/> class.
        /// </summary>
        /// <returns></returns>
        public virtual HttpClient Create()
        {
            var httpClient = new HttpClient(HttpClientHandler);

            var url = Configuration.GetValue<string>(ConfigurationPath.ServerUrl);

            var token = Configuration.GetValue<string>(ConfigurationPath.ServerAuthenticationUuid);

            httpClient.BaseAddress = new Uri(url);

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Reporter");

            var timeout = GetTimeout();
            if (timeout.HasValue)
            {
                httpClient.Timeout = timeout.Value;
            }

            return httpClient;
        }

        /// <summary>
        /// Parses timeout in configuration (in seconds).
        /// </summary>
        /// <returns></returns>
        protected virtual TimeSpan? GetTimeout()
        {
            TimeSpan? timeout = null;

            var seconds = Configuration.GetValue("Server:Timeout", double.NaN);

            if (!double.IsNaN(seconds))
            {
                timeout = TimeSpan.FromSeconds(seconds);
            }

            return timeout;
        }
    }
}
