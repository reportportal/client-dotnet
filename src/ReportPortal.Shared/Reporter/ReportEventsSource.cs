using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using System;

namespace ReportPortal.Shared.Reporter
{
    public class ReportEventsSource : IReportEventsSource
    {
        private static Internal.Logging.ITraceLogger _traceLogger = Internal.Logging.TraceLogManager.Instance.GetLogger<ReportEventsSource>();

        public event LaunchEventHandler<LaunchInitializingEventArgs> OnLaunchInitializing;

        public event LaunchEventHandler<BeforeLaunchStartingEventArgs> OnBeforeLaunchStarting;
        public event LaunchEventHandler<AfterLaunchStartedEventArgs> OnAfterLaunchStarted;
        public event LaunchEventHandler<BeforeLaunchFinishingEventArgs> OnBeforeLaunchFinishing;
        public event LaunchEventHandler<AfterLaunchFinishedEventArgs> OnAfterLaunchFinished;

        public event TestEventHandler<BeforeTestStartingEventArgs> OnBeforeTestStarting;
        public event TestEventHandler<AfterTestStartedEventArgs> OnAfterTestStarted;
        public event TestEventHandler<BeforeTestFinishingEventArgs> OnBeforeTestFinishing;
        public event TestEventHandler<AfterTestFinishedEventArgs> OnAfterTestFinished;

        public event LogsEventHandler<BeforeLogsSendingEventArgs> OnBeforeLogsSending;

        public static void RaiseLaunchInitializing(ReportEventsSource source, ILaunchReporter launchReporter, LaunchInitializingEventArgs args)
        {
            RaiseSafe(source.OnLaunchInitializing, launchReporter, args);
        }

        public static void RaiseBeforeLaunchStarting(ReportEventsSource source, ILaunchReporter launchReporter, BeforeLaunchStartingEventArgs args)
        {
            RaiseSafe(source.OnBeforeLaunchStarting, launchReporter, args);
        }

        public static void RaiseAfterLaunchStarted(ReportEventsSource source, ILaunchReporter launchReporter, AfterLaunchStartedEventArgs args)
        {
            RaiseSafe(source.OnAfterLaunchStarted, launchReporter, args);
        }

        public static void RaiseBeforeLaunchFinishing(ReportEventsSource source, ILaunchReporter launchReporter, BeforeLaunchFinishingEventArgs args)
        {
            RaiseSafe(source.OnBeforeLaunchFinishing, launchReporter, args);
        }

        public static void RaiseAfterLaunchFinished(ReportEventsSource source, ILaunchReporter launchReporter, AfterLaunchFinishedEventArgs args)
        {
            RaiseSafe(source.OnAfterLaunchFinished, launchReporter, args);
        }

        public static void RaiseBeforeTestStarting(ReportEventsSource source, ITestReporter testReporter, BeforeTestStartingEventArgs args)
        {
            RaiseSafe(source.OnBeforeTestStarting, testReporter, args);
        }

        public static void RaiseAfterTestStarted(ReportEventsSource source, ITestReporter testReporter, AfterTestStartedEventArgs args)
        {
            RaiseSafe(source.OnAfterTestStarted, testReporter, args);
        }

        public static void RaiseBeforeTestFinishing(ReportEventsSource source, ITestReporter testReporter, BeforeTestFinishingEventArgs args)
        {
            RaiseSafe(source.OnBeforeTestFinishing, testReporter, args);
        }

        public static void RaiseAfterTestFinished(ReportEventsSource source, ITestReporter testReporter, AfterTestFinishedEventArgs args)
        {
            RaiseSafe(source.OnAfterTestFinished, testReporter, args);
        }

        public static void RaiseBeforeLogsSending(ReportEventsSource source, ILogsReporter logsReporter, BeforeLogsSendingEventArgs args)
        {
            RaiseSafe(source.OnBeforeLogsSending, logsReporter, args);
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
