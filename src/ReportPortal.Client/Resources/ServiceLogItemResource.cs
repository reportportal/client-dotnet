using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceLogItemResource : ServiceBaseResource, ILogItemResource
    {
        public ServiceLogItemResource(HttpClient httpClient, string project) : base(httpClient, project)
        {

        }

        public Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/log";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return GetAsJsonAsync<Content<LogItemResponse>>(uri);
        }

        public Task<LogItemResponse> GetAsync(string uuid)
        {
            return GetAsJsonAsync<LogItemResponse>($"{ProjectName}/log/uuid/{uuid}");
        }

        public Task<LogItemResponse> GetAsync(long id)
        {
            return GetAsJsonAsync<LogItemResponse>($"{ProjectName}/log/{id}");
        }

        public Task<byte[]> GetBinaryDataAsync(string id)
        {
            return GetAsBytesAsync($"data/{ProjectName}/{id}");
        }

        public async Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request)
        {
            var uri = $"{ProjectName}/log";

            if (request.Attach == null)
            {
                return await PostAsJsonAsync<LogItemCreatedResponse, CreateLogItemRequest>(uri, request);
            }
            else
            {
                var results = await CreateAsync(new CreateLogItemRequest[] { request });
                return results.LogItems.First();
            }
        }

        public Task<LogItemsCreatedResponse> CreateAsync(params CreateLogItemRequest[] requests)
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

            return SendHttpRequestAsync<LogItemsCreatedResponse>(HttpMethod.Post, uri, multipartContent);
        }


        public Task<MessageResponse> DeleteAsync(long id)
        {
            return DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/log/{id}");
        }
    }
}
