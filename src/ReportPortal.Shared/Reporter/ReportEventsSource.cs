using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;

namespace ReportPortal.Shared.Reporter
{
    public class ReportEventsSource : IReportEventsSource
    {
        public event LaunchEventHandler<LaunchInitializingEventArgs> OnLaunchInitializing;

        public event LaunchEventHandler<BeforeLaunchStartingEventArgs> OnBeforeLaunchStarting;
        public event LaunchEventHandler<AfterLaunchStartedEventArgs> OnAfterLaunchStarted;
        public event LaunchEventHandler<BeforeLaunchFinishingEventArgs> OnBeforeLaunchFinishing;
        public event LaunchEventHandler<AfterLaunchFinishedEventArgs> OnAfterLaunchFinished;

        public event TestEventHandler<BeforeTestStartingEventArgs> OnBeforeTestStarting;
        public event TestEventHandler<AfterTestStartedEventArgs> OnAfterTestStarted;
        public event TestEventHandler<BeforeTestFinishingEventArgs> OnBeforeTestFinishing;
        public event TestEventHandler<AfterTestFinishedEventArgs> OnAfterTestFinished;

        public static void RaiseLaunchInitializing(ReportEventsSource source, ILaunchReporter launchReporter, LaunchInitializingEventArgs args)
        {
            source.OnLaunchInitializing?.Invoke(launchReporter, args);
        }

        public static void RaiseBeforeLaunchStarting(ReportEventsSource source, ILaunchReporter launchReporter, BeforeLaunchStartingEventArgs args)
        {
            source.OnBeforeLaunchStarting?.Invoke(launchReporter, args);
        }

        public static void RaiseAfterLaunchStarted(ReportEventsSource source, ILaunchReporter launchReporter, AfterLaunchStartedEventArgs args)
        {
            source.OnAfterLaunchStarted?.Invoke(launchReporter, args);
        }

        public static void RaiseBeforeLaunchFinishing(ReportEventsSource source, ILaunchReporter launchReporter, BeforeLaunchFinishingEventArgs args)
        {
            source.OnBeforeLaunchFinishing?.Invoke(launchReporter, args);
        }

        public static void RaiseAfterLaunchFinished(ReportEventsSource source, ILaunchReporter launchReporter, AfterLaunchFinishedEventArgs args)
        {
            source.OnAfterLaunchFinished?.Invoke(launchReporter, args);
        }

        public static void RaiseBeforeTestStarting(ReportEventsSource source, ITestReporter testReporter, BeforeTestStartingEventArgs args)
        {
            source.OnBeforeTestStarting?.Invoke(testReporter, args);
        }

        public static void RaiseAfterTestStarted(ReportEventsSource source, ITestReporter testReporter, AfterTestStartedEventArgs args)
        {
            source.OnAfterTestStarted?.Invoke(testReporter, args);
        }

        public static void RaiseBeforeTestFinishing(ReportEventsSource source, ITestReporter testReporter, BeforeTestFinishingEventArgs args)
        {
            source.OnBeforeTestFinishing?.Invoke(testReporter, args);
        }

        public static void RaiseAfterTestFinished(ReportEventsSource source, ITestReporter testReporter, AfterTestFinishedEventArgs args)
        {
            source.OnAfterTestFinished?.Invoke(testReporter, args);
        }
    }
}
