using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Requests;
using NUnit.Framework;

namespace ReportPortal.Client.Tests.LaunchItem
{
    [TestFixture]
    public class NegativeFixture : BaseFixture
    {
        [Test]
        public void IncorrectHost()
        {
            var service = new Service(new Uri("https://abc.abc/"), "p", "p");
            Assert.That(() => service.GetLaunch("123"), Throws.Exception);
        }

        [Test]
        public void IncorrectUrlInCorrectHost()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/blabla/"), "p", "p");
            Assert.That(() => service.StartLaunch(new StartLaunchRequest { Name = "abc" }), Throws.Exception);
        }

        [Test]
        public void IncorrectUuid()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "incorrect_uuid");
            Assert.That(() => service.GetLaunches(), Throws.Exception);
        }
    }
}
