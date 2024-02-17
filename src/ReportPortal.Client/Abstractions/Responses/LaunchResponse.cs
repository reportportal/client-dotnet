using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents the response of a launch.
    /// </summary>
    public class LaunchResponse
    {
        /// <summary>
        /// Gets or sets the ID of the launch.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the UUID of the launch.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the name of the launch.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the number of the launch.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the mode of the launch.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<LaunchMode>))]
        public LaunchMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the start time of the launch.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the launch.
        /// </summary>
        [JsonConverter(typeof(NullableDateTimeUnixEpochConverter))]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the launch has retries.
        /// </summary>
        public bool HasRetries { get; set; }

        /// <summary>
        /// Gets or sets the list of attributes of the launch.
        /// </summary>
        public IList<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the statistics of the launch.
        /// </summary>
        public Statistic Statistics { get; set; }
    }

    /// <summary>
    /// Represents the statistics of a launch.
    /// </summary>
    public class Statistic
    {
        /// <summary>
        /// Gets or sets the executions statistics of the launch.
        /// </summary>
        public Executions Executions { get; set; }

        /// <summary>
        /// Gets or sets the defects statistics of the launch.
        /// </summary>
        public Defects Defects { get; set; }
    }

    /// <summary>
    /// Represents the executions statistics of a launch.
    /// </summary>
    public class Executions
    {
        /// <summary>
        /// Gets or sets the total number of executions.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of passed executions.
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// Gets or sets the number of failed executions.
        /// </summary>
        public int Failed { get; set; }

        /// <summary>
        /// Gets or sets the number of skipped executions.
        /// </summary>
        public int Skipped { get; set; }
    }

    /// <summary>
    /// Represents the defects statistics of a launch.
    /// </summary>
    public class Defects
    {
        /// <summary>
        /// Gets or sets the number of product bugs.
        /// </summary>
        [JsonPropertyName("product_bug")]
        public Defect ProductBugs { get; set; }

        /// <summary>
        /// Gets or sets the number of automation bugs.
        /// </summary>
        [JsonPropertyName("automation_bug")]
        public Defect AutomationBugs { get; set; }

        /// <summary>
        /// Gets or sets the number of system issues.
        /// </summary>
        [JsonPropertyName("system_issue")]
        public Defect SystemIssues { get; set; }

        /// <summary>
        /// Gets or sets the number of defects to investigate.
        /// </summary>
        [JsonPropertyName("to_investigate")]
        public Defect ToInvestigate { get; set; }

        /// <summary>
        /// Gets or sets the number of defects with no defect.
        /// </summary>
        [JsonPropertyName("no_defect")]
        public Defect NoDefect { get; set; }
    }

    /// <summary>
    /// Represents a defect.
    /// </summary>
    public class Defect
    {
        /// <summary>
        /// Gets or sets the total number of defects.
        /// </summary>
        public int Total { get; set; }
    }
}
