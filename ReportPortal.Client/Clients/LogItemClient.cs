using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using System.Linq;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.Clients
{
    public class LogItemClient: BaseClient
    {
        public LogItemClient(HttpClient httpCLient, Uri baseUri, string project): base(httpCLient, baseUri, project)
        {
        }

        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving log items.</param>
        /// <returns>A list of log items.</returns>
        public virtual async Task<LogItemsContainer> GetLogItemsAsync(FilterOption filterOption = null)
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
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        public virtual async Task<LogItem> GetLogItemAsync(string id)
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
            var uri = BaseUri.Append($"{Project}/data/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="model">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        public virtual async Task<LogItem> AddLogItemAsync(AddLogItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/log");

            if (model.Attach == null)
            {
                var body = ModelSerializer.Serialize<AddLogItemRequest>(model);
                var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                response.VerifySuccessStatusCode();
                return ModelSerializer.Deserialize<LogItem>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            else
            {
                var body = ModelSerializer.Serialize<List<AddLogItemRequest>>(new List<AddLogItemRequest> { model });
                var multipartContent = new MultipartFormDataContent();

                var jsonContent = new StringContent(body, Encoding.UTF8, "application/json");
                multipartContent.Add(jsonContent, "json_request_part");

                var byteArrayContent = new ByteArrayContent(model.Attach.Data.ToArray(), 0, model.Attach.Data.Count);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Attach.MimeType);
                multipartContent.Add(byteArrayContent, "file", model.Attach.Name);

                var response = await HttpClient.PostAsync(uri, multipartContent).ConfigureAwait(false);
                response.VerifySuccessStatusCode();
                var c = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return ModelSerializer.Deserialize<Responses>(c).LogItems[0];
            }
        }

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<Message> DeleteLogItemAsync(string id)
        {
            var uri = BaseUri.Append($"{Project}/log/{id}");
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
