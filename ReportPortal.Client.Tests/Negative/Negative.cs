using System;
using ReportPortal.Client.Requests;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;
using Xunit;

namespace ReportPortal.Client.Tests
{
    public class NegativeFixture : BaseFixture
    {
        [Fact]
        public async Task IncorrectHost()
        {
            var service = new Service(new Uri("https://abc.abc/"), "p", "p");
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetLaunchAsync("123"));
        }

        [Fact]
        public async Task IncorrectUrlInCorrectHost()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/blabla/"), "p", "p");
            await Assert.ThrowsAsync<SerializationException>(async () => await service.StartLaunchAsync(new StartLaunchRequest { Name = "abc" }));
        }

        [Fact]
        public async Task IncorrectUuid()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "incorrect_uuid");
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetLaunchesAsync());
        }
    }
}
