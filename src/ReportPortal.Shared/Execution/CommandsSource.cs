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

        public ITestCommandsSource TestCommandsSource { get; } = new TestCommandsSource();

        public event LogCommandHandler<LogScopeCommandArgs> OnBeginLogScopeCommand;

        public event LogCommandHandler<LogScopeCommandArgs> OnEndLogScopeCommand;

        public event LogCommandHandler<LogMessageCommandArgs> OnLogMessageCommand;

        public static void RaiseOnBeginScopeCommand(CommandsSource commandsSource, ILogContext logContext, LogScopeCommandArgs args)
        {
            commandsSource.OnBeginLogScopeCommand?.Invoke(logContext, args);
        }

        public static void RaiseOnEndScopeCommand(CommandsSource commandsSource, ILogContext logContext, LogScopeCommandArgs args)
        {
            commandsSource.OnEndLogScopeCommand?.Invoke(logContext, args);
        }

        public static void RaiseOnLogMessageCommand(CommandsSource commandsSource, ILogContext logContext, LogMessageCommandArgs args)
        {
            commandsSource.OnLogMessageCommand?.Invoke(logContext, args);
        }
    }

    public class TestCommandsSource : ITestCommandsSource
    {
        public event TestCommandHandler<TestAttributesCommandArgs> OnGetTestAttributes;

        public event TestCommandHandler<TestAttributesCommandArgs> OnAddTestAttributes;

        public event TestCommandHandler<TestAttributesCommandArgs> OnRemoveTestAttributes;

        public static void RaiseOnGetTestAttributes(TestCommandsSource commandsSource, ITestContext testContext, TestAttributesCommandArgs args)
        {
            commandsSource.OnGetTestAttributes?.Invoke(testContext, args);
        }

        public static void RaiseOnAddTestAttributes(TestCommandsSource commandsSource, ITestContext testContext, TestAttributesCommandArgs args)
        {
            commandsSource.OnAddTestAttributes?.Invoke(testContext, args);
        }

        public static void RaiseOnRemoveTestAttributes(TestCommandsSource commandsSource, ITestContext testContext, TestAttributesCommandArgs args)
        {
            commandsSource.OnRemoveTestAttributes?.Invoke(testContext, args);
        }
    }
}
