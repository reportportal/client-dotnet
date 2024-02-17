using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;

namespace ReportPortal.Shared.Extensibility.Commands
{
    /// <summary>
    /// Represents a source of test commands.
    /// </summary>
    public interface ITestCommandsSource
    {
        /// <summary>
        /// Occurs when a test command to get test attributes is raised.
        /// </summary>
        event TestCommandHandler<TestAttributesCommandArgs> OnGetTestAttributes;

        /// <summary>
        /// Occurs when a test command to add test attributes is raised.
        /// </summary>
        event TestCommandHandler<TestAttributesCommandArgs> OnAddTestAttributes;

        /// <summary>
        /// Occurs when a test command to remove test attributes is raised.
        /// </summary>
        event TestCommandHandler<TestAttributesCommandArgs> OnRemoveTestAttributes;
    }

    /// <summary>
    /// Represents a delegate for handling test commands.
    /// </summary>
    /// <typeparam name="TCommandArgs">The type of the command arguments.</typeparam>
    /// <param name="testContext">The test context.</param>
    /// <param name="args">The command arguments.</param>
    public delegate void TestCommandHandler<TCommandArgs>(ITestContext testContext, TCommandArgs args);
}
