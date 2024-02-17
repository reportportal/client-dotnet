using ReportPortal.Shared.Extensibility.Commands;

namespace ReportPortal.Shared.Extensibility
{
    /// <summary>
    /// Represents an interface for a commands listener.
    /// </summary>
    public interface ICommandsListener
    {
        /// <summary>
        /// Initializes the commands listener with the specified commands source.
        /// </summary>
        /// <param name="commandsSource">The commands source to initialize with.</param>
        void Initialize(ICommandsSource commandsSource);
    }
}
