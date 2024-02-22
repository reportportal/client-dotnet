using ReportPortal.Shared.Configuration;
using System;
using System.Net;
using System.Net.Http;

namespace ReportPortal.Shared.Reporter.Http
{
    /// <summary>
    /// Class to create <see cref="HttpClientHandler"/> instance based on <see cref="IConfiguration"/> object.
    /// </summary>
    public class HttpClientHandlerFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="HttpClientHandlerFactory"/> class.
        /// </summary>
        /// <param name="configuration">Flatten configuration values.</param>
        public HttpClientHandlerFactory(IConfiguration configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }

        /// <summary>
        /// Flatten configuration values.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Parses all well known configuration values and returns new instance of <see cref="HttpClientHandler"/> class.
        /// </summary>
        /// <returns></returns>
        public virtual HttpClientHandler Create()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.Proxy = GetProxy();

            var ignoreSslErrors = Configuration.GetValue<bool>("Server:IgnoreSslErrors", false);

#if NET462
            if (ignoreSslErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#else
            if (ignoreSslErrors)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            }
#endif

            return httpClientHandler;
        }

        /// <summary>
        /// Identify whether proxy is configured.
        /// </summary>
        /// <returns>Object of <see cref="IWebProxy"/>. Null if proxy is not configured.</returns>
        protected virtual IWebProxy GetProxy()
        {
            WebProxy webProxy = null;

            var proxyUrl = Configuration.GetValue<string>("Server:Proxy:Url", null);

            if (proxyUrl != null)
            {
                webProxy = new WebProxy(proxyUrl);

                var username = Configuration.GetValue<string>("Server:Proxy:Username", null);

                if (username != null)
                {
                    var password = Configuration.GetValue<string>("Server:Proxy:Password", null);

                    var domain = Configuration.GetValue<string>("Server:Proxy:Domain", null);

                    var credential = new NetworkCredential(username, password, domain);

                    webProxy.Credentials = credential;
                }
            }

            return webProxy;
        }
    }
}
