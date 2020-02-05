using System;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Resources;

namespace ReportPortal.Client
{
    public class ServiceLaunchResource : BaseResource, ILaunchResource
    {
        public ServiceLaunchResource(HttpClient httpClient, Uri baseUri, string project) : base(httpClient, baseUri, project)
        {

        }

        /// <summary>
        /// Returns a list of launches for current project.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving launches.</param>
        /// <returns>A list of launches.</returns>
        public virtual async Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption = null)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch");

            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Content<LaunchResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns a list of debug launches for current project.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving launches.</param>
        /// <returns>A list of launches.</returns>
        public virtual async Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption = null)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/mode");

            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Content<LaunchResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified launch by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        public virtual async Task<LaunchResponse> GetAsync(string uuid)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/uuid/{uuid}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified launch by ID.
        /// </summary>
        /// <param name="id">ID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        public virtual async Task<LaunchResponse> GetAsync(long id)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Creates a new launch.
        /// </summary>
        /// <param name="request">Information about representation of launch.</param>
        /// <returns>Representation of just created launch.</returns>
        public virtual async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch");
            var body = ModelSerializer.Serialize<StartLaunchRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Finishes specified launch.
        /// </summary>
        /// <param name="uuid">UUID of specified launch.</param>
        /// <param name="request">Information about representation of launch to finish.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/{uuid}/finish");
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchFinishedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Stopes specified launch even if inner tests are not finished yet.
        /// </summary>
        /// <param name="id">ID of specified launch.</param>
        /// <param name="request">Information about representation of launch to finish.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/{id}/stop");
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchFinishedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes specified launch.
        /// </summary>
        /// <param name="id">ID of the launch to delete.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<MessageResponse> DeleteAsync(long id)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/{id}");
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <param name="request">Request for merging.</param>
        /// <returns>Returns the model of merged launches.</returns>
        public virtual async Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/merge");
            var body = ModelSerializer.Serialize<MergeLaunchesRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Update specified launch.
        /// </summary>
        /// <param name="id">ID of launch to update.</param>
        /// <param name="request">Information about launch.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/{id}/update");
            var body = ModelSerializer.Serialize<UpdateLaunchRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <returns>A message from service.</returns>
        public virtual async Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request)
        {
            var uri = BaseUri.Append($"{ProjectName}/launch/analyze");
            var body = ModelSerializer.Serialize<AnalyzeLaunchRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
