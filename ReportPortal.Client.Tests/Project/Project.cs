using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client.Api.Filter.Model;
using ReportPortal.Client.Api.Filter.Request;
using ReportPortal.Client.Api.Project.Request;
using ReportPortal.Client.Common.Model.Filtering;
using Xunit;

namespace ReportPortal.Client.Tests.Project
{
    public class ProjectFixture : BaseFixture
    {
        [Fact]
        public async Task UpdatePreferences()
        {
            var filterEntity = new FilterEntity
            {
                UserFilterCondition = FilterOperation.Contains,
                FilteringField = "name",
                Value = "test value"
            };

            var order1 = new FilterOrder
            {
                Asc = true,
                SortingColumn = "name",
            };

            var selectionParameters = new FilterSelectionParameter
            {
                Orders = new List<FilterOrder> { order1 },
                PageNumber = 1
            };

            var filterElement = new FilterElement
            {
                Name = Guid.NewGuid().ToString(),
                Description = "testDscr_1",
                IsLink = false,
                Share = true,
                UserFilterType = FilterType.Launch,
                Entities = new List<FilterEntity> { filterEntity },
                SelectionParameters = selectionParameters
            };

            var userFilters = await Service.Filter.AddUserFilterAsync(new AddUserFilterRequest { FilterElements = new List<FilterElement> { filterElement } });

            var message = await Service.Project.UpdatePreferencesAsync(new UpdatePreferenceRequest { FilderIds = userFilters.Select(x => x.Id) }, Username);
            Assert.Equal(Service.ProjectName, message.ProjectRef);

            var allPreferences = await Service.Project.GetAllPreferences(Username);
            Assert.True(allPreferences.FilterIds.Intersect(userFilters.Select(x => x.Id)).Any());

            userFilters.ForEach(async x => await Service.Filter.DeleteUserFilterAsync(x.Id));
        }
    }
}
