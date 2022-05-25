using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceLaunchResource : ServiceBaseResource, ILaunchResource
    {
        public ServiceLaunchResource(HttpClient httpClient, string project) : base(httpClient, project)
        {

        }

        public async Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/launch";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri).ConfigureAwait(false);
        }

        public async Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/launch/mode";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri).ConfigureAwait(false);
        }

        public async Task<LaunchResponse> GetAsync(string uuid)
        {
            return await GetAsJsonAsync<LaunchResponse>($"{ProjectName}/launch/uuid/{uuid}").ConfigureAwait(false);
        }

        public async Task<LaunchResponse> GetAsync(long id)
        {
            return await GetAsJsonAsync<LaunchResponse>($"{ProjectName}/launch/{id}").ConfigureAwait(false);
        }

        public async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            return await PostAsJsonAsync<LaunchCreatedResponse, StartLaunchRequest>($"{ProjectName}/launch", request).ConfigureAwait(false);
        }

        public async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>($"{ProjectName}/launch/{uuid}/finish", request).ConfigureAwait(false);
        }

        public async Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>($"{ProjectName}/launch/{id}/stop", request).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/launch/{id}").ConfigureAwait(false);
        }

        public async Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request)
        {
            return await PostAsJsonAsync<LaunchResponse, MergeLaunchesRequest>($"{ProjectName}/launch/merge", request).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateLaunchRequest>($"{ProjectName}/launch/{id}/update", request).ConfigureAwait(false);
        }

        public async Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request)
        {
            return await PostAsJsonAsync<MessageResponse, AnalyzeLaunchRequest>($"{ProjectName}/launch/analyze", request).ConfigureAwait(false);
        }
    }
}
