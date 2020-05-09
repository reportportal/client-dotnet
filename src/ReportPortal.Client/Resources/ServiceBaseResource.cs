using ReportPortal.Client.Converters;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

        protected async Task<TResponse> GetAsJsonAsync<TResponse>(string uri)
        {
            return await SendAsJsonAsync<TResponse, object>(HttpMethod.Get, uri, null);
        }

        protected async Task<TResponse> PostAsJsonAsync<TResponse, TRequest>(string uri, TRequest request)
        {
            return await SendAsJsonAsync<TResponse, TRequest>(HttpMethod.Post, uri, request);
        }

        protected async Task<TResponse> PutAsJsonAsync<TResponse, TRequest>(string uri, TRequest request)
        {
            return await SendAsJsonAsync<TResponse, TRequest>(HttpMethod.Put, uri, request);
        }

        protected async Task<TResponse> DeleteAsJsonAsync<TResponse>(string uri)
        {
            return await SendAsJsonAsync<TResponse, object>(HttpMethod.Delete, uri, null);
        }

        private async Task<TResponse> SendAsJsonAsync<TResponse, TRequest>(HttpMethod httpMethod, string uri, TRequest request)
        {
            HttpContent httpContent = null;

            if (request != null)
            {
                var memoryStream = new MemoryStream();
                ModelSerializer.Serialize<TRequest>(request, memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(memoryStream);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            using (var httpRequest = new HttpRequestMessage(httpMethod, uri))
            {
                using (httpContent)
                {
                    httpRequest.Content = httpContent;

                    using (var response = await HttpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            await CheckSuccessStatusCodeAsync(response, stream).ConfigureAwait(false);

                            return ModelSerializer.Deserialize<TResponse>(stream);
                        }
                    }
                }
            }
        }

        protected async Task<byte[]> GetAsBytesAsync(string uri)
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                using (var response = await HttpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        await CheckSuccessStatusCodeAsync(response, stream).ConfigureAwait(false);

                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }

        private async Task CheckSuccessStatusCodeAsync(HttpResponseMessage response, Stream stream)
        {
            if (!response.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(stream))
                {
                    string body = await reader.ReadToEndAsync().ConfigureAwait(false);
                    throw new ReportPortalException($"Response status code does not indicate success: {response.StatusCode} ({(int)response.StatusCode}) {response.RequestMessage.Method} {response.RequestMessage.RequestUri}", new HttpRequestException($"Response message: {body}"));
                }
            }
        }
    }
}
