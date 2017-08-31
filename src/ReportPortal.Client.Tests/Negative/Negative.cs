using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Requests;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Tests.LaunchItem
{
    [TestFixture]
    public class NegativeFixture : BaseFixture
    {
        [Test]
        public void IncorrectHost()
        {
            var service = new Service(new Uri("https://abc.abc/"), "p", "p");
            Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetLaunchAsync("123"));
        }

        [Test]
        public void IncorrectUrlInCorrectHost()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/blabla/"), "p", "p");
            Assert.ThrowsAsync<SerializationException>(async () => await service.StartLaunchAsync(new StartLaunchRequest { Name = "abc" }));
        }

        [Test]
        public void IncorrectUuid()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "incorrect_uuid");
            Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetLaunchesAsync());
        }
    }
}
