using ReportPortal.Client;
using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Internal.Logging;
using System;

namespace ReportPortal.Shared.Reporter.Http
{
    /// <summary>
    /// Builder for <see cref="IClientService"/> instance with configuration.
    /// </summary>
    public class ClientServiceBuilder
    {
        private static ITraceLogger TraceLogger => TraceLogManager.Instance.GetLogger<ClientServiceBuilder>();

        private readonly IConfiguration _configuration;

        private HttpClientHandlerFactory _httpClientHandlerFactory;

        private HttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constructor to create an instance of <see cref="ClientServiceBuilder"/> class.
        /// </summary>
        /// <param name="configuration">Well known list of properties.</param>
        public ClientServiceBuilder(IConfiguration configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;
        }

        /// <summary>
        /// Sets <see cref="HttpClientHandlerFactory"/> instance to be used for building Web API client.
        /// </summary>
        /// <param name="httpClientHandlerFactory"></param>
        /// <returns></returns>
        public ClientServiceBuilder UseHttpClientHandlerFactory(HttpClientHandlerFactory httpClientHandlerFactory)
        {
            _httpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpClientFactory"/> instance to be used for building Web API client.
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <returns></returns>
        public ClientServiceBuilder UseHttpClientFactory(HttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            return this;
        }

        /// <summary>
        /// Parses configuration and builds an instance of <see cref="IClientService"/>.
        /// </summary>
        /// <returns>Client to interact with Web API.</returns>
        public IClientService Build()
        {
            var url = _configuration.GetValue<string>(ConfigurationPath.ServerUrl);

            var project = _configuration.GetValue<string>(ConfigurationPath.ServerProject);

            var apiKey = _configuration.GetValue<string>(ConfigurationPath.ServerAuthenticationKey, null);
            if (apiKey is null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                apiKey = _configuration.GetValue<string>(ConfigurationPath.ServerAuthenticationUuid, null);
#pragma warning restore CS0618 // Type or member is obsolete
                if (apiKey is null)
                {
                    // Trigger proper exception throwing or use 'null'.
                    apiKey = _configuration.GetValue<string>(ConfigurationPath.ServerAuthenticationKey);
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    TraceLogger.Warn($"Configuration parameter '${ConfigurationPath.ServerAuthenticationUuid}' is deprecated. " +
                        $"Use '${ConfigurationPath.ServerAuthenticationKey}' instead.");
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }

            if (_httpClientHandlerFactory is null)
            {
                _httpClientHandlerFactory = new HttpClientHandlerFactory(_configuration);
            }

            if (_httpClientFactory is null)
            {
                _httpClientFactory = new HttpClientFactory(_configuration, _httpClientHandlerFactory.Create());
            }

            IClientService service = new Service(new Uri(url), project, apiKey, _httpClientFactory);

            return service;
        }
    }
}
