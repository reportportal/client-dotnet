using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Shared.Reporter
{
    public class TestInfo
    {
        public string Uuid { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Status Status { get; set; }
    }
}
