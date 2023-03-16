using ReportPortal.Client.Abstractions.Requests;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.Negative
{
    public class NegativeFixture
    {
        [Fact]
        public async Task IncorrectHost()
        {
            var service = new Service(new Uri("https://abc.abc/"), "p", "p", ignoreSslErrors: true);
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.Launch.GetAsync("123"));
        }

        [Fact]
        public async Task IncorrectUrlInCorrectHost()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/blabla/"), "p", "p", ignoreSslErrors: true);
            await Assert.ThrowsAsync<ServiceException>(async () => await service.Launch.StartAsync(new StartLaunchRequest { Name = "abc" }));
        }

        [Fact]
        public async Task IncorrectUuid()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "incorrect_uuid", ignoreSslErrors: true);
            await Assert.ThrowsAsync<ServiceException>(async () => await service.Launch.GetAsync());
        }
    }
}
