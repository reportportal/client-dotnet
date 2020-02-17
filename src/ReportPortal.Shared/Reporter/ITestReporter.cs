using ReportPortal.Client.Abstractions.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public interface ITestReporter
    {
        TestInfo TestInfo { get; }

        ITestReporter ParentTestReporter { get; }

        ILaunchReporter LaunchReporter { get; }

        void Start(StartTestItemRequest startTestItemRequest);

        Task StartTask { get; }

        void Finish(FinishTestItemRequest finishTestItemRequest);

        Task FinishTask { get; }

        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        IList<ITestReporter> ChildTestReporters { get; }

        void Log(CreateLogItemRequest createLogItemRequest);

        void Sync();
    }
}
