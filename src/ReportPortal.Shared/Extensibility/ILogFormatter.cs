using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Extensibility
{
    /// <summary>
    /// Extensibility point to bring ability to modify log message produced by tests.
    /// </summary>
    public interface ILogFormatter
    {
        /// <summary>
        /// Order of the formatter in chain of registered log message formatters.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Modify log message before sending it to the server.
        /// </summary>
        /// <param name="logRequest">Log message to format</param>
        /// <returns>Specify whether log message is formatted and should not be sent up to formatters chain.</returns>
        bool FormatLog(CreateLogItemRequest logRequest);
    }
}
