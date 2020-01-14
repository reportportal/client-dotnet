using FluentAssertions;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Extensibility.LogFormatter;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.LogFormatter
{
    public class Base64LogFormatterTest
    {
        [Fact]
        public void ShouldNotFormatNullBase64String()
        {
            var formatter = new Base64LogFormatter();

            var logRequest = new AddLogItemRequest();

            var isHandled = formatter.FormatLog(ref logRequest);
            isHandled.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotFormatEmptyBase64String()
        {
            var formatter = new Base64LogFormatter();

            var logRequest = new AddLogItemRequest() { Text = "" };

            var isHandled = formatter.FormatLog(ref logRequest);
            isHandled.Should().BeFalse();
        }

        [Fact]
        public void ShouldFormatBase64String()
        {
            var formatter = new Base64LogFormatter();

            var data = new byte[] { 1, 2, 3 };
            var base64 = Convert.ToBase64String(data);

            var logRequest = new AddLogItemRequest() { Text = $"{{rp#base64#image/png#{base64}}}" };

            var isHandled = formatter.FormatLog(ref logRequest);
            isHandled.Should().BeTrue();
            logRequest.Attach.Should().NotBeNull();
            logRequest.Attach.MimeType.Should().Be("image/png");
            logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void ShouldThrowFormatIncorrectBase64String()
        {
            var formatter = new Base64LogFormatter();

            var incorrectBase64 = "123";

            var logRequest = new AddLogItemRequest() { Text = $"{{rp#base64#image/png#{incorrectBase64}}}" };

            formatter.Invoking(f => f.FormatLog(ref logRequest)).Should().Throw<Exception>();
        }
    }
}
