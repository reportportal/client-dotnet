﻿using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ReportPortal.Client.Api.Launch.Requests;
using Xunit;

namespace ReportPortal.Client.Tests.Negative
{
    public class NegativeFixture : BaseFixture
    {
        [Fact]
        public async Task IncorrectHost()
        {
            var service = new ReportPortalClient(new Uri("https://abc.abc/"), "p", "p");
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.Launch.GetLaunchAsync("123"));
        }

        [Fact]
        public async Task IncorrectUrlInCorrectHost()
        {
            var service = new ReportPortalClient(new Uri("https://rp.epam.com/api/blabla/"), "p", "p");
            await Assert.ThrowsAsync<SerializationException>(async () => await service.Launch.StartLaunchAsync(new StartLaunchRequest { Name = "abc" }));
        }

        [Fact]
        public async Task IncorrectUuid()
        {
            var service = new ReportPortalClient(new Uri("https://rp.epam.com/api/v1/"), "default_project", "incorrect_uuid");
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.Launch.GetLaunchesAsync());
        }
    }
}
