using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.User
{
    public class UserFixture : BaseFixture
    {
        [Fact]
        public async Task GetUserInfo()
        {
            var user = await Service.User.GetAsync();
            // Assert.Equal("Used for Net integration check via CI", user.Fullname);
            Assert.Equal("tester", user.Fullname);
            Assert.NotEmpty(user.Email);

            Assert.NotNull(user.AssignedProjects);
            Assert.NotEmpty(user.AssignedProjects.Keys);

            // Assert.Contains("ci-agents-checks", user.AssignedProjects.Keys);
            Assert.Contains("default_personal", user.AssignedProjects.Keys);
            //Assert.Equal(ProjectRole.Member, user.AssignedProjects["ci-agents-checks"].ProjectRole);
            Assert.Equal(ProjectRole.ProjectManager, user.AssignedProjects["default_personal"].ProjectRole);
        }
    }
}
