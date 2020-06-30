using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility.Commands;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;

namespace ReportPortal.Shared.Execution
{
    public class CommandsSource : ICommandsSource
    {
        public event LogCommandHandler<ILogScope> OnBeginLogScopeCommand;

        public event LogCommandHandler<ILogScope> OnEndLogScopeCommand;

        public event LogCommandHandler<LogMessageCommandArgs> OnLogMessageCommand;

        public static void RaiseOnBeginScopeCommand(CommandsSource commandsSource, ITestContext testContext, ILogScope logScope)
        {
            commandsSource.OnBeginLogScopeCommand?.Invoke(testContext, logScope);
        }

        public static void RaiseOnEndScopeCommand(CommandsSource commandsSource, ITestContext testContext, ILogScope logScope)
        {
            commandsSource.OnEndLogScopeCommand?.Invoke(testContext, logScope);
        }

        public static void RaiseOnLogMessageCommand(CommandsSource commandsSource, ITestContext testContext, LogMessageCommandArgs args)
        {
            commandsSource.OnLogMessageCommand?.Invoke(testContext, args);
        }
    }
}
