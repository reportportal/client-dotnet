using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceAsyncLogItemResource : ServiceBaseResource, IAsyncLogItemResource
    {
        public ServiceAsyncLogItemResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request)
        {
            return CreateAsync(request, CancellationToken.None);
        }

        public async Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken)
        {
            var uri = $"v2/{ProjectName}/log";

            if (request.Attach == null)
            {
                return await PostAsJsonAsync<LogItemCreatedResponse, CreateLogItemRequest>(uri, request, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var results = await CreateAsync(new CreateLogItemRequest[] { request }, cancellationToken).ConfigureAwait(false);
                return results.LogItems.First();
            }
        }

        public Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests)
        {
            return CreateAsync(requests, CancellationToken.None);
        }

        public async Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests, CancellationToken cancellationToken)
        {
            var uri = $"v2/{ProjectName}/log";

            var multipartContent = new MultipartFormDataContent();

            using (var memoryStream = new MemoryStream())
            {
                await ModelSerializer.SerializeAsync<CreateLogItemRequest[]>(requests, memoryStream, cancellationToken).ConfigureAwait(false);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var httpContent = new StreamContent(memoryStream);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                multipartContent.Add(httpContent, "json_request_part");

                foreach (var request in requests)
                {
                    if (request.Attach != null)
                    {
                        var byteArrayContent = new ByteArrayContent(request.Attach.Data, 0, request.Attach.Data.Length);
                        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(request.Attach.MimeType);
                        multipartContent.Add(byteArrayContent, "file", request.Attach.Name);
                    }
                }

                return await SendHttpRequestAsync<LogItemsCreatedResponse>(HttpMethod.Post, uri, multipartContent, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
