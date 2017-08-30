using System.Collections.Generic;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using RestSharp;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;

namespace ReportPortal.Client
{
    public partial class Service
    {
        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving log items.</param>
        /// <returns>A list of log items.</returns>
        public IEnumerable<LogItem> GetLogItems(FilterOption filterOption = null)
        {
            var request = new RestRequest(Project + "/log");
            if (filterOption != null)
            {
                foreach (var p in filterOption.ConvertToDictionary())
                {
                    request.AddParameter(p.Key, p.Value);
                }
            }
            var response = _restClient.ExecuteWithErrorHandling(request);
            return ModelSerializer.Deserialize<List<LogItem>>(response.Content);
        }

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        public LogItem GetLogItem(string id)
        {
            var request = new RestRequest(Project + "/log/" + id);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return ModelSerializer.Deserialize<LogItem>(response.Content);
        }

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <returns>Array of bytes.</returns>
        public byte[] GetBinaryData(string id)
        {
            var request = new RestRequest(Project + "/data/" + id);
            var response = _restClient.Execute(request);
            return response.RawBytes;
        }

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="model">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        public LogItem AddLogItem(AddLogItemRequest model)
        {
            var request = new RestRequest(Project + "/log/", Method.POST);
           
            if (model.Attach == null)
            {
                var body = ModelSerializer.Serialize<AddLogItemRequest>(model);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }
            else
            {
                var body = ModelSerializer.Serialize<List<AddLogItemRequest>>(new List<AddLogItemRequest> { model });
                request.AddFile("json_request_part", Encoding.Unicode.GetBytes(body), "body", "application/json");
                request.AddFile(model.Attach.Name, model.Attach.Data, model.Attach.Name, model.Attach.MimeType);
            }

            var response = _restClient.ExecuteWithErrorHandling(request);
            var result = ModelSerializer.Deserialize<LogItem>(response.Content);
            return result;
        }

        public async Task<LogItem> AddLogItemAsync(AddLogItemRequest model)
        {
            return await Task.Run(() => AddLogItem(model));
        }

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <returns>A message from service.</returns>
        public Message DeleteLogItem(string id)
        {
            var request = new RestRequest(Project + "/log/" + id, Method.DELETE);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return ModelSerializer.Deserialize<Message>(response.Content);
        }
    }
}
