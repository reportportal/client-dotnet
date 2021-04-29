using ReportPortal.Client.Extentions;
using System;
#if NET45
using System.Net;
#endif
using System.Net.Http;
using System.Net.Http.Headers;

namespace ReportPortal.Client
{
    partial class Service
    {
        class HttpClientFactory : IHttpClientFactory
        {
            private Uri _baseUri;
            private string _token;

            public HttpClientFactory(Uri baseUri, string token)
            {
                _baseUri = baseUri;
                _token = token;
            }

            public HttpClient Create()
            {
                var httpClientHandler = new HttpClientHandler();

#if !NET45
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    errors = System.Net.Security.SslPolicyErrors.None;

                    return true;
                };
#else
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                {
                    sslPolicyErrors = System.Net.Security.SslPolicyErrors.None;

                    return true;
                };;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#endif

                var httpClient = new HttpClient(httpClientHandler);

                httpClient.BaseAddress = _baseUri.Normalize();

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Reporter");

                return httpClient;
            }
        }
    }
}
