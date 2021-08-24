using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;

namespace ReportPortal.Shared.Extensibility.ReportEvents
{
    public interface IReportEventsSource
    {
        event LaunchEventHandler<LaunchInitializingEventArgs> OnLaunchInitializing;

        event LaunchEventHandler<BeforeLaunchStartingEventArgs> OnBeforeLaunchStarting;

        event LaunchEventHandler<AfterLaunchStartedEventArgs> OnAfterLaunchStarted;

        event LaunchEventHandler<BeforeLaunchFinishingEventArgs> OnBeforeLaunchFinishing;

        event LaunchEventHandler<AfterLaunchFinishedEventArgs> OnAfterLaunchFinished;


        event TestEventHandler<BeforeTestStartingEventArgs> OnBeforeTestStarting;

        event TestEventHandler<AfterTestStartedEventArgs> OnAfterTestStarted;

        event TestEventHandler<BeforeTestFinishingEventArgs> OnBeforeTestFinishing;

        event TestEventHandler<AfterTestFinishedEventArgs> OnAfterTestFinished;


        event LogsEventHandler<BeforeLogsSendingEventArgs> OnBeforeLogsSending;
    }

    public delegate void LaunchEventHandler<TEventArgs>(ILaunchReporter launchReporter, TEventArgs args);

    public delegate void TestEventHandler<TEventAgrs>(ITestReporter testReporter, TEventAgrs args);

    public delegate void LogsEventHandler<TEventAgrs>(ILogsReporter logsReporter, TEventAgrs args);
}
