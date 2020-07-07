using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;
using System;

namespace ReportPortal.Shared.Execution.Logging
{
    class LogScope : BaseLogScope
    {
        public LogScope(ILogContext logContext, IExtensionManager extensionManager, CommandsSource commandsSource, ILogScope root, ILogScope parent, string name) : base(logContext, extensionManager, commandsSource)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Log scope name cannot be null of empty.", nameof(name));
            }

            Root = root;
            Parent = parent;
            Name = name;

            CommandsSource.RaiseOnBeginScopeCommand(commandsSource, logContext, new LogScopeCommandArgs(this));
        }

        public override ILogScope Parent { get; }

        public override string Name { get; }

        public override void Dispose()
        {
            base.Dispose();

            CommandsSource.RaiseOnEndScopeCommand(_commandsSource, Context, new LogScopeCommandArgs(this));

            Context.Log = Parent;
        }
    }
}
