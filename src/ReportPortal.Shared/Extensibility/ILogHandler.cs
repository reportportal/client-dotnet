using ReportPortal.Client.Requests;

namespace ReportPortal.Shared.Extensibility
{
    /// <summary>
    /// Handle all incoming log messages to <see cref="Bridge.LogMessage(Client.Models.LogLevel, string)"/>. Usually from log frameworks.
    /// </summary>
    public interface ILogHandler
    {
        int Order { get; }

        bool Handle(AddLogItemRequest logRequest);
    }
}
