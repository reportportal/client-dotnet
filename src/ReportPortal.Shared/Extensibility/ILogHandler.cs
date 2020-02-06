using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Extensibility
{
    /// <summary>
    /// Handle all incoming log messages to <see cref="Log.Message(CreateLogItemRequest)"/>. Usually from log frameworks.
    /// </summary>
    public interface ILogHandler
    {
        int Order { get; }

        bool Handle(CreateLogItemRequest logRequest);
    }
}
