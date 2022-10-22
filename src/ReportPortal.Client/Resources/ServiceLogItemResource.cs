using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceLogItemResource : ServiceBaseResource, ILogItemResource
    {
        public ServiceLogItemResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public Task<Content<LogItemResponse>> GetAsync()
        {
            return GetAsync(filterOption: null, CancellationToken.None);
        }

        public Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption)
        {
            return GetAsync(filterOption, CancellationToken.None);
        }

        public async Task<Content<LogItemResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"{ProjectName}/log";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LogItemResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public Task<LogItemResponse> GetAsync(string uuid)
        {
            return GetAsync(uuid, CancellationToken.None);
        }

        public async Task<LogItemResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<LogItemResponse>($"{ProjectName}/log/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public Task<LogItemResponse> GetAsync(long id)
        {
            return GetAsync(id, CancellationToken.None);
        }

        public async Task<LogItemResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<LogItemResponse>($"{ProjectName}/log/{id}", cancellationToken).ConfigureAwait(false);
        }

        public Task<byte[]> GetBinaryDataAsync(string id)
        {
            return GetBinaryDataAsync(id, CancellationToken.None);
        }

        public async Task<byte[]> GetBinaryDataAsync(string id, CancellationToken cancellationToken)
        {
            return await GetAsBytesAsync($"data/{ProjectName}/{id}", cancellationToken).ConfigureAwait(false);
        }

        public Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request)
        {
            return CreateAsync(request, CancellationToken.None);
        }

        public async Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken)
        {
            var uri = $"{ProjectName}/log";

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
            var uri = $"{ProjectName}/log";

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

        public Task<MessageResponse> DeleteAsync(long id)
        {
            return DeleteAsync(id, CancellationToken.None);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/log/{id}", cancellationToken).ConfigureAwait(false);
        }
    }
}
