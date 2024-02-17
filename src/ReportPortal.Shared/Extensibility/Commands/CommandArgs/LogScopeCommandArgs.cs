using ReportPortal.Shared.Execution.Logging;
using System;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    /// <summary>
    /// Represents the arguments for a log scope command.
    /// </summary>
    public class LogScopeCommandArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogScopeCommandArgs"/> class with the specified log scope.
        /// </summary>
        /// <param name="logScope">The log scope.</param>
        public LogScopeCommandArgs(ILogScope logScope)
        {
            LogScope = logScope;
        }

        /// <summary>
        /// Gets the log scope.
        /// </summary>
        public ILogScope LogScope { get; }
    }
}
