using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    public class TestItemHistory
    {
        [DataMember(Name = "launchNumber")]
        public long LaunchNumber { get; set; }

        [DataMember(Name = "launchId")]
        public string LaunchId { get; set; }

        [DataMember(Name = "startTime")]
        public string StartTimeString { get; set; }

        public DateTime StartTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(StartTimeString);
            }
            set
            {
                StartTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        [DataMember(Name = "launchStatus")]
        public Status LaunchStatus { get; set; }

        [DataMember(Name = "resources")]
        public List<TestItem> Resources { get; set; }
    }
}
