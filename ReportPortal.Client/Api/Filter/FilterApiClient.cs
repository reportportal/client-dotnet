using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Api.Filter.Model;
using ReportPortal.Client.Api.Filter.Request;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Common.Model.Filtering;
using ReportPortal.Client.Common.Model.Paging;
using ReportPortal.Client.Converter;
using ReportPortal.Client.Extention;

namespace ReportPortal.Client.Api.Filter
{
    public class FilterApiClient: BaseApiClient, IFilterApiClient
    {
        public FilterApiClient(HttpClient httpCLient, Uri baseUri, string project) : base(httpCLient, baseUri, project)
        {
        }

        public virtual async Task<List<EntryCreated>> AddUserFilterAsync(AddUserFilterRequest model)
        {
            var uri = BaseUri.Append($"{Project}/filter");

            var body = ModelSerializer.Serialize<AddUserFilterRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<EntryCreated>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<PagingContent<FilterModel>> GetUserFiltersAsync(FilterOption filterOption = null)
        {
            var uri = BaseUri.Append($"{Project}/filter/");
            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<PagingContent<FilterModel>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> DeleteUserFilterAsync(string filterId)
        {
            var uri = BaseUri.Append($"{Project}/filter/{filterId}");

            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
