using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.Tests.User
{
    public class UserFixture : BaseFixture
    {
        [Fact]
        public async Task GetUserInfo()
        {
            var user = await Service.GetUserAsync();
            Assert.Equal("RP Tester", user.Fullname);
        }
    }
}
