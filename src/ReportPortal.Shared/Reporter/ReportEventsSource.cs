using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using System;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents a source of report events.
    /// </summary>
    public class ReportEventsSource : IReportEventsSource
    {
        private static Internal.Logging.ITraceLogger _traceLogger = Internal.Logging.TraceLogManager.Instance.GetLogger<ReportEventsSource>();

        /// <summary>
        /// Event that is triggered when the launch is initializing.
        /// </summary>
        public event LaunchEventHandler<LaunchInitializingEventArgs> OnLaunchInitializing;

        /// <summary>
        /// Event that is triggered before the launch starts.
        /// </summary>
        public event LaunchEventHandler<BeforeLaunchStartingEventArgs> OnBeforeLaunchStarting;

        /// <summary>
        /// Event that is triggered after the launch has started.
        /// </summary>
        public event LaunchEventHandler<AfterLaunchStartedEventArgs> OnAfterLaunchStarted;

        /// <summary>
        /// Event that is triggered before the launch finishes.
        /// </summary>
        public event LaunchEventHandler<BeforeLaunchFinishingEventArgs> OnBeforeLaunchFinishing;

        /// <summary>
        /// Event that is triggered after the launch has finished.
        /// </summary>
        public event LaunchEventHandler<AfterLaunchFinishedEventArgs> OnAfterLaunchFinished;

        /// <summary>
        /// Event that is triggered before a test starts.
        /// </summary>
        public event TestEventHandler<BeforeTestStartingEventArgs> OnBeforeTestStarting;

        /// <summary>
        /// Event that is triggered after a test has started.
        /// </summary>
        public event TestEventHandler<AfterTestStartedEventArgs> OnAfterTestStarted;

        /// <summary>
        /// Event that is triggered before a test finishes.
        /// </summary>
        public event TestEventHandler<BeforeTestFinishingEventArgs> OnBeforeTestFinishing;

        /// <summary>
        /// Event that is triggered after a test has finished.
        /// </summary>
        public event TestEventHandler<AfterTestFinishedEventArgs> OnAfterTestFinished;

        /// <summary>
        /// Event that is triggered before logs are sent.
        /// </summary>
        public event LogsEventHandler<BeforeLogsSendingEventArgs> OnBeforeLogsSending;

        /// <summary>
        /// Event that is triggered after logs have been sent.
        /// </summary>
        public event LogsEventHandler<AfterLogsSentEventArgs> OnAfterLogsSent;

        /// <summary>
        /// Raises the OnLaunchInitializing event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="launchReporter">The launch reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseLaunchInitializing(ReportEventsSource source, ILaunchReporter launchReporter, LaunchInitializingEventArgs args)
        {
            RaiseSafe(source.OnLaunchInitializing, launchReporter, args);
        }

        /// <summary>
        /// Raises the OnBeforeLaunchStarting event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="launchReporter">The launch reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseBeforeLaunchStarting(ReportEventsSource source, ILaunchReporter launchReporter, BeforeLaunchStartingEventArgs args)
        {
            RaiseSafe(source.OnBeforeLaunchStarting, launchReporter, args);
        }

        /// <summary>
        /// Raises the OnAfterLaunchStarted event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="launchReporter">The launch reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseAfterLaunchStarted(ReportEventsSource source, ILaunchReporter launchReporter, AfterLaunchStartedEventArgs args)
        {
            RaiseSafe(source.OnAfterLaunchStarted, launchReporter, args);
        }

        /// <summary>
        /// Raises the OnBeforeLaunchFinishing event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="launchReporter">The launch reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseBeforeLaunchFinishing(ReportEventsSource source, ILaunchReporter launchReporter, BeforeLaunchFinishingEventArgs args)
        {
            RaiseSafe(source.OnBeforeLaunchFinishing, launchReporter, args);
        }

        /// <summary>
        /// Raises the OnAfterLaunchFinished event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="launchReporter">The launch reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseAfterLaunchFinished(ReportEventsSource source, ILaunchReporter launchReporter, AfterLaunchFinishedEventArgs args)
        {
            RaiseSafe(source.OnAfterLaunchFinished, launchReporter, args);
        }

        /// <summary>
        /// Raises the OnBeforeTestStarting event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="testReporter">The test reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseBeforeTestStarting(ReportEventsSource source, ITestReporter testReporter, BeforeTestStartingEventArgs args)
        {
            RaiseSafe(source.OnBeforeTestStarting, testReporter, args);
        }

        /// <summary>
        /// Raises the OnAfterTestStarted event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="testReporter">The test reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseAfterTestStarted(ReportEventsSource source, ITestReporter testReporter, AfterTestStartedEventArgs args)
        {
            RaiseSafe(source.OnAfterTestStarted, testReporter, args);
        }

        /// <summary>
        /// Raises the OnBeforeTestFinishing event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="testReporter">The test reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseBeforeTestFinishing(ReportEventsSource source, ITestReporter testReporter, BeforeTestFinishingEventArgs args)
        {
            RaiseSafe(source.OnBeforeTestFinishing, testReporter, args);
        }

        /// <summary>
        /// Raises the OnAfterTestFinished event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="testReporter">The test reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseAfterTestFinished(ReportEventsSource source, ITestReporter testReporter, AfterTestFinishedEventArgs args)
        {
            RaiseSafe(source.OnAfterTestFinished, testReporter, args);
        }

        /// <summary>
        /// Raises the OnBeforeLogsSending event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="logsReporter">The logs reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseBeforeLogsSending(ReportEventsSource source, ILogsReporter logsReporter, BeforeLogsSendingEventArgs args)
        {
            RaiseSafe(source.OnBeforeLogsSending, logsReporter, args);
        }

        /// <summary>
        /// Raises the OnAfterLogsSent event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="logsReporter">The logs reporter.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseAfterLogsSent(ReportEventsSource source, ILogsReporter logsReporter, AfterLogsSentEventArgs args)
        {
            RaiseSafe(source.OnAfterLogsSent, logsReporter, args);
        }

        private static void RaiseSafe(Delegate source, object reporter, ReportEventBaseArgs args)
        {
            var handlers = source?.GetInvocationList();

            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    try
                    {
                        handler.DynamicInvoke(reporter, args);
                    }
                    catch (Exception ex)
                    {
                        _traceLogger.Error(new Exception($"Unhandled error occurred in handler '{handler.Method}' for one of the reporting event.", ex).ToString());
                    }
                }
            }
        }
    }
}
