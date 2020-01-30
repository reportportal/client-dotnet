using System;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Converters;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Responses;
using System.Collections.Generic;

namespace ReportPortal.Client
{
    public class ServiceLogItemResource : BaseResource, ILogItemResource
    {
        public ServiceLogItemResource(HttpClient httpClient, Uri baseUri, string project, string token) : base(httpClient, baseUri, project, token)
        {

        }

        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving log items.</param>
        /// <returns>A list of log items.</returns>
        public virtual async Task<LogItemsContainer> GetAsync(FilterOption filterOption = null)
        {
            var uri = BaseUri.Append($"{Project}/log");

            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LogItemsContainer>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified log item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        public virtual async Task<LogItem> GetAsync(string uuid)
        {
            var uri = BaseUri.Append($"{Project}/log/uuid/{uuid}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LogItem>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        public virtual async Task<LogItem> GetAsync(long id)
        {
            var uri = BaseUri.Append($"{Project}/log/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LogItem>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <returns>Array of bytes.</returns>
        public virtual async Task<byte[]> GetBinaryDataAsync(string id)
        {
            var uri = BaseUri.Append($"data/{Project}/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="model">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        public virtual async Task<LogItemCreatedResponse> AddAsync(AddLogItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/log");

            if (model.Attach == null)
            {
                var body = ModelSerializer.Serialize<AddLogItemRequest>(model);
                var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                response.VerifySuccessStatusCode();
                return ModelSerializer.Deserialize<LogItemCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            else
            {
                var body = ModelSerializer.Serialize<List<AddLogItemRequest>>(new List<AddLogItemRequest> { model });
                var multipartContent = new MultipartFormDataContent();

                var jsonContent = new StringContent(body, Encoding.UTF8, "application/json");
                multipartContent.Add(jsonContent, "json_request_part");

                var byteArrayContent = new ByteArrayContent(model.Attach.Data, 0, model.Attach.Data.Length);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Attach.MimeType);
                multipartContent.Add(byteArrayContent, "file", model.Attach.Name);

                var response = await HttpClient.PostAsync(uri, multipartContent).ConfigureAwait(false);
                response.VerifySuccessStatusCode();
                var c = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return ModelSerializer.Deserialize<Responses>(c).LogItems[0];
            }
        }

        [System.Runtime.Serialization.DataContract]
        public class Responses
        {
            [System.Runtime.Serialization.DataMember(Name = "responses")]
            public List<LogItemCreatedResponse> LogItems { get; set; }
        }

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<Message> DeleteAsync(long id)
        {
            var uri = BaseUri.Append($"{Project}/log/{id}");
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
