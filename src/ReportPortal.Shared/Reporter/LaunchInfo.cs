using System;

namespace ReportPortal.Shared.Reporter
{
    public class LaunchInfo : ILaunchReporterInfo
    {
        public string Uuid { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public string Url { get; set; }
    }
}
