using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;

namespace ReportPortal.Shared.Extensibility.Commands
{
    public interface ICommandsSource
    {
        event LogCommandHandler<LogScopeCommandArgs> OnBeginLogScopeCommand;

        event LogCommandHandler<LogScopeCommandArgs> OnEndLogScopeCommand;

        event LogCommandHandler<LogMessageCommandArgs> OnLogMessageCommand;

        ITestCommandsSource TestCommandsSource { get; }
    }

    public delegate void LogCommandHandler<TCommandArgs>(ILogContext logContext, TCommandArgs args);
}
