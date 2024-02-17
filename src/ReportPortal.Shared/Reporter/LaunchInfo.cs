using System;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents the information about a launch reporter.
    /// </summary>
    public class LaunchInfo : ILaunchReporterInfo
    {
        /// <summary>
        /// Gets or sets the UUID of the launch.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the name of the launch.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start time of the launch.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the finish time of the launch.
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// Gets or sets the URL of the launch.
        /// </summary>
        public string Url { get; set; }
    }
}
