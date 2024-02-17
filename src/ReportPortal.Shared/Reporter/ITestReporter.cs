using ReportPortal.Client.Abstractions.Requests;
using System.Collections.Generic;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents a test reporter that is responsible for reporting test results.
    /// </summary>
    public interface ITestReporter : IReporter
    {
        /// <summary>
        /// Gets the information about the test reporter.
        /// </summary>
        ITestReporterInfo Info { get; }

        /// <summary>
        /// Gets the parent test reporter.
        /// </summary>
        ITestReporter ParentTestReporter { get; }

        /// <summary>
        /// Gets the launch reporter.
        /// </summary>
        ILaunchReporter LaunchReporter { get; }

        /// <summary>
        /// Starts the test item.
        /// </summary>
        /// <param name="startTestItemRequest">The request to start the test item.</param>
        void Start(StartTestItemRequest startTestItemRequest);

        /// <summary>
        /// Finishes the test item.
        /// </summary>
        /// <param name="finishTestItemRequest">The request to finish the test item.</param>
        void Finish(FinishTestItemRequest finishTestItemRequest);

        /// <summary>
        /// Starts a child test reporter.
        /// </summary>
        /// <param name="startTestItemRequest">The request to start the child test item.</param>
        /// <returns>The child test reporter.</returns>
        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        /// <summary>
        /// Gets the list of child test reporters.
        /// </summary>
        IList<ITestReporter> ChildTestReporters { get; }

        /// <summary>
        /// Logs a log item.
        /// </summary>
        /// <param name="createLogItemRequest">The request to create the log item.</param>
        void Log(CreateLogItemRequest createLogItemRequest);
    }
}
