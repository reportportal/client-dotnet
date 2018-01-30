using System;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Converters;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;

namespace ReportPortal.Client
{
    public partial class Service
    {
        /// <summary>
        /// Returns a list of launches for current project.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving launches.</param>
        /// <param name="debug">Returns user debug launches or not.</param>
        /// <returns>A list of launches.</returns>
        public async Task<LaunchesContainer> GetLaunchesAsync(FilterOption filterOption = null, bool debug = false)
        {
            var uri = BaseUri.Append($"{Project}/launch");
            if (debug) { uri = uri.Append("mode"); }

            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }

            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchesContainer>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified launch by ID.
        /// </summary>
        /// <param name="id">ID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        public async Task<Launch> GetLaunchAsync(string id)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Launch>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Creates a new launch.
        /// </summary>
        /// <param name="model">Information about representation of launch.</param>
        /// <returns>Representation of just created launch.</returns>
        public async Task<Launch> StartLaunchAsync(StartLaunchRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch");
            var body = ModelSerializer.Serialize<StartLaunchRequest>(model);
            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Launch>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Finishes specified launch.
        /// </summary>
        /// <param name="id">ID of specified launch.</param>
        /// <param name="model">Information about representation of launch to finish.</param>
        /// <param name="force">Force finish launch even if test items are in progress.</param>
        /// <returns>A message from service.</returns>
        public async Task<Message> FinishLaunchAsync(string id, FinishLaunchRequest model, bool force = false)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            uri = force == true ? uri.Append("/stop") : uri.Append("/finish");
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(model);
            var response = await _httpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes specified launch.
        /// </summary>
        /// <param name="id">ID of the launch to delete.</param>
        /// <returns>A message from service.</returns>
        public async Task<Message> DeleteLaunchAsync(string id)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            var response = await _httpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <param name="model">Request for merging.</param>
        /// <returns>Returns the model of merged launches.</returns>
        public async Task<Launch> MergeLaunchesAsync(MergeLaunchesRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch/merge");
            var body = ModelSerializer.Serialize<MergeLaunchesRequest>(model);
            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Launch>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Update specified launch.
        /// </summary>
        /// <param name="id">ID of launch to update.</param>
        /// <param name="model">Information about launch.</param>
        /// <returns>A message from service.</returns>
        public async Task<Message> UpdateLaunchAsync(string id, UpdateLaunchRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}/update");
            var body = ModelSerializer.Serialize<UpdateLaunchRequest>(model);
            var response = await _httpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <param name="id">Request for merging.</param>
        /// <param name="strategy">Known strategy is 'history'.</param>
        /// <returns>A message from service.</returns>
        public async Task<Message> AnalyzeLaunchAsync(string id, string strategy)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}/analyze/{strategy}");
            var response = await _httpClient.PostAsync(uri, new StringContent(string.Empty, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
