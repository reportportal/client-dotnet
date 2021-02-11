using ReportPortal.Client;
using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;
using System;

namespace ReportPortal.Shared.Reporter.Http
{
    /// <summary>
    /// Builder for <see cref="IClientService"/> instance with configuration.
    /// </summary>
    public class ClientServiceBuilder
    {
        private readonly IConfiguration _configuraion;

        private HttpClientHandlerFactory _httpClientHandlerFactory;

        private HttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constructor to create an instance of <see cref="ClientServiceBuilder"/> class.
        /// </summary>
        /// <param name="configuration">Well known list of properties.</param>
        public ClientServiceBuilder(IConfiguration configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            _configuraion = configuration;
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
            var url = _configuraion.GetValue<string>(ConfigurationPath.ServerUrl);

            var project = _configuraion.GetValue<string>(ConfigurationPath.ServerProject);

            var token = _configuraion.GetValue<string>(ConfigurationPath.ServerAuthenticationUuid);

            if (_httpClientHandlerFactory is null)
            {
                _httpClientHandlerFactory = new HttpClientHandlerFactory(_configuraion);
            }

            if (_httpClientFactory is null)
            {
                _httpClientFactory = new HttpClientFactory(_configuraion, _httpClientHandlerFactory.Create());
            }

            IClientService service = new Service(new Uri(url), project, token, _httpClientFactory);

            return service;
        }
    }
}
