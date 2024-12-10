using ReportPortal.Client.Converters;
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

        protected Task<TResponse> GetAsJsonAsync<TResponse>(string uri, CancellationToken cancellationToken)
        {
            return SendAsJsonAsync<TResponse, object>(HttpMethod.Get, uri, null, cancellationToken: cancellationToken);
        }

        protected Task<TResponse> GetAsJsonAsync<TResponse>(string uri, string contentType, CancellationToken cancellationToken)
        {
            return SendAsJsonAsync<TResponse, object>(HttpMethod.Get, uri, null, contentType, cancellationToken);
        }

        protected Task<TResponse> PostAsJsonAsync<TResponse, TRequest>(
            string uri, TRequest request, CancellationToken cancellationToken)
        {
            return SendAsJsonAsync<TResponse, TRequest>(HttpMethod.Post, uri, request, cancellationToken: cancellationToken);
        }

        protected Task<TResponse> PostAsJsonAsync<TResponse, TRequest>(
            string uri, TRequest request, string contentType, CancellationToken cancellationToken)
        {
            return SendAsJsonAsync<TResponse, TRequest>(HttpMethod.Post, uri, request, contentType, cancellationToken);
        }

        protected Task<TResponse> PutAsJsonAsync<TResponse, TRequest>(
            string uri, TRequest request, CancellationToken cancellationToken)
        {
            return SendAsJsonAsync<TResponse, TRequest>(HttpMethod.Put, uri, request, cancellationToken: cancellationToken);
        }

        protected Task<TResponse> DeleteAsJsonAsync<TResponse>(string uri, CancellationToken cancellationToken)
        {
            return SendAsJsonAsync<TResponse, object>(HttpMethod.Delete, uri, null, cancellationToken: cancellationToken);
        }

        private async Task<TResponse> SendAsJsonAsync<TResponse, TRequest>(
            HttpMethod httpMethod, string uri, TRequest request, string contentType = "application/json", CancellationToken cancellationToken = default)
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

                    return await SendHttpRequestAsync<TResponse>(httpMethod, uri, httpContent, contentType, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                return await SendHttpRequestAsync<TResponse>(httpMethod, uri, httpContent, contentType, cancellationToken).ConfigureAwait(false);
            }
        }

        protected async Task<TResponse> SendHttpRequestAsync<TResponse>(
            HttpMethod httpMethod, string uri, HttpContent httpContent, string contentType = "application/json", CancellationToken cancellationToken = default)
        {
            using (var httpRequest = new HttpRequestMessage(httpMethod, uri))
            {
                using (httpContent)
                {
                    httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
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

        protected async Task<byte[]> GetAsBytesAsync(string uri, CancellationToken cancellationToken)
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
                    string responseBody = reader.ReadToEnd();

                    throw new ServiceException(
                        "Response status code does not indicate success.",
                        response.StatusCode,
                        response.RequestMessage.RequestUri,
                        response.RequestMessage.Method,
                        responseBody);
                }
            }
        }
    }
}
