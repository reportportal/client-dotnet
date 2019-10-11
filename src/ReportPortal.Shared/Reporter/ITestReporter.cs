using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public interface ITestReporter
    {
        TestItem TestInfo { get; }

        ITestReporter ParentTestReporter { get; }

        ILaunchReporter LaunchReporter { get; }

        Task StartTask { get; }

        void Finish(FinishTestItemRequest finishTestItemRequest);

        Task FinishTask { get; }

        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        ConcurrentBag<ITestReporter> ChildTestReporters { get; }

        void Log(AddLogItemRequest addLogItemRequest);

        void Sync();
    }
}
