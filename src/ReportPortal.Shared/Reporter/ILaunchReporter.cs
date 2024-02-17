using ReportPortal.Client.Abstractions.Requests;
using System.Collections.Generic;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents a reporter for a launch in the ReportPortal system.
    /// </summary>
    public interface ILaunchReporter : IReporter
    {
        /// <summary>
        /// Gets the information about the launch reporter.
        /// </summary>
        ILaunchReporterInfo Info { get; }

        /// <summary>
        /// Starts a new launch with the specified start launch request.
        /// </summary>
        /// <param name="startLaunchRequest">The start launch request.</param>
        void Start(StartLaunchRequest startLaunchRequest);

        /// <summary>
        /// Finishes the current launch with the specified finish launch request.
        /// </summary>
        /// <param name="finishLaunchRequest">The finish launch request.</param>
        void Finish(FinishLaunchRequest finishLaunchRequest);

        /// <summary>
        /// Starts a child test reporter with the specified start test item request.
        /// </summary>
        /// <param name="startTestItemRequest">The start test item request.</param>
        /// <returns>The child test reporter.</returns>
        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        /// <summary>
        /// Gets the list of child test reporters.
        /// </summary>
        IList<ITestReporter> ChildTestReporters { get; }

        /// <summary>
        /// Logs a new log item with the specified create log item request.
        /// </summary>
        /// <param name="createLogItemRequest">The create log item request.</param>
        void Log(CreateLogItemRequest createLogItemRequest);
    }
}
