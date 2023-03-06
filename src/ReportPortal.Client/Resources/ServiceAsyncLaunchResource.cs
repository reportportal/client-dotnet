using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceAsyncLaunchResource : ServiceBaseResource, IAsyncLaunchResource
    {
        public ServiceAsyncLaunchResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            return StartAsync(request, CancellationToken.None);
        }

        public async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<LaunchCreatedResponse, StartLaunchRequest>(
                $"v2/{ProjectName}/launch", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request)
        {
            return FinishAsync(uuid, request, CancellationToken.None);
        }

        public async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>(
                $"v2/{ProjectName}/launch/{uuid}/finish", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request)
        {
            return MergeAsync(request, CancellationToken.None);
        }

        public async Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<LaunchResponse, MergeLaunchesRequest>(
                $"v2/{ProjectName}/launch/merge", request, cancellationToken).ConfigureAwait(false);
        }
    }
}
