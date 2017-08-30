using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class Launch
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? Number { get; set; }

        public LaunchMode Mode { get; set; }

        [DataMember(Name = "start_time")]
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

        [DataMember(Name = "end_time")]
        public string EndTimeString { get; set; }

        public DateTime EndTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(EndTimeString);
            }
            set
            {
                EndTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

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
        [DataMember(Name = "product_bugs")]
        public Defect ProductBugs { get; set; }

        [DataMember(Name = "automation_bugs")]
        public Defect AutomationBugs { get; set; }

        [DataMember(Name = "system_issue")]
        public Defect SystemIssues { get; set; }

        [DataMember(Name = "to_investigate")]
        public Defect ToInvestigate { get; set; }
    }

    public class Defect
    {
        [DataMember(Name = "total")]
        public int Total { get; set; }
    }
}
