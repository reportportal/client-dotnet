using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;

namespace ReportPortal.Shared.Extensibility.ReportEvents
{
    /// <summary>
    /// Represents the interface for a source of report events.
    /// </summary>
    public interface IReportEventsSource
    {
        /// <summary>
        /// Occurs when the launch is initializing.
        /// </summary>
        event LaunchEventHandler<LaunchInitializingEventArgs> OnLaunchInitializing;

        /// <summary>
        /// Occurs before the launch is starting.
        /// </summary>
        event LaunchEventHandler<BeforeLaunchStartingEventArgs> OnBeforeLaunchStarting;

        /// <summary>
        /// Occurs after the launch has started.
        /// </summary>
        event LaunchEventHandler<AfterLaunchStartedEventArgs> OnAfterLaunchStarted;

        /// <summary>
        /// Occurs before the launch is finishing.
        /// </summary>
        event LaunchEventHandler<BeforeLaunchFinishingEventArgs> OnBeforeLaunchFinishing;

        /// <summary>
        /// Occurs after the launch has finished.
        /// </summary>
        event LaunchEventHandler<AfterLaunchFinishedEventArgs> OnAfterLaunchFinished;

        /// <summary>
        /// Occurs before a test is starting.
        /// </summary>
        event TestEventHandler<BeforeTestStartingEventArgs> OnBeforeTestStarting;

        /// <summary>
        /// Occurs after a test has started.
        /// </summary>
        event TestEventHandler<AfterTestStartedEventArgs> OnAfterTestStarted;

        /// <summary>
        /// Occurs before a test is finishing.
        /// </summary>
        event TestEventHandler<BeforeTestFinishingEventArgs> OnBeforeTestFinishing;

        /// <summary>
        /// Occurs after a test has finished.
        /// </summary>
        event TestEventHandler<AfterTestFinishedEventArgs> OnAfterTestFinished;

        /// <summary>
        /// Occurs before logs are sending.
        /// </summary>
        event LogsEventHandler<BeforeLogsSendingEventArgs> OnBeforeLogsSending;

        /// <summary>
        /// Occurs after logs are sent.
        /// </summary>
        event LogsEventHandler<AfterLogsSentEventArgs> OnAfterLogsSent;
    }

    /// <summary>
    /// Represents the delegate for handling launch events.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    /// <param name="launchReporter">The launch reporter.</param>
    /// <param name="args">The event arguments.</param>
    public delegate void LaunchEventHandler<TEventArgs>(ILaunchReporter launchReporter, TEventArgs args);

    /// <summary>
    /// Represents the delegate for handling test events.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    /// <param name="testReporter">The test reporter.</param>
    /// <param name="args">The event arguments.</param>
    public delegate void TestEventHandler<TEventArgs>(ITestReporter testReporter, TEventArgs args);

    /// <summary>
    /// Represents the delegate for handling logs events.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    /// <param name="logsReporter">The logs reporter.</param>
    /// <param name="args">The event arguments.</param>
    public delegate void LogsEventHandler<TEventArgs>(ILogsReporter logsReporter, TEventArgs args);
}
