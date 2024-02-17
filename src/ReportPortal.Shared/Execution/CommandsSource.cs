using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;
using System.Collections.Generic;

namespace ReportPortal.Shared.Execution
{
    /// <summary>
    /// Represents a source of commands.
    /// </summary>
    public class CommandsSource : ICommandsSource
    {
        private IList<ICommandsListener> _listeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsSource"/> class.
        /// </summary>
        /// <param name="listeners">The list of commands listeners.</param>
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

        /// <summary>
        /// Gets the test commands source.
        /// </summary>
        public ITestCommandsSource TestCommandsSource { get; } = new TestCommandsSource();

        /// <summary>
        /// Occurs when a begin log scope command is raised.
        /// </summary>
        public event LogCommandHandler<LogScopeCommandArgs> OnBeginLogScopeCommand;

        /// <summary>
        /// Occurs when an end log scope command is raised.
        /// </summary>
        public event LogCommandHandler<LogScopeCommandArgs> OnEndLogScopeCommand;

        /// <summary>
        /// Occurs when a log message command is raised.
        /// </summary>
        public event LogCommandHandler<LogMessageCommandArgs> OnLogMessageCommand;

        /// <summary>
        /// Raises the <see cref="OnBeginLogScopeCommand"/> event.
        /// </summary>
        /// <param name="commandsSource">The commands source.</param>
        /// <param name="logContext">The log context.</param>
        /// <param name="args">The command arguments.</param>
        public static void RaiseOnBeginScopeCommand(CommandsSource commandsSource, ILogContext logContext, LogScopeCommandArgs args)
        {
            commandsSource.OnBeginLogScopeCommand?.Invoke(logContext, args);
        }

        /// <summary>
        /// Raises the <see cref="OnEndLogScopeCommand"/> event.
        /// </summary>
        /// <param name="commandsSource">The commands source.</param>
        /// <param name="logContext">The log context.</param>
        /// <param name="args">The command arguments.</param>
        public static void RaiseOnEndScopeCommand(CommandsSource commandsSource, ILogContext logContext, LogScopeCommandArgs args)
        {
            commandsSource.OnEndLogScopeCommand?.Invoke(logContext, args);
        }

        /// <summary>
        /// Raises the <see cref="OnLogMessageCommand"/> event.
        /// </summary>
        /// <param name="commandsSource">The commands source.</param>
        /// <param name="logContext">The log context.</param>
        /// <param name="args">The command arguments.</param>
        public static void RaiseOnLogMessageCommand(CommandsSource commandsSource, ILogContext logContext, LogMessageCommandArgs args)
        {
            commandsSource.OnLogMessageCommand?.Invoke(logContext, args);
        }
    }

    /// <summary>
    /// Represents a source of test commands.
    /// </summary>
    public class TestCommandsSource : ITestCommandsSource
    {
        /// <summary>
        /// Occurs when a get test attributes command is raised.
        /// </summary>
        public event TestCommandHandler<TestAttributesCommandArgs> OnGetTestAttributes;

        /// <summary>
        /// Occurs when an add test attributes command is raised.
        /// </summary>
        public event TestCommandHandler<TestAttributesCommandArgs> OnAddTestAttributes;

        /// <summary>
        /// Occurs when a remove test attributes command is raised.
        /// </summary>
        public event TestCommandHandler<TestAttributesCommandArgs> OnRemoveTestAttributes;

        /// <summary>
        /// Raises the <see cref="OnGetTestAttributes"/> event.
        /// </summary>
        /// <param name="commandsSource">The commands source.</param>
        /// <param name="testContext">The test context.</param>
        /// <param name="args">The command arguments.</param>
        public static void RaiseOnGetTestAttributes(TestCommandsSource commandsSource, ITestContext testContext, TestAttributesCommandArgs args)
        {
            commandsSource.OnGetTestAttributes?.Invoke(testContext, args);
        }

        /// <summary>
        /// Raises the <see cref="OnAddTestAttributes"/> event.
        /// </summary>
        /// <param name="commandsSource">The commands source.</param>
        /// <param name="testContext">The test context.</param>
        /// <param name="args">The command arguments.</param>
        public static void RaiseOnAddTestAttributes(TestCommandsSource commandsSource, ITestContext testContext, TestAttributesCommandArgs args)
        {
            commandsSource.OnAddTestAttributes?.Invoke(testContext, args);
        }

        /// <summary>
        /// Raises the <see cref="OnRemoveTestAttributes"/> event.
        /// </summary>
        /// <param name="commandsSource">The commands source.</param>
        /// <param name="testContext">The test context.</param>
        /// <param name="args">The command arguments.</param>
        public static void RaiseOnRemoveTestAttributes(TestCommandsSource commandsSource, ITestContext testContext, TestAttributesCommandArgs args)
        {
            commandsSource.OnRemoveTestAttributes?.Invoke(testContext, args);
        }
    }
}
