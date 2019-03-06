using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client.Api.Filter.Model;
using ReportPortal.Client.Api.Filter.Request;
using ReportPortal.Client.Common.Model.Filtering;
using ReportPortal.Client.Common.Model.Paging;
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

            var userFilterContainer = await Service.Filter.GetUserFiltersAsync();
            Assert.True(userFilterContainer.Collection.Any());
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
                UserFilterType = FilterType.Launch,
                Entities = new List<FilterEntity> { filterEntity },
                SelectionParameters = selectionParameters
            };

            var userFilters = await Service.Filter.AddUserFilterAsync(new AddUserFilterRequest { FilterElements = new List<FilterElement> { filterElement } });

            var userFilterContainer = await Service.Filter.GetUserFiltersAsync(new FilterOption
            {
                FilterConditions = new List<FilterCondition> { new FilterCondition(FilterOperation.Equals, "name", filterName)},
                Paging = new Page(1, 200)
            });
            Assert.Contains(userFilterContainer.Collection, f => f.Id.Equals(userFilters.First().Id));

            var deleteMessage = await Service.Filter.DeleteUserFilterAsync(userFilters.First().Id);
            Assert.Contains("success", deleteMessage.Info);
        }
    }
}
