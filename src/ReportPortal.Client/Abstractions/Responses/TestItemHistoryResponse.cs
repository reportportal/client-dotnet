using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class TestItemHistoryContainer
    {
        public string GroupingField { get; set; }

        public IList<TestItemHistoryElement> Resources { get; set; }
    }

    public class TestItemHistoryElement
    {
        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status Status { get; set; }

        public long LaunchNumber { get; set; }

        public long LaunchId { get; set; }

        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime StartTime { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status LaunchStatus { get; set; }

        public IList<TestItemResponse> Resources { get; set; }
    }
}
