using ReportPortal.Shared.Reporter;
using System;

namespace ReportPortal.Shared
{
    [Obsolete("We will avoid global static context.")]
    public class ContextInfo
    {
        /// <summary>
        /// Current reporter to send results.
        /// </summary>
        public ILaunchReporter LaunchReporter { get; set; }
    }
}