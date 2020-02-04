using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.UserFilter
{
    public class UserFilterFixture : BaseFixture
    {
        [Fact]
        public async Task GetUserFilters()
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
                UserFilterType = UserFilterType.Launch,
                Entities = new List<FilterEntity> { filterEntity },
                SelectionParameters = selectionParameters
            };

            var userFilters = await Service.UserFilter.CreateAsync(new CreateUserFilterRequest { FilterElements = new List<FilterElement> { filterElement } });

            var userFilterContainer = await Service.UserFilter.GetAsync();
            Assert.True(userFilterContainer.FilterElements.Any());
        }

        [Fact]
        public async Task CreateDeleteUserFilter()
        {
            var filterName = Guid.NewGuid().ToString();

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
                Name = filterName,
                Description = "testDscr_1",
                IsLink = false,
                Share = true,
                UserFilterType = UserFilterType.Launch,
                Entities = new List<FilterEntity> { filterEntity },
                SelectionParameters = selectionParameters
            };

            var userFilters = await Service.UserFilter.CreateAsync(new CreateUserFilterRequest { FilterElements = new List<FilterElement> { filterElement } });

            var userFilterContainer = await Service.UserFilter.GetAsync(new FilterOption
            {
                Filters = new List<Filter> { new Filter(FilterOperation.Equals, "name", filterName) },
                Paging = new Paging(1, 200)
            });
            Assert.Contains(userFilterContainer.FilterElements, f => f.Id.Equals(userFilters.First().Id));

            var deleteMessage = await Service.UserFilter.DeleteAsync(userFilters.First().Id);
            Assert.Contains("success", deleteMessage.Info);
        }
    }
}
