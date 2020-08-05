using ReportPortal.Shared.Execution.Logging;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    public class LogMessageCommandArgs
    {
        public LogMessageCommandArgs(ILogScope logScope, ILogMessage logMessage)
        {
            LogScope = logScope;
            LogMessage = logMessage;
        }

        public ILogScope LogScope { get; }

        public ILogMessage LogMessage { get; }
    }
}
