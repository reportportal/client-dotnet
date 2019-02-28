﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client
{
    public partial class Service
    {
        /// <summary>
        /// adds the specified user ilter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<List<EntryCreated>> AddUserFilterAsync(AddUserFilterRequest model)
        {
            var uri = BaseUri.Append($"{Project}/filter");

            var body = ModelSerializer.Serialize<AddUserFilterRequest>(model);
            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<EntryCreated>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// gets all user filters
        /// </summary>
        /// <param name="filterOption"></param>
        /// <returns></returns>
        public virtual async Task<UserFilterContainer> GetUserFiltersAsync(FilterOption filterOption = null)
        {
            var uri = BaseUri.Append($"{Project}/filter/");
            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UserFilterContainer>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// deletes the specified filter by id
        /// </summary>
        /// <param name="filterId"></param>
        /// <returns></returns>
        public virtual async Task<Message> DeleteUserFilterAsync(string filterId)
        {
            var uri = BaseUri.Append($"{Project}/filter/{filterId}");

            var response = await _httpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
