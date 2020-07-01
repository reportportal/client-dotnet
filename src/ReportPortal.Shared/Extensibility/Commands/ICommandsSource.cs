using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;

namespace ReportPortal.Shared.Extensibility.Commands
{
    public interface ICommandsSource
    {
        event LogCommandHandler<ILogScope> OnBeginLogScopeCommand;

        event LogCommandHandler<ILogScope> OnEndLogScopeCommand;

        event LogCommandHandler<LogMessageCommandArgs> OnLogMessageCommand;
    }

    public delegate void LogCommandHandler<TCommandArgs>(ILogContext logContext, TCommandArgs args);
}
