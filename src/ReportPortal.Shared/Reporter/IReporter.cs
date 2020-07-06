using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public interface IReporter
    {
        IReporterInfo Info { get; }

        Task StartTask { get; }

        Task FinishTask { get; }
    }
}
