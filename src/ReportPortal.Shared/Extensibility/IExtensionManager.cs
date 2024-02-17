using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility
{
    /// <summary>
    /// Represents an interface for managing extensions.
    /// </summary>
    public interface IExtensionManager
    {
        /// <summary>
        /// Explores the specified path for extensions.
        /// </summary>
        /// <param name="path">The path to explore.</param>
        void Explore(string path);

        /// <summary>
        /// Gets the list of report event observers.
        /// </summary>
        IList<IReportEventsObserver> ReportEventObservers { get; }

        /// <summary>
        /// Gets the list of commands listeners.
        /// </summary>
        IList<ICommandsListener> CommandsListeners { get; }
    }
}
