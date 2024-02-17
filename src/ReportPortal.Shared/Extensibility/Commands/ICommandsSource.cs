using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;

namespace ReportPortal.Shared.Extensibility.Commands
{
    /// <summary>
    /// Represents a source of commands.
    /// </summary>
    public interface ICommandsSource
    {
        /// <summary>
        /// Event that is raised when a log scope command begins.
        /// </summary>
        event LogCommandHandler<LogScopeCommandArgs> OnBeginLogScopeCommand;

        /// <summary>
        /// Event that is raised when a log scope command ends.
        /// </summary>
        event LogCommandHandler<LogScopeCommandArgs> OnEndLogScopeCommand;

        /// <summary>
        /// Event that is raised when a log message command is executed.
        /// </summary>
        event LogCommandHandler<LogMessageCommandArgs> OnLogMessageCommand;

        /// <summary>
        /// Gets the source of test commands.
        /// </summary>
        ITestCommandsSource TestCommandsSource { get; }
    }

    /// <summary>
    /// Represents a delegate for handling log commands.
    /// </summary>
    /// <typeparam name="TCommandArgs">The type of the command arguments.</typeparam>
    /// <param name="logContext">The log context.</param>
    /// <param name="args">The command arguments.</param>
    public delegate void LogCommandHandler<TCommandArgs>(ILogContext logContext, TCommandArgs args);
}
