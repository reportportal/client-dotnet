using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;
using System.Collections.Generic;

namespace ReportPortal.Shared.Execution
{
    public class CommandsSource : ICommandsSource
    {
        private IList<ICommandsListener> _listeners;

        public CommandsSource(IList<ICommandsListener> listeners)
        {
            _listeners = listeners;

            if (_listeners != null)
            {
                foreach (var listener in _listeners)
                {
                    listener.Initialize(this);
                }
            }
        }

        public event LogCommandHandler<ILogScope> OnBeginLogScopeCommand;

        public event LogCommandHandler<ILogScope> OnEndLogScopeCommand;

        public event LogCommandHandler<LogMessageCommandArgs> OnLogMessageCommand;

        public static void RaiseOnBeginScopeCommand(CommandsSource commandsSource, ILogContext logContext, ILogScope logScope)
        {
            commandsSource.OnBeginLogScopeCommand?.Invoke(logContext, logScope);
        }

        public static void RaiseOnEndScopeCommand(CommandsSource commandsSource, ILogContext logContext, ILogScope logScope)
        {
            commandsSource.OnEndLogScopeCommand?.Invoke(logContext, logScope);
        }

        public static void RaiseOnLogMessageCommand(CommandsSource commandsSource, ILogContext logContext, LogMessageCommandArgs args)
        {
            commandsSource.OnLogMessageCommand?.Invoke(logContext, args);
        }
    }
}
