﻿using System.Threading.Tasks;
using ReportPortal.Client.Tests.Base;
using Xunit;

namespace ReportPortal.Client.Tests.User
{
    public class UserFixture : BaseFixture
    {
        [Fact]
        public async Task GetUserInfo()
        {
            var user = await Service.GetUserAsync();
            Assert.Equal("Used for Net integration check via CI", user.Fullname);
            Assert.NotEmpty(user.Email);
        }
    }
}
