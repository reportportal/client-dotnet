using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using System.Collections.Generic;
using Xunit;

namespace ReportPortal.Shared.Tests.Execution
{
    public class LogFixture
    {
        private string text = "text";
        private string mimeType = "image/png";
        private byte[] data = new byte[] { 1 };

        CreateLogItemRequest logRequest;
        ICommandsListener listener;
        ITestContext testContext;

        public LogFixture()
        {
            var mockListener = new Mock<ICommandsListener>();
            mockListener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => logRequest = b.LogItemRequest;
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

            VerifyLogMessage(logRequest, LogLevel.Debug);

            testContext.Log.Debug(text, mimeType, data);

            VerifyLogMessageWithAttach(logRequest, LogLevel.Debug);
        }

        [Fact]
        public void ShouldRaiseLogErrorMessage()
        {
            testContext.Log.Error(text);

            VerifyLogMessage(logRequest, LogLevel.Error);

            testContext.Log.Error(text, mimeType, data);

            VerifyLogMessageWithAttach(logRequest, LogLevel.Error);
        }

        [Fact]
        public void ShouldRaiseLogFatalMessage()
        {
            testContext.Log.Fatal(text);

            VerifyLogMessage(logRequest, LogLevel.Fatal);

            testContext.Log.Fatal(text, mimeType, data);

            VerifyLogMessageWithAttach(logRequest, LogLevel.Fatal);
        }

        [Fact]
        public void ShouldRaiseLogInfoMessage()
        {
            testContext.Log.Info(text);

            VerifyLogMessage(logRequest, LogLevel.Info);

            testContext.Log.Info(text, mimeType, data);

            VerifyLogMessageWithAttach(logRequest, LogLevel.Info);
        }

        [Fact]
        public void ShouldRaiseLogTraceMessage()
        {
            testContext.Log.Trace(text);

            VerifyLogMessage(logRequest, LogLevel.Trace);

            testContext.Log.Trace(text, mimeType, data);

            VerifyLogMessageWithAttach(logRequest, LogLevel.Trace);
        }

        [Fact]
        public void ShouldRaiseLogWarnMessage()
        {
            testContext.Log.Warn(text);

            VerifyLogMessage(logRequest, LogLevel.Warning);

            testContext.Log.Warn(text, mimeType, data);

            VerifyLogMessageWithAttach(logRequest, LogLevel.Warning);
        }

        private void VerifyLogMessage(CreateLogItemRequest logRequest, LogLevel level)
        {
            logRequest.Level.Should().Be(level);
            logRequest.Text.Should().Be(text);
        }

        private void VerifyLogMessageWithAttach(CreateLogItemRequest logRequest, LogLevel level)
        {
            logRequest.Level.Should().Be(level);
            logRequest.Text.Should().Be(text);
            logRequest.Attach.Should().NotBeNull();
            logRequest.Attach.MimeType.Should().Be(mimeType);
            logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }
    }
}
