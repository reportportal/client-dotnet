using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility.Embedded.LogFormatters;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.Embedded.LogFormatter
{
    public class Base64LogFormatterTest
    {
        private Mock<IReportEventsSource> _eventSource;
        private Base64LogFormatter _formatter;

        public Base64LogFormatterTest()
        {
            _eventSource = new Mock<IReportEventsSource>();

            _formatter = new Base64LogFormatter();
            _formatter.Initialize(_eventSource.Object);
        }

        [Fact]
        public void ShouldNotFormatNullBase64String()
        {
            var logRequests = new List<CreateLogItemRequest> { new CreateLogItemRequest() };

            _eventSource.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests));

            logRequests[0].Text.Should().BeNull();
        }

        [Fact]
        public void ShouldNotFormatEmptyBase64String()
        {
            var logRequest = new CreateLogItemRequest() { Text = "" };
            var logRequests = new List<CreateLogItemRequest> { logRequest };

            _eventSource.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests));

            logRequest.Text.Should().Be("");
        }

        [Fact]
        public void ShouldFormatBase64String()
        {
            var data = new byte[] { 1, 2, 3 };
            var base64 = Convert.ToBase64String(data);

            var logRequest = new CreateLogItemRequest() { Text = $"{{rp#base64#image/png#{base64}}}" };
            var logRequests = new List<CreateLogItemRequest> { logRequest };

            _eventSource.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests));

            logRequest.Attach.Should().NotBeNull();
            logRequest.Attach.MimeType.Should().Be("image/png");
            logRequest.Attach.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void ShouldThrowFormatIncorrectBase64String()
        {
            var incorrectBase64 = "123";

            var logRequest = new CreateLogItemRequest() { Text = $"{{rp#base64#image/png#{incorrectBase64}}}" };
            var logRequests = new List<CreateLogItemRequest> { logRequest };

            _eventSource.Invoking(e => e.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests)))
                .Should()
                .Throw<FormatException>()
#if NET452 || NET46
                .WithMessage("*Invalid length for a Base-64*");
#else
                .WithMessage("*not a valid Base-64 string*");
#endif
        }
    }
}
