using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public interface ILaunchReporter
    {
        LaunchResponse LaunchInfo { get; }

        void Start(StartLaunchRequest startLaunchRequest);

        Task StartTask { get; }

        void Finish(FinishLaunchRequest finishLaunchRequest);

        Task FinishTask { get; }

        ITestReporter StartChildTestReporter(StartTestItemRequest startTestItemRequest);

        IList<ITestReporter> ChildTestReporters { get; }

        void Sync();
    }
}
