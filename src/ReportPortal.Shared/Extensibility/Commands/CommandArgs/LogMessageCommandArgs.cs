using ReportPortal.Shared.Execution.Logging;
using System;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    /// <summary>
    /// Represents the arguments for a log message command.
    /// </summary>
    public class LogMessageCommandArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessageCommandArgs"/> class.
        /// </summary>
        /// <param name="logScope">The log scope.</param>
        /// <param name="logMessage">The log message.</param>
        public LogMessageCommandArgs(ILogScope logScope, ILogMessage logMessage)
        {
            LogScope = logScope;
            LogMessage = logMessage;
        }

        /// <summary>
        /// Gets the log scope.
        /// </summary>
        public ILogScope LogScope { get; }

        /// <summary>
        /// Gets the log message.
        /// </summary>
        public ILogMessage LogMessage { get; }
    }
}
