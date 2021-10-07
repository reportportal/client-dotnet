using ReportPortal.Client.Abstractions.Requests;
using System.Collections.Generic;

namespace ReportPortal.Shared.Reporter
{
    public interface ITestReporter : IReporter
    {
        ITestReporterInfo Info { get; }

        ITestReporter ParentTestReporter { get; }

        ILaunchReporter LaunchReporter { get; }

        void Start(StartTestItemRequest startTestItemRequest);

        void Finish(FinishTestItemRequest finishTestItemRequest);

        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        IList<ITestReporter> ChildTestReporters { get; }

        void Log(CreateLogItemRequest createLogItemRequest);

        void Sync();
    }
}
