using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.Linq;
using System.Net.Http;
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

        public async Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption = null, CancellationToken cancellationToken = default)
        {
            var uri = $"{ProjectName}/log";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LogItemResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<LogItemResponse> GetAsync(string uuid, CancellationToken cancellationToken = default)
        {
            return await GetAsJsonAsync<LogItemResponse>($"{ProjectName}/log/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<LogItemResponse> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await GetAsJsonAsync<LogItemResponse>($"{ProjectName}/log/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<byte[]> GetBinaryDataAsync(string id, CancellationToken cancellationToken = default)
        {
            return await GetAsBytesAsync($"data/{ProjectName}/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken = default)
        {
            var uri = $"{ProjectName}/log";

            if (request.Attach == null)
            {
                return await PostAsJsonAsync<LogItemCreatedResponse, CreateLogItemRequest>(uri, request, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var results = await CreateAsync(cancellationToken, new CreateLogItemRequest[] { request }).ConfigureAwait(false);
                return results.LogItems.First();
            }
        }

        public async Task<LogItemsCreatedResponse> CreateAsync(CancellationToken cancellationToken = default, params CreateLogItemRequest[] requests)
        {
            var uri = $"{ProjectName}/log";

            var multipartContent = new MultipartFormDataContent();

            var body = ModelSerializer.Serialize<CreateLogItemRequest[]>(requests);

            var jsonContent = new StringContent(body, Encoding.UTF8, "application/json");
            multipartContent.Add(jsonContent, "json_request_part");

            foreach (var request in requests)
            {
                if (request.Attach != null)
                {
                    var byteArrayContent = new ByteArrayContent(request.Attach.Data, 0, request.Attach.Data.Length);
                    byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(request.Attach.MimeType);
                    multipartContent.Add(byteArrayContent, "file", request.Attach.Name);
                }
            }

            return await SendHttpRequestAsync<LogItemsCreatedResponse>(HttpMethod.Post, uri, multipartContent, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/log/{id}", cancellationToken).ConfigureAwait(false);
        }
    }
}
