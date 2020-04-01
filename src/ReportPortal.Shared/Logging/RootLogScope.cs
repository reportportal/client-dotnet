using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Logging
{
    class RootLogScope : BaseLogScope
    {
        public RootLogScope(ILogScopeManager logScopeManager) : base(logScopeManager)
        {

        }

        public override void Message(CreateLogItemRequest logRequest)
        {
            foreach (var handler in Bridge.LogHandlerExtensions)
            {
                var isHandled = handler.Handle(null, logRequest);

                if (isHandled) break;
            }
        }

        public override ILogScope BeginNewScope(string name)
        {
            var logScope = new LogScope(_logScopeManager, null, name);
            _logScopeManager.ActiveScope = logScope;

            return logScope;
        }
    }
}
