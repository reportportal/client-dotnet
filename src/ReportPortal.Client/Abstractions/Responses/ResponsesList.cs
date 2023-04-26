using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class ResponsesList<T>
    {
        [JsonPropertyName("responses")]
        public IList<T> Items { get; set; }
    }
}
