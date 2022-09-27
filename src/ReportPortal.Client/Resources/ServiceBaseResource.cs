﻿using ReportPortal.Client.Converters;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    abstract class ServiceBaseResource
    {
        public ServiceBaseResource(HttpClient httpClient, string projectName)
        {
            HttpClient = httpClient;
            ProjectName = projectName;
        }

        protected HttpClient HttpClient { get; }

        protected string ProjectName { get; }

        protected async ValueTask<TResponse> GetAsJsonAsync<TResponse>(string uri, CancellationToken cancellationToken)
        {
            return await SendAsJsonAsync<TResponse, object>(HttpMethod.Get, uri, null, cancellationToken).ConfigureAwait(false);
        }

        protected async ValueTask<TResponse> PostAsJsonAsync<TResponse, TRequest>(
            string uri, TRequest request, CancellationToken cancellationToken)
        {
            return await SendAsJsonAsync<TResponse, TRequest>(HttpMethod.Post, uri, request, cancellationToken).ConfigureAwait(false);
        }

        protected async ValueTask<TResponse> PutAsJsonAsync<TResponse, TRequest>(
            string uri, TRequest request, CancellationToken cancellationToken)
        {
            return await SendAsJsonAsync<TResponse, TRequest>(HttpMethod.Put, uri, request, cancellationToken).ConfigureAwait(false);
        }

        protected async ValueTask<TResponse> DeleteAsJsonAsync<TResponse>(string uri, CancellationToken cancellationToken)
        {
            return await SendAsJsonAsync<TResponse, object>(HttpMethod.Delete, uri, null, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<TResponse> SendAsJsonAsync<TResponse, TRequest>(
            HttpMethod httpMethod, string uri, TRequest request, CancellationToken cancellationToken)
        {
            HttpContent httpContent = null;

            if (request != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ModelSerializer.SerializeAsync<TRequest>(request, memoryStream, cancellationToken).ConfigureAwait(false);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    httpContent = new StreamContent(memoryStream);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return await SendHttpRequestAsync<TResponse>(httpMethod, uri, httpContent, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                return await SendHttpRequestAsync<TResponse>(httpMethod, uri, httpContent, cancellationToken).ConfigureAwait(false);
            }
        }

        protected async ValueTask<TResponse> SendHttpRequestAsync<TResponse>(
            HttpMethod httpMethod, string uri, HttpContent httpContent, CancellationToken cancellationToken)
        {
            using (var httpRequest = new HttpRequestMessage(httpMethod, uri))
            {
                using (httpContent)
                {
                    httpRequest.Content = httpContent;

                    using (var response = await HttpClient
                        .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            CheckSuccessStatusCode(response, stream);

                            return await ModelSerializer.DeserializeAsync<TResponse>(stream, cancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        protected async ValueTask<byte[]> GetAsBytesAsync(string uri, CancellationToken cancellationToken)
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                using (var response = await HttpClient
                    .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        CheckSuccessStatusCode(response, stream);

                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }

        private void CheckSuccessStatusCode(HttpResponseMessage response, Stream stream)
        {
            if (!response.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(stream))
                {
                    string body = reader.ReadToEnd();
                    throw new ReportPortalException($"Response status code does not indicate success: {response.StatusCode} ({(int)response.StatusCode}) {response.RequestMessage.Method} {response.RequestMessage.RequestUri}", new HttpRequestException($"Response message: {body}"));
                }
            }
        }
    }
}
