using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class Content<T>
    {
        [JsonPropertyName("content")]
        public IEnumerable<T> Items { get; set; }

        public Page Page { get; set; }
    }

    public class Page
    {
        public int Size { get; set; }

        public int TotalElements { get; set; }

        public int TotalPages { get; set; }

        public int Number { get; set; }
    }
}
