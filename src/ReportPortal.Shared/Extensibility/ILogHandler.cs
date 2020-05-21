using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Logging;

namespace ReportPortal.Shared.Extensibility
{
    /// <summary>
    /// Handle all incoming log messages to <see cref="Log.Message(CreateLogItemRequest)"/>. Usually from log frameworks.
    /// </summary>
    public interface ILogHandler
    {
        /// <summary>
        /// Order of the handler in chain of registered handlers.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// You are aware what to do with specific log message.
        /// </summary>
        /// <param name="logScope">Logging scope which log message belongs to.</param>
        /// <param name="logRequest">Actual log message request.</param>
        /// <returns>True if you handled log message. Otherwise engine asks others extensions in the chain. Can be null if there is no active logging scope.</returns>
        bool Handle(ILogScope logScope, CreateLogItemRequest logRequest);

        /// <summary>
        /// Notifies whether new logging scope (aka nested step) should be started.
        /// </summary>
        /// <param name="logScope">The information about log scope.</param>
        void BeginScope(ILogScope logScope);

        /// <summary>
        /// Notifies whether specified logging scope (aka nested step) should be finished.
        /// </summary>
        /// <param name="logScope">Logging scope to finish.</param>
        void EndScope(ILogScope logScope);
    }
}
