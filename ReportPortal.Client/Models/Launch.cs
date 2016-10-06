using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using Newtonsoft.Json;

namespace ReportPortal.Client.Models
{
    public class Launch
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? Number { get; set; }

        public LaunchMode Mode { get; set; }

        [JsonProperty("start_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartTime { get; set; }

        [JsonProperty("end_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? EndTime { get; set; }

        public List<string> Tags { get; set; }

        public Statistic Statistics { get; set; }
    }

    public class Statistic
    {
        public Executions Executions { get; set; }
        public Defects Defects { get; set; }
    }

    public class Executions
    {
        public int Total { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
    }

    public class Defects
    {
        [JsonProperty("product_bugs")]
        public Defect ProductBugs { get; set; }

        [JsonProperty("automation_bugs")]
        public Defect AutomationBugs { get; set; }

        [JsonProperty("system_issue")]
        public Defect SystemIssues { get; set; }

        [JsonProperty("to_investigate")]
        public Defect ToInvestigate { get; set; }
    }

    public class Defect
    {
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
