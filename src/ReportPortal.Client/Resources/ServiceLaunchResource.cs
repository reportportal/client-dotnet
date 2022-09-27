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

        public async ValueTask<LaunchResponse> GetAsync(string uuid)
        {
            return await GetAsync(uuid, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<LaunchResponse>($"{ProjectName}/launch/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<LaunchResponse> GetAsync(long id)
        {
            return await GetAsync(id, CancellationToken.None);
        }

        public async ValueTask<LaunchResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<LaunchResponse>($"{ProjectName}/launch/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            return await StartAsync(request, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<LaunchCreatedResponse, StartLaunchRequest>(
                $"{ProjectName}/launch", request, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request)
        {
            return await FinishAsync(uuid, request, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>(
                $"{ProjectName}/launch/{uuid}/finish", request, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request)
        {
            return await StopAsync(id, request, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>(
                $"{ProjectName}/launch/{id}/stop", request, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/launch/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<LaunchResponse> MergeAsync(MergeLaunchesRequest request)
        {
            return await MergeAsync(request, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<LaunchResponse, MergeLaunchesRequest>(
                $"{ProjectName}/launch/merge", request, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request)
        {
            return await UpdateAsync(id, request, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateLaunchRequest>(
                $"{ProjectName}/launch/{id}/update", request, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request)
        {
            return await AnalyzeAsync(request, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<MessageResponse, AnalyzeLaunchRequest>(
                $"{ProjectName}/launch/analyze", request, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetAsync()
        {
            return await GetAsync(filterOption: null, CancellationToken.None).ConfigureAwait(false);
        }
        
        public async ValueTask<Content<LaunchResponse>> GetAsync(FilterOption filterOption)
        {
            return await GetAsync(filterOption, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"{ProjectName}/launch";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetDebugAsync()
        {
            return await GetDebugAsync(filterOption: null, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption)
        {
            return await GetDebugAsync(filterOption, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetDebugAsync(CancellationToken cancellationToken)
        {
            return await GetDebugAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"{ProjectName}/launch/mode";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }
    }
}
