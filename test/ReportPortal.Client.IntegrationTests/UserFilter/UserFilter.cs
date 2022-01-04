﻿using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.UserFilter
{
    public class UserFilterFixture : BaseFixture
    {
        [Fact]
        public async Task CreateGetDeleteUserFilter()
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

            var userFilter = await Service.UserFilter.GetAsync(userFilterCreatedReponse.Id);

            Assert.NotEqual(0, userFilter.Id);
            Assert.Equal(createUserFilterRequest.Name, userFilter.Name);
            Assert.Equal(createUserFilterRequest.Description, userFilter.Description);
            Assert.Equal(createUserFilterRequest.IsShared, userFilter.IsShared);
            Assert.Equal(createUserFilterRequest.UserFilterType, userFilter.UserFilterType);

            var delMessage = await Service.UserFilter.DeleteAsync(userFilterCreatedReponse.Id);
            Assert.Contains("success", delMessage.Info);
        }

        [Fact]
        public async Task UpdateUserFilter()
        {
            var condition = new Condition
            {
                UserFilterCondition = FilterOperation.Contains,
                FilteringField = "name",
                Value = "test value updates"
            };

            var order = new FilterOrder
            {
                Asc = false,
                SortingColumn = "name",
            };

            var createUserFilterRequest = new CreateUserFilterRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = "testDscr_1",
                IsShared = false,
                UserFilterType = UserFilterType.Launch,
                Conditions = new List<Condition> { condition },
                Orders = new List<FilterOrder> { order }
            };

            var userFilterCreatedReponse = await Service.UserFilter.CreateAsync(createUserFilterRequest);

            var updateUserFilterRequest = new UpdateUserFilterRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = "testDscr_Updated",
                IsShared = true,
                UserFilterType = UserFilterType.TestItem,
                Conditions = new List<Condition> { condition },
                Orders = new List<FilterOrder> { order }
            };

            await Service.UserFilter.UpdateAsync(userFilterCreatedReponse.Id, updateUserFilterRequest);

            var updatedUserFilter = await Service.UserFilter.GetAsync(userFilterCreatedReponse.Id);

            Assert.Equal(updatedUserFilter.Id, userFilterCreatedReponse.Id);
            Assert.Equal(updateUserFilterRequest.Name, updatedUserFilter.Name);
            Assert.Equal(updateUserFilterRequest.Description, updatedUserFilter.Description);
            Assert.Equal(updateUserFilterRequest.IsShared, updatedUserFilter.IsShared);
            Assert.Equal(updateUserFilterRequest.UserFilterType, updatedUserFilter.UserFilterType);

            var delMessage = await Service.UserFilter.DeleteAsync(userFilterCreatedReponse.Id);
            Assert.Contains("success", delMessage.Info);
        }

        [Fact]
        public async Task FindUserFilters()
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

            var userFilterCreatedResponse = await Service.UserFilter.CreateAsync(createUserFilterRequest);

            var userFilterContainer = await Service.UserFilter.GetAsync(new FilterOption
            {
                Filters = new List<Filter> { new Filter(FilterOperation.Equals, "name", createUserFilterRequest.Name) },
                Paging = new Paging(1, 200)
            });

            Assert.Contains(userFilterContainer.Items, f => f.Id.Equals(userFilterCreatedResponse.Id));

            var deleteMessage = await Service.UserFilter.DeleteAsync(userFilterCreatedResponse.Id);
            Assert.Contains("success", deleteMessage.Info);
        }
    }
}
