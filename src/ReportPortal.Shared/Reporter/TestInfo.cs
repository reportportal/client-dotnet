using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Shared.Reporter
{
    public class TestInfo : ITestReporterInfo
    {
        public string Uuid { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public Status Status { get; set; }
    }
}
