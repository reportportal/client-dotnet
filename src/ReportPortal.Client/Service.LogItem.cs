using System.Collections.Generic;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;

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
            var request = new RestRequest("log");
            if (filterOption != null)
            {
                foreach (var p in filterOption.ConvertToDictionary())
                {
                    request.AddParameter(p.Key, p.Value);
                }
            }
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<List<LogItem>>(JObject.Parse(response.Content)["content"].ToString());
        }

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        public LogItem GetLogItem(string id)
        {
            var request = new RestRequest("log/" + id);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<LogItem>(response.Content);
        }

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <returns>Array of bytes.</returns>
        public byte[] GetBinaryData(string id)
        {
            var request = new RestRequest("data/" + id);
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
            var request = new RestRequest("log/", Method.POST);
           
            if (model.Attach == null)
            {
                var body = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }
            else
            {
                var body = JsonConvert.SerializeObject(new List<AddLogItemRequest> {model}, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                request.AddFile("json_request_part", Encoding.Unicode.GetBytes(body), "body", "application/json");
                request.AddFile(model.Attach.Name, model.Attach.Data, model.Attach.Name, model.Attach.MimeType);
            }

            var response = _restClient.ExecuteWithErrorHandling(request);
            var result = JsonConvert.DeserializeObject<LogItem>(model.Attach == null ? response.Content : JObject.Parse(response.Content)["responses"].First.ToString());
            return result;
        }

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <returns>A message from service.</returns>
        public Message DeleteLogItem(string id)
        {
            var request = new RestRequest("log/" + id, Method.DELETE);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<Message>(response.Content);
        }
    }
}
