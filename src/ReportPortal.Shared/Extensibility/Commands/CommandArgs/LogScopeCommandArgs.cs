using ReportPortal.Shared.Execution.Logging;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    public class LogScopeCommandArgs
    {
        public LogScopeCommandArgs(ILogScope logScope)
        {
            LogScope = logScope;
        }

        public ILogScope LogScope { get; }
    }
}
