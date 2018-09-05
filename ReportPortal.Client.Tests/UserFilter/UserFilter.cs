using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Tests.Builder;
using Xunit;

namespace ReportPortal.Client.Tests.UserFilter
{
    public class UserFilterFixture : BaseFixture, IDisposable
    {
        private List<EntryCreated> _userFiltersToDelete;

        public UserFilterFixture()
        {
            _userFiltersToDelete = new List<EntryCreated>();
        }

        [Fact]
        public async Task GetFilters()
        {
            var filterElement = UserFilterBuilder.BuildFlterElement();
            var userFilters = await Service.AddUserFilterAsync(new AddUserFilterRequest { FilterElements = new List<FilterElement> { filterElement } });
            _userFiltersToDelete.AddRange(userFilters.ToList());

            var userFiltercontainer = await Service.GetUserFiltersAsync();
            Assert.True(userFiltercontainer.FilterElements.Any());
        }

        [Fact]
        public async Task AddFilter()
        {
            var filterElement = UserFilterBuilder.BuildFlterElement();

            var userFilters = await Service.AddUserFilterAsync(new AddUserFilterRequest{FilterElements = new List<FilterElement>{ filterElement } });
            _userFiltersToDelete.AddRange(userFilters);

            var userFiltercontainer = await Service.GetUserFiltersAsync();
            Assert.Contains(userFiltercontainer.FilterElements, f => f.Id.Equals(userFilters.First().Id));
        }

        [Fact]
        public async Task DeleteFilter()
        {
            var filterElement = UserFilterBuilder.BuildFlterElement();
            var userFilter = await Service.AddUserFilterAsync(new AddUserFilterRequest { FilterElements = new List<FilterElement> { filterElement } });

            var message = await Service.DeleteUserFilterAsync(userFilter.First().Id);
            Assert.Contains("success", message.Info);
            
            var allFilters = await Service.GetUserFiltersAsync();
            Assert.DoesNotContain(allFilters.FilterElements, fe => fe.Id.Equals(userFilter.First().Id));
        }

        public void Dispose()
        {
            _userFiltersToDelete.ToList().ForEach(async x => await Service.DeleteUserFilterAsync(x.Id));
        }
    }
}
