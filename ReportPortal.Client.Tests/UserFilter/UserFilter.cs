using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Xunit;

namespace ReportPortal.Client.Tests.UserFilter
{
    public class UserFilterFixture : BaseFixture
    {
        [Fact]
        public async Task GetUserFilters()
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

            var userFilterContainer = await Service.GetUserFiltersAsync();
            Assert.True(userFilterContainer.FilterElements.Any());
        }

        [Fact]
        public async Task CreateDeleteUserFilter()
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

            var userFilterContainer = await Service.GetUserFiltersAsync(new FilterOption
            {
                Paging = new Paging(1, 200)
            });
            Assert.Contains(userFilterContainer.FilterElements, f => f.Id.Equals(userFilters.First().Id));

            var deleteMessage = await Service.DeleteUserFilterAsync(userFilters.First().Id);
            Assert.Contains("success", deleteMessage.Info);
        }
    }
}
