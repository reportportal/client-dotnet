using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client.Tests.User
{
    public class UserFixture : BaseFixture
    {
        [Test]
        public async Task GetUserInfo()
        {
            var user = await Service.GetUserAsync();
            Assert.AreEqual("RP Tester", user.Fullname);
        }
    }
}
