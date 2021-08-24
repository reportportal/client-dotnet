using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility.Embedded.LogFormatters;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.Embedded.LogFormatter
{
    public class FileLogFormatterTest
    {
        private Mock<IReportEventsSource> _eventSource;
        private FileLogFormatter _formatter;

        public FileLogFormatterTest()
        {
            _eventSource = new Mock<IReportEventsSource>();

            _formatter = new FileLogFormatter();
            _formatter.Initialize(_eventSource.Object);
        }

        [Fact]
        public void ShouldNotFormatNullFileString()
        {
            var logRequests = new List<CreateLogItemRequest> { new CreateLogItemRequest() };

            _eventSource.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests));

            logRequests[0].Text.Should().BeNull();
        }

        [Fact]
        public void ShouldNotFormatEmptyString()
        {
            var logRequests = new List<CreateLogItemRequest> { new CreateLogItemRequest() { Text = "" } };

            _eventSource.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests));

            logRequests[0].Text.Should().Be("");
        }

        [Fact]
        public void ShouldFormatFileString()
        {
            var data = "123";
            var filePath = Path.GetTempPath() + Path.GetRandomFileName();
            File.WriteAllText(filePath, data);

            var logRequest = new CreateLogItemRequest() { Text = $"{{rp#file#{filePath}}}" };

            var logRequests = new List<CreateLogItemRequest> { logRequest };

            _eventSource.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests));

            var formatter = new FileLogFormatter();

            logRequest.Attach.Should().NotBeNull();
            logRequest.Attach.Data.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(data));
        }

        [Fact]
        public void ShouldThrowFormatIncorrectFilePathString()
        {
            var incorrectFilePath = "q.w";

            var logRequest = new CreateLogItemRequest() { Text = $"{{rp#file#{incorrectFilePath}}}" };
            var logRequests = new List<CreateLogItemRequest> { logRequest };

            _eventSource.Raise(es => es.OnBeforeLogsSending += null, null, new BeforeLogsSendingEventArgs(null, null, logRequests));

            logRequest.Text.Should().Contain("Cannot");
        }
    }
}
