using Newtonsoft.Json;

namespace ReportPortal.Client.Models
{
    public class Message
    {
        [JsonProperty("msg")]
        public string Info { get; set; }
    }
}
