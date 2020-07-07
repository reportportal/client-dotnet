using FluentAssertions;
using Moq;
using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using System.Collections.Generic;
using Xunit;

namespace ReportPortal.Shared.Tests.Execution
{
    public class LogFixture
    {
        private readonly string text = "text";
        private readonly string mimeType = "image/png";
        private readonly byte[] data = new byte[] { 1 };

        ILogMessage logMessage;
        readonly ICommandsListener listener;
        readonly ITestContext testContext;

        public LogFixture()
        {
            var mockListener = new Mock<ICommandsListener>();
            mockListener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => logMessage = b.LogMessage;
            });

            listener = mockListener.Object;

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener);
            
            testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { mockListener.Object }));
        }

        [Fact]
        public void ShouldRaiseLogDebugMessage()
        {
            testContext.Log.Debug(text);

            VerifyLogMessage(logMessage, LogMessageLevel.Debug);

            testContext.Log.Debug(text, mimeType, data);

            VerifyLogMessageWithAttach(logMessage, LogMessageLevel.Debug);
        }

        [Fact]
        public void ShouldRaiseLogErrorMessage()
        {
            testContext.Log.Error(text);

            VerifyLogMessage(logMessage, LogMessageLevel.Error);

            testContext.Log.Error(text, mimeType, data);

            VerifyLogMessageWithAttach(logMessage, LogMessageLevel.Error);
        }

        [Fact]
        public void ShouldRaiseLogFatalMessage()
        {
            testContext.Log.Fatal(text);

            VerifyLogMessage(logMessage, LogMessageLevel.Fatal);

            testContext.Log.Fatal(text, mimeType, data);

            VerifyLogMessageWithAttach(logMessage, LogMessageLevel.Fatal);
        }

        [Fact]
        public void ShouldRaiseLogInfoMessage()
        {
            testContext.Log.Info(text);

            VerifyLogMessage(logMessage, LogMessageLevel.Info);

            testContext.Log.Info(text, mimeType, data);

            VerifyLogMessageWithAttach(logMessage, LogMessageLevel.Info);
        }

        [Fact]
        public void ShouldRaiseLogTraceMessage()
        {
            testContext.Log.Trace(text);

            VerifyLogMessage(logMessage, LogMessageLevel.Trace);

            testContext.Log.Trace(text, mimeType, data);

            VerifyLogMessageWithAttach(logMessage, LogMessageLevel.Trace);
        }

        [Fact]
        public void ShouldRaiseLogWarnMessage()
        {
            testContext.Log.Warn(text);

            VerifyLogMessage(logMessage, LogMessageLevel.Warning);

            testContext.Log.Warn(text, mimeType, data);

            VerifyLogMessageWithAttach(logMessage, LogMessageLevel.Warning);
        }

        private void VerifyLogMessage(ILogMessage logMessage, LogMessageLevel level)
        {
            logMessage.Level.Should().Be(level);
            logMessage.Message.Should().Be(text);
        }

        private void VerifyLogMessageWithAttach(ILogMessage logMessage, LogMessageLevel level)
        {
            logMessage.Level.Should().Be(level);
            logMessage.Message.Should().Be(text);
            logMessage.Attachment.Should().NotBeNull();
            logMessage.Attachment.MimeType.Should().Be(mimeType);
            logMessage.Attachment.Data.Should().BeEquivalentTo(data);
        }
    }
}
