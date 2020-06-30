using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Execution.Logging;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    public class LogMessageCommandArgs
    {
        public LogMessageCommandArgs(ILogScope logScope, CreateLogItemRequest createLogItemRequest)
        {
            LogScope = logScope;
            LogItemRequest = createLogItemRequest;
        }

        public ILogScope LogScope { get; }

        public CreateLogItemRequest LogItemRequest { get; }
    }
}
