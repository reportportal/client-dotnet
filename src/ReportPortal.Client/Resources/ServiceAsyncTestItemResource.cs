using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceAsyncTestItemResource : ServiceBaseResource, IAsyncTestItemResource
    {
        public ServiceAsyncTestItemResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"v2/{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"v2/{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, FinishTestItemRequest>(
                $"v2/{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }
    }
}
