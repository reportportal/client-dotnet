using ReportPortal.Shared.Execution.Logging;

namespace ReportPortal.Shared.Execution
{
    public interface ITestContext
    {
        ILogScope Log { get; set; }
    }
}
