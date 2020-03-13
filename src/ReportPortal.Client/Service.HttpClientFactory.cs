using ReportPortal.Client.Extentions;
using System;
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
                var httpClient = new HttpClient();

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
