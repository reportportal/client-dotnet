using ReportPortal.Shared.Extensibility;
using System;

namespace ReportPortal.Shared.Execution.Logging
{
    class LogScope : BaseLogScope
    {
        public LogScope(ITestContext testContext, IExtensionManager extensionManager, ILogScope root, ILogScope parent, string name) : base(testContext, extensionManager)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Log scope name cannot be null of empty.", nameof(name));
            }

            Root = root;
            Parent = parent;
            Name = name;

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

            foreach (var logHandler in _extensionManager.LogHandlers)
            {
                logHandler.EndScope(this);
            }

            _testContext.Log = Parent;
        }
    }
}
