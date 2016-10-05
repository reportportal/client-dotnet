using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortal.Client.Models
{
    public class LaunchesContainer
    {
        [JsonProperty("content")]
        public List<Launch> Launches { get; set; } 

        public Page Page { get; set; }
    }

    public class TestItemsContainer
    {
        [JsonProperty("content")]
        public List<TestItem> TestItems { get; set; }

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
