using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;

namespace ReportPortal.Shared.Logging
{
    class RootLogScope : BaseLogScope
    {
        public RootLogScope(ILogScopeManager logScopeManager, IExtensionManager extensionManager) : base(logScopeManager, extensionManager)
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
            var logScope = new LogScope(_logScopeManager, _extensionManager, null, name);
            _logScopeManager.ActiveScope = logScope;

            return logScope;
        }
    }
}
