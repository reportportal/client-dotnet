using ReportPortal.Shared.Execution.Logging;
using System;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    public class LogMessageCommandArgs : EventArgs
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
