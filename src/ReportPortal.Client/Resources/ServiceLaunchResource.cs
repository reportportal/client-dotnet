using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceLaunchResource : ServiceBaseResource, ILaunchResource
    {
        public ServiceLaunchResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public async Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption = null, CancellationToken cancellationToken = default)
        {
            var uri = $"{ProjectName}/launch";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption = null, CancellationToken cancellationToken = default)
        {
            var uri = $"{ProjectName}/launch/mode";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<LaunchResponse> GetAsync(string uuid, CancellationToken cancellationToken = default)
        {
            return await GetAsJsonAsync<LaunchResponse>($"{ProjectName}/launch/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<LaunchResponse> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await GetAsJsonAsync<LaunchResponse>($"{ProjectName}/launch/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken = default)
        {
            return await PostAsJsonAsync<LaunchCreatedResponse, StartLaunchRequest>(
                $"{ProjectName}/launch", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken = default)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>(
                $"{ProjectName}/launch/{uuid}/finish", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request, CancellationToken cancellationToken = default)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>(
                $"{ProjectName}/launch/{id}/stop", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/launch/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken = default)
        {
            return await PostAsJsonAsync<LaunchResponse, MergeLaunchesRequest>(
                $"{ProjectName}/launch/merge", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request, CancellationToken cancellationToken = default)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateLaunchRequest>(
                $"{ProjectName}/launch/{id}/update", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request, CancellationToken cancellationToken = default)
        {
            return await PostAsJsonAsync<MessageResponse, AnalyzeLaunchRequest>(
                $"{ProjectName}/launch/analyze", request, cancellationToken).ConfigureAwait(false);
        }
    }
}
