using System;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish execution of specified launch.
    /// </summary>
    public class FinishLaunchRequest
    {
        /// <summary>
        /// Gets or sets the date and time when the launch execution is finished.
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
    }
}
