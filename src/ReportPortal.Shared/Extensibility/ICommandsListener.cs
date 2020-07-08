using ReportPortal.Shared.Extensibility.Commands;

namespace ReportPortal.Shared.Extensibility
{
    public interface ICommandsListener
    {
        void Initialize(ICommandsSource commandsSource);
    }
}
