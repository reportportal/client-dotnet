using FluentAssertions;
using Moq;
using ReportPortal.Shared.Extensibility.Embedded.Analytics;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using RichardSzalay.MockHttp;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.Embedded.Analytics
{
    public class AnalyticsReportEventsObserverTest
    {
        [Fact]
        public void ShouldHaveCorrectFormat()
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            mockHttpHandler.Expect(HttpMethod.Post, "https://www.google-analytics.com/mp/collect").With(m =>
            {
                var queryParams = m.RequestUri.Query.TrimStart('?').Split('&')
                .Select(pair => pair.Split(new char[] { '=' }, 2))
                .ToDictionary(pair => HttpUtility.UrlDecode(pair[0]), pair => pair.Length == 2 ? HttpUtility.UrlDecode(pair[1]) : "");
                if (queryParams.Count() == 0)
                {
                    return false;
                }
                var isOk = queryParams.ContainsKey("measurement_id") && queryParams["measurement_id"].StartsWith("G-");
                isOk = isOk && queryParams.ContainsKey("api_secret") && !string.IsNullOrEmpty(queryParams["api_secret"]);
                isOk = isOk && m.Content.Headers.ContentType.ToString().StartsWith("application/json");
                var content = m.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                isOk = isOk && content.Contains("\"client_id\":\"");
                isOk = isOk && content.Contains("\"events\":[{\"");
                isOk = isOk && content.Contains("\"params\":{\"");
                return isOk;
            }).Respond(HttpStatusCode.OK);

            var analyticsObserver = new AnalyticsReportEventsObserver(mockHttpHandler);
            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(analyticsObserver);

            var client = new MockServiceBuilder().Build().Object;
            new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0).Sync();

            mockHttpHandler.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public void ShouldNotThrow()
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            mockHttpHandler.Expect(HttpMethod.Post, "https://www.google-analytics.com/mp/collect").Respond(HttpStatusCode.InternalServerError);

            var analyticsObserver = new AnalyticsReportEventsObserver(mockHttpHandler);
            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(analyticsObserver);

            var client = new MockServiceBuilder().Build().Object;
            new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0).Sync();

            mockHttpHandler.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public void ShouldDefineConsumer()
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            mockHttpHandler.Expect(HttpMethod.Post, "https://www.google-analytics.com/mp/collect").Respond(HttpStatusCode.InternalServerError);

            AnalyticsReportEventsObserver.DefineConsumer("agent1", "5.0");

            AnalyticsReportEventsObserver.AgentName.Should().Be("agent1");
            AnalyticsReportEventsObserver.AgentVersion.Should().Be("5.0");
        }

        [Fact]
        public void ShouldDefineConsumerWithEmptyAgentName()
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            mockHttpHandler.Expect(HttpMethod.Post, "https://www.google-analytics.com/mp/collect").Respond(HttpStatusCode.InternalServerError);

            AnalyticsReportEventsObserver.DefineConsumer(null);

            AnalyticsReportEventsObserver.AgentName.Should().Be("ReportPortal.Shared.Tests");
            AnalyticsReportEventsObserver.AgentVersion.Should().Be("1.0.0");
        }

        [Fact]
        public void ShouldThrowIfEventsSourceIsNull()
        {
            var ga = new AnalyticsReportEventsObserver();

            Action act = () => ga.Initialize(reportEventsSource: null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldBeSilentIfLaunchIsNotStartedButFinished()
        {
            var ga = new AnalyticsReportEventsObserver();

            var launchReporter = new Mock<ILaunchReporter>();

            var source = new Mock<IReportEventsSource>();

            ga.Initialize(source.Object);

            source.Raise(es => es.OnAfterLaunchFinished += null, launchReporter.Object, new AfterLaunchFinishedEventArgs(null, null));
        }

        [Fact]
        public void ShouldDispose()
        {
            var ga = new AnalyticsReportEventsObserver();

            ga.Initialize(new ReportEventsSource());

            ga.Dispose();
        }
    }
}
