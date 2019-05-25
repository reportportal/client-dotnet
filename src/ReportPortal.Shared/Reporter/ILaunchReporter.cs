using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public interface ILaunchReporter
    {
        Launch LaunchInfo { get; }

        void Start(StartLaunchRequest startLaunchRequest);

        Task StartTask { get; }

        void Finish(FinishLaunchRequest finishLaunchRequest);

        Task FinishTask { get; }

        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        ConcurrentBag<ITestReporter> ChildTestReporters { get; }
    }
}
