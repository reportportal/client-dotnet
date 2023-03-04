using FluentAssertions;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Responses.Project;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.Project
{
#pragma warning disable xUnit1000 // https://github.com/reportportal/reportportal/issues/1213
    class ProjectFixture : IClassFixture<BaseFixture>
#pragma warning restore xUnit1000
    {
        Service Service { get; }

        string ProjectName { get; }

        string Username { get; }

        public ProjectFixture(BaseFixture baseFixture)
        {
            Service = baseFixture.Service;
            ProjectName = baseFixture.ProjectName;
            Username = baseFixture.Username;
        }

        [Fact]
        public async Task GetProjectInfo()
        {
            var projectInfo = await Service.Project.GetAsync();

            projectInfo.Name.Should().Be(ProjectName);
            projectInfo.Id.Should().BePositive();

            projectInfo.Configuration.Should().NotBeNull();
            projectInfo.Configuration.DefectSubTypes.Should().NotBeNull();

            VerifyDefectTypesModel(projectInfo.Configuration.DefectSubTypes.ProductBugTypes);
            VerifyDefectTypesModel(projectInfo.Configuration.DefectSubTypes.AutomationBugTypes);
            VerifyDefectTypesModel(projectInfo.Configuration.DefectSubTypes.SystemIssueTypes);
            VerifyDefectTypesModel(projectInfo.Configuration.DefectSubTypes.ToInvestigateTypes);
            VerifyDefectTypesModel(projectInfo.Configuration.DefectSubTypes.NoDefectTypes);
        }

        private void VerifyDefectTypesModel(IList<ProjectDefectSubType> defectTypes)
        {
            defectTypes.Should().NotBeEmpty();
            foreach (var defectType in defectTypes)
            {
                defectType.Id.Should().BeGreaterThan(0);
                defectType.Color.Should().NotBeNullOrEmpty();
                defectType.Locator.Should().NotBeNullOrEmpty();
                defectType.LongName.Should().NotBeNullOrEmpty();
                defectType.ShortName.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public async Task GetProjectInfoByName()
        {
            var projectInfo = await Service.Project.GetAsync(ProjectName);

            projectInfo.Name.Should().Be(ProjectName);
        }

        [Fact]
        public async Task UpdatePreferences()
        {
            var condition = new Condition
            {
                UserFilterCondition = FilterOperation.Contains,
                FilteringField = "name",
                Value = "test value"
            };

            var order = new FilterOrder
            {
                Asc = true,
                SortingColumn = "name",
            };

            var createUserFilterRequest = new CreateUserFilterRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = "testDscr_1",
                IsShared = false,
                UserFilterType = UserFilterType.Launch,
                Conditions = new List<Condition> { condition },
                Orders = new List<FilterOrder> { order },
                Owner = "default"
            };

            var userFilterCreatedReponse = await Service.UserFilter.CreateAsync(createUserFilterRequest);

            var message = await Service.Project.UpdatePreferencesAsync(Service.ProjectName, Username, userFilterCreatedReponse.Id);
            message.Info.Should().Contain("successfully added");

            var allPreferences = await Service.Project.GetAllPreferencesAsync(Service.ProjectName, Username);
            allPreferences.Filters.Should().ContainSingle(p => p.Id == userFilterCreatedReponse.Id);

            var delMessage = await Service.UserFilter.DeleteAsync(userFilterCreatedReponse.Id);
            delMessage.Info.Should().Contain("successfully deleted");
        }
    }
}
