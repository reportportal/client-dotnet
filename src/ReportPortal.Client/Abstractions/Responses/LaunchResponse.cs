using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class LaunchResponse
    {
        public long Id { get; set; }

        public string Uuid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Number { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverterEx<LaunchMode>))]
        public LaunchMode Mode { get; set; }

        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime StartTime { get; set; }

        [JsonConverter(typeof(NullableDateTimeUnixEpochConverter))]
        public DateTime? EndTime { get; set; }

        public bool HasRetries { get; set; }

        public IList<ItemAttribute> Attributes { get; set; }

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
        [JsonPropertyName("product_bug")]
        public Defect ProductBugs { get; set; }

        [JsonPropertyName("automation_bug")]
        public Defect AutomationBugs { get; set; }

        [JsonPropertyName("system_issue")]
        public Defect SystemIssues { get; set; }

        [JsonPropertyName("to_investigate")]
        public Defect ToInvestigate { get; set; }

        [JsonPropertyName("no_defect")]
        public Defect NoDefect { get; set; }
    }

    public class Defect
    {
        public int Total { get; set; }
    }
}
