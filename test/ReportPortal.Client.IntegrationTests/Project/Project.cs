using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.Project
{
    public class ProjectFixture : BaseFixture
    {
        [Fact(Skip = "Temporary ignore this test to make it possible deploy beta version")]
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
                IsShared = true,
                UserFilterType = UserFilterType.Launch,
                Conditions = new List<Condition> { condition },
                Orders = new List<FilterOrder> { order }
            };

            var userFilterCreatedReponse = await Service.UserFilter.CreateAsync(createUserFilterRequest);

            var message = await Service.Project.UpdatePreferencesAsync(Service.ProjectName, Username, userFilterCreatedReponse.Id);
            //Assert.Equal(base.Service.ProjectName, message.ProjectRef);

            var allPreferences = await Service.Project.GetAllPreferences(Service.ProjectName, Username);
            //Assert.True(allPreferences.FilterIds.Intersect(userFilters.Select(x => x.Id)).Any());

            await Service.UserFilter.DeleteAsync(userFilterCreatedReponse.Id);
        }
    }
}
