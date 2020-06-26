using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Execution.Log;
using ReportPortal.Shared.Extensibility;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    [Collection("Static")]
    public class EntryLogTest : IDisposable
    {
        private string text = "text";
        private string mimeType = "image/png";
        private byte[] data = new byte[] { 1 };

        private Mock<ILogHandler> _logHandler;

        public EntryLogTest()
        {
            _logHandler = new Mock<ILogHandler>();
            _logHandler.Setup(lh => lh.Handle(It.IsAny<ILogScope>(), It.IsAny<CreateLogItemRequest>())).Callback((ILogScope ls, CreateLogItemRequest clr) => { _logRequest = clr; });

            ExtensionManager.Instance.LogHandlers.Add(_logHandler.Object);
        }

        private CreateLogItemRequest _logRequest;

        public int Order => 10;

        [Fact]
        public void ShouldHandleTraceMessage()
        {
            Log.Trace(text);
            _logRequest.Level.Should().Be(LogLevel.Trace);
            _logRequest.Text.Should().Be(text);

            Log.Trace(text, mimeType, data);

            _logRequest.Level.Should().Be(LogLevel.Trace);
            _logRequest.Text.Should().Be(text);
            _logRequest.Attach.Should().NotBeNull();
            _logRequest.Attach.MimeType.Should().Be(mimeType);
            _logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void ShouldHandleDebugMessage()
        {
            Log.Debug(text);
            _logRequest.Level.Should().Be(LogLevel.Debug);
            _logRequest.Text.Should().Be(text);

            Log.Debug(text, mimeType, data);

            _logRequest.Level.Should().Be(LogLevel.Debug);
            _logRequest.Text.Should().Be(text);
            _logRequest.Attach.Should().NotBeNull();
            _logRequest.Attach.MimeType.Should().Be(mimeType);
            _logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void ShouldHandleWarnMessage()
        {
            Log.Warn(text);
            _logRequest.Level.Should().Be(LogLevel.Warning);
            _logRequest.Text.Should().Be(text);

            Log.Warn(text, mimeType, data);

            _logRequest.Level.Should().Be(LogLevel.Warning);
            _logRequest.Text.Should().Be(text);
            _logRequest.Attach.Should().NotBeNull();
            _logRequest.Attach.MimeType.Should().Be(mimeType);
            _logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void ShouldHandleErrorMessage()
        {
            Log.Error(text);
            _logRequest.Level.Should().Be(LogLevel.Error);
            _logRequest.Text.Should().Be(text);

            Log.Error(text, mimeType, data);

            _logRequest.Level.Should().Be(LogLevel.Error);
            _logRequest.Text.Should().Be(text);
            _logRequest.Attach.Should().NotBeNull();
            _logRequest.Attach.MimeType.Should().Be(mimeType);
            _logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void ShouldHandleFatalMessage()
        {
            Log.Fatal(text);
            _logRequest.Level.Should().Be(LogLevel.Fatal);
            _logRequest.Text.Should().Be(text);

            Log.Fatal(text, mimeType, data);

            _logRequest.Level.Should().Be(LogLevel.Fatal);
            _logRequest.Text.Should().Be(text);
            _logRequest.Attach.Should().NotBeNull();
            _logRequest.Attach.MimeType.Should().Be(mimeType);
            _logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void ShouldHandleInfoMessage()
        {
            Log.Info(text);
            _logRequest.Level.Should().Be(LogLevel.Info);
            _logRequest.Text.Should().Be(text);

            Log.Info(text, mimeType, data);

            _logRequest.Level.Should().Be(LogLevel.Info);
            _logRequest.Text.Should().Be(text);
            _logRequest.Attach.Should().NotBeNull();
            _logRequest.Attach.MimeType.Should().Be(mimeType);
            _logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        public void Dispose()
        {
            ExtensionManager.Instance.LogHandlers.Remove(_logHandler.Object);
        }
    }
}
