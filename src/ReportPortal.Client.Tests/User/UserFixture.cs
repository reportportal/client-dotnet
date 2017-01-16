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
        public void GetUserInfo()
        {
            var user = Service.GetUser();
            Assert.AreEqual("RP Tester", user.Fullname);
        }
    }
}
