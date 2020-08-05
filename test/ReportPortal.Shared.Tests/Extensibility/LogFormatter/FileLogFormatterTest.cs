using FluentAssertions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility.LogFormatter;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.LogFormatter
{
    public class FileLogFormatterTest
    {
        [Fact]
        public void ShouldNotFormatNullFileString()
        {
            var formatter = new FileLogFormatter();

            var logRequest = new CreateLogItemRequest();

            var isHandled = formatter.FormatLog(logRequest);
            isHandled.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotFormatEmptyString()
        {
            var formatter = new FileLogFormatter();

            var logRequest = new CreateLogItemRequest() { Text = "" };

            var isHandled = formatter.FormatLog(logRequest);
            isHandled.Should().BeFalse();
        }

        [Fact]
        public void ShouldFormatFileString()
        {
            var formatter = new FileLogFormatter();

            var data = "123";
            var filePath = Path.GetTempPath() + Path.GetRandomFileName();
            File.WriteAllText(filePath, data);

            var logRequest = new CreateLogItemRequest() { Text = $"{{rp#file#{filePath}}}" };

            var isHandled = formatter.FormatLog(logRequest);
            isHandled.Should().BeTrue();
            logRequest.Attach.Should().NotBeNull();
            logRequest.Attach.Data.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(data));
        }

        [Fact]
        public void ShouldThrowFormatIncorrectFilePathString()
        {
            var formatter = new FileLogFormatter();

            var incorrectFilePath = "q.w";

            var logRequest = new CreateLogItemRequest() { Text = $"{{rp#file#{incorrectFilePath}}}" };

            formatter.FormatLog(logRequest);

            logRequest.Text.Should().Contain("Cannot");
        }
    }
}
