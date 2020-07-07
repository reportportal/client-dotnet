using Moq;
using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.LogHandler
{
    [Collection("Static")]
    public class LogHandlerTest
    {
        [Fact]
        public void ShouldInvokeHandleLogMethod()
        {
            var logHandler = new Mock<ILogHandler>();

            Shared.Extensibility.ExtensionManager.Instance.LogHandlers.Add(logHandler.Object);
            Context.Current.Log.Info("message from test domain");
            Log.Info("message from test domain");
            Shared.Extensibility.ExtensionManager.Instance.LogHandlers.Remove(logHandler.Object);

            logHandler.Verify(lh => lh.Handle(null, It.IsAny<ILogMessage>()), Times.Exactly(2));
        }
    }
}
