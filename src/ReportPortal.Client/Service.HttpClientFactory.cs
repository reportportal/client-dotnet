using ReportPortal.Client.Extentions;
using System;
#if NET462
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
            private readonly bool _ignoreSslErrors;

            public HttpClientFactory(Uri baseUri, string token, bool ignoreSslErrors)
            {
                _baseUri = baseUri;
                _token = token;
                _ignoreSslErrors = ignoreSslErrors;
            }

            public HttpClient Create()
            {
                var httpClientHandler = new HttpClientHandler();

#if !NET462
                if (_ignoreSslErrors)
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => _ignoreSslErrors;
                }
#else
                if (_ignoreSslErrors)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                }
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
