﻿using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.Tests.User
{
    public class UserFixture : BaseFixture
    {
        [Fact]
        public async Task GetUserInfo()
        {
            var user = await Service.User.GetUserAsync();
            Assert.Equal("Used for Net integration check via CI", user.Fullname);
            Assert.NotEmpty(user.Email);
        }
    }
}
