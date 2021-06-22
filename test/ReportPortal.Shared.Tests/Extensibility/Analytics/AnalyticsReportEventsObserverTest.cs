using ReportPortal.Shared.Extensibility.Analytics;
using ReportPortal.Shared.Tests.Helpers;
using RichardSzalay.MockHttp;
using RichardSzalay.MockHttp.Matchers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.Analytics
{
    public class AnalyticsReportEventsObserverTest
    {
        private const string CATEGORY_VALIDATION_PATTERN = "Client name \"[^\"]+\", version \"[^\"]+\", interpreter \".NET[^\"]+\"";

        [Fact]
        public void ShouldHAveCorrectCategoryFormat()
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            mockHttpHandler.Expect(HttpMethod.Post, "https://www.google-analytics.com/collect").With(new CustomMatcher(m =>
            {
                var content = m.RequestUri.Query.TrimStart('?').Split('&')
                .Select(pair => pair.Split(new char[] { '=' }, 2))
                .ToDictionary(pair => HttpUtility.UrlDecode(pair[0]), pair => pair.Length == 2 ? HttpUtility.UrlDecode(pair[1]) : "");
                if (content.Count() == 0)
                {
                    return false;
                }
                return Regex.IsMatch(content["ec"], CATEGORY_VALIDATION_PATTERN);
            })).Respond(HttpStatusCode.OK);

            var analyticsObserver = new AnalyticsReportEventsObserver(mockHttpHandler);
            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(analyticsObserver);

            var client = new MockServiceBuilder().Build().Object;
            new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0).Sync();

            mockHttpHandler.VerifyNoOutstandingExpectation();
        }
    }
}
