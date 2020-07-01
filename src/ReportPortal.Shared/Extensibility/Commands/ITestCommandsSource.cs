using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;

namespace ReportPortal.Shared.Extensibility.Commands
{
    public interface ITestCommandsSource
    {
        event TestCommandHandler<TestAttributesCommandArgs> OnGetTestAttributes;

        event TestCommandHandler<TestAttributesCommandArgs> OnAddTestAttributes;

        event TestCommandHandler<TestAttributesCommandArgs> OnRemoveTestAttributes;
    }

    public delegate void TestCommandHandler<TCommandArgs>(ITestContext testContext, TCommandArgs args);
}
