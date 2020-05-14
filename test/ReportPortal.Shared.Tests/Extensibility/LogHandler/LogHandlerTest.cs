using Moq;
using ReportPortal.Client.Abstractions.Requests;
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
            Log.Info("message from test domain");
            Shared.Extensibility.ExtensionManager.Instance.LogHandlers.Remove(logHandler.Object);

            logHandler.Verify(lh => lh.Handle(null, It.IsAny<CreateLogItemRequest>()), Times.Once);
        }
    }
}
