using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Reporter.Statistics;
using System.Collections.Generic;

namespace ReportPortal.Shared.Reporter
{
    public interface ILaunchReporter : IReporter
    {
        void Start(StartLaunchRequest startLaunchRequest);

        void Finish(FinishLaunchRequest finishLaunchRequest);

        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        IList<ITestReporter> ChildTestReporters { get; }

        void Log(CreateLogItemRequest createLogItemRequest);

        void Sync();
    }
}
