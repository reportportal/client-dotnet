using ReportPortal.Shared.Execution.Logging;

namespace ReportPortal.Shared.Execution
{
    /// <summary>
    /// Provides api to send contextual log messages or log scopes.
    /// </summary>
    public interface ILogContext
    {
        /// <summary>
        /// Current contextual <see href="ILogScope"/>
        /// </summary>
        /// <value>Instance of <see href="ILogScope"/></value>
        ILogScope Log { get; set; }
    }
}
