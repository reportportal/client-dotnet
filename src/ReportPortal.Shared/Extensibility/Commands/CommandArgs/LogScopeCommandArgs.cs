using ReportPortal.Shared.Execution.Logging;
using System;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    public class LogScopeCommandArgs : EventArgs
    {
        public LogScopeCommandArgs(ILogScope logScope)
        {
            LogScope = logScope;
        }

        public ILogScope LogScope { get; }
    }
}
