using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
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
                UserFilterCondition = Filtering.FilterOperation.Contains,
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
                UserFilterType = UserFilterType.Launch,
                Entities = new List<FilterEntity> { filterEntity },
                SelectionParameters = selectionParameters
            };

            var userFilters = await Service.AddUserFilterAsync(new AddUserFilterRequest { FilterElements = new List<FilterElement> { filterElement } });

            var message = await Service.UpdatePreferencesAsync(new UpdatePreferenceRequest { FilderIds = userFilters.Select(x => x.Id) }, Username);
            Assert.Equal(base.Service.Project, message.ProjectRef);

            var allPreferences = await Service.GetAllPreferences(Username);
            Assert.True(allPreferences.FilterIds.Intersect(userFilters.Select(x => x.Id)).Any());

            userFilters.ForEach(async x => await Service.DeleteUserFilterAsync(x.Id));
        }
    }
}
