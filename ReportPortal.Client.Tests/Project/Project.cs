using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Xunit;

namespace ReportPortal.Client.Tests.Project
{
    public class ProjectFixture : BaseFixture, IDisposable
    {
        private List<EntryCreated> _userFiltersToDelete;

        public ProjectFixture()
        {
            _userFiltersToDelete = new List<EntryCreated>();
        }

        [Fact]
        public async Task UpdatePreferences()
        {
            var filterEntity = new FilterEntity
            {
                UserFilterCondition = UserFilterCondition.Contains,
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
            _userFiltersToDelete.AddRange(userFilters);

            var message = await Service.UpdatePreferencesAsync(new UpdatePreferenceRequest{filderiDs = userFilters.Select(x => x.Id)}, Username);
            Assert.Contains("user have been updated", message.Info);

            var allPreferences = await Service.GetAllPreferences(Username);
            Assert.True(allPreferences.FilterIds.Intersect(userFilters.Select(x => x.Id)).Any());
        }

        public void Dispose()
        {
            _userFiltersToDelete.ToList().ForEach(async x => await Service.DeleteUserFilterAsync(x.Id));
        }
    }
}
