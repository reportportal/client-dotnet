using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Resources;

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

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Content<LaunchResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/launch/mode";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Content<LaunchResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<LaunchResponse> GetAsync(string uuid)
        {
            var uri = $"{ProjectName}/launch/uuid/{uuid}";
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<LaunchResponse> GetAsync(long id)
        {
            var uri = $"{ProjectName}/launch/{id}";
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            var uri = $"{ProjectName}/launch";
            var body = ModelSerializer.Serialize<StartLaunchRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request)
        {
            var uri = $"{ProjectName}/launch/{uuid}/finish";
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchFinishedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request)
        {
            var uri = $"{ProjectName}/launch/{id}/stop";
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchFinishedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            var uri = $"{ProjectName}/launch/{id}";
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request)
        {
            var uri = $"{ProjectName}/launch/merge";
            var body = ModelSerializer.Serialize<MergeLaunchesRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request)
        {
            var uri = $"{ProjectName}/launch/{id}/update";
            var body = ModelSerializer.Serialize<UpdateLaunchRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request)
        {
            var uri = $"{ProjectName}/launch/analyze";
            var body = ModelSerializer.Serialize<AnalyzeLaunchRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
