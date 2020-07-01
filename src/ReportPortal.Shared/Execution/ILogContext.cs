using ReportPortal.Shared.Execution.Logging;

namespace ReportPortal.Shared.Execution
{
    public interface ILogContext
    {
        ILogScope Log { get; set; }
    }
}
