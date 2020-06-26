using ReportPortal.Shared.Execution.Log;

namespace ReportPortal.Shared.Execution
{
    public interface ITestContext
    {
        ILogScope Log { get; set; }
    }
}
