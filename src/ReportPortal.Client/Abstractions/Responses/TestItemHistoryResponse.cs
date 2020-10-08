using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class TestItemHistoryContainer
    {
        [DataMember(Name = "groupingField")]
        public string GroupingField { get; set; }

        [DataMember(Name = "resources")]
        public IEnumerable<TestItemHistoryElement> Resources { get; set; }
    }

    [DataContract]
    public class TestItemHistoryElement
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "status")]
        private string StatusString { get; set; }

        public Status Status => EnumConverter.ConvertTo<Status>(StatusString);

        [DataMember(Name = "launchNumber")]
        public long LaunchNumber { get; set; }

        [DataMember(Name = "launchId")]
        public string LaunchId { get; set; }

        [DataMember(Name = "startTime")]
        private string StartTimeString { get; set; }

        public DateTime StartTime => DateTimeConverter.ConvertTo(StartTimeString);

        [DataMember(Name = "launchStatus")]
        private string LaunchStatusString { get; set; }

        public Status LaunchStatus => EnumConverter.ConvertTo<Status>(LaunchStatusString);

        [DataMember(Name = "resources")]
        public IEnumerable<TestItemResponse> Resources { get; set; }
    }
}
