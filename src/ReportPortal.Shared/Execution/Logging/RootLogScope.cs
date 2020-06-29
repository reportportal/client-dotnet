using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;

namespace ReportPortal.Shared.Execution.Logging
{
    class RootLogScope : BaseLogScope
    {
        public RootLogScope(ITestContext testContext, IExtensionManager extensionManager) : base(testContext, extensionManager)
        {

        }

        public override LogScopeStatus Status { get => base.Status; set { } }

        public override void Message(CreateLogItemRequest logRequest)
        {
            foreach (var handler in _extensionManager.LogHandlers)
            {
                var isHandled = handler.Handle(null, logRequest);

                if (isHandled) break;
            }
        }

        public override ILogScope BeginScope(string name)
        {
            var logScope = new LogScope(_testContext, _extensionManager, this, null, name);

            _testContext.Log = logScope;

            return logScope;
        }

        public override ILogScope Root { get => this; protected set => base.Root = value; }
    }
}
