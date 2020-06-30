using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using System;

namespace ReportPortal.Shared.Execution.Logging
{
    class LogScope : BaseLogScope
    {
        public LogScope(ITestContext testContext, IExtensionManager extensionManager, CommandsSource commandsSource, ILogScope root, ILogScope parent, string name) : base(testContext, extensionManager, commandsSource)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Log scope name cannot be null of empty.", nameof(name));
            }

            Root = root;
            Parent = parent;
            Name = name;

            CommandsSource.RaiseOnBeginScopeCommand(commandsSource, testContext, this);

            foreach (var logHandler in _extensionManager.LogHandlers)
            {
                logHandler.BeginScope(this);
            }
        }

        public override ILogScope Parent { get; }

        public override string Name { get; }

        public override void Dispose()
        {
            base.Dispose();

            CommandsSource.RaiseOnEndScopeCommand(_commandsSource, _testContext, this);

            foreach (var logHandler in _extensionManager.LogHandlers)
            {
                logHandler.EndScope(this);
            }

            _testContext.Log = Parent;
        }
    }
}
