using FluentAssertions;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Shared.Execution.Logging;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Execution
{
    public class LogMessageExtensionsFixture
    {
        [Fact]
        public void ShouldNotConvertNullableLogMessage()
        {
            ILogMessage logMessage = null;

            Action act = () => logMessage.ConvertToRequest();

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ShouldNotCreateNullableOrEmptyLogMessage()
        {
            Action ctor = () => new LogMessage(null);

            ctor.Should().ThrowExactly<ArgumentException>();

            ctor = () => new LogMessage("");

            ctor.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [InlineData(LogMessageLevel.Info, LogLevel.Info)]
        [InlineData(LogMessageLevel.Warning, LogLevel.Warning)]
        [InlineData(LogMessageLevel.Debug, LogLevel.Debug)]
        [InlineData(LogMessageLevel.Trace, LogLevel.Trace)]
        [InlineData(LogMessageLevel.Error, LogLevel.Error)]
        [InlineData(LogMessageLevel.Fatal, LogLevel.Fatal)]
        public void ShouldConvertLogMessage(LogMessageLevel level, LogLevel expectedLevel)
        {
            ILogMessage logMessage = new LogMessage("message") { Level = level };

            var request = logMessage.ConvertToRequest();

            request.Text.Should().Be("message");
            request.Level.Should().Be(expectedLevel);
            request.Time.Should().Be(logMessage.Time);

            request.Attach.Should().BeNull();
        }

        [Fact]
        public void ShouldConvertLogMessageWithAttachment()
        {
            ILogMessage logMessage = new LogMessage("message") { Attachment = new LogMessageAttachment("image/png", new byte[] { 1, 2, 3 }) };

            var request = logMessage.ConvertToRequest();

            request.Text.Should().Be("message");
            request.Level.Should().Be(LogLevel.Info);
            request.Time.Should().Be(logMessage.Time);

            request.Attach.Should().NotBeNull();
            request.Attach.MimeType.Should().Be("image/png");
            request.Attach.Data.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }
    }
}
