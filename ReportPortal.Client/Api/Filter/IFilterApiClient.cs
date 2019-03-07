using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Api.Filter.Model;
using ReportPortal.Client.Api.Filter.Request;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Common.Model.Filtering;
using ReportPortal.Client.Common.Model.Paging;

namespace ReportPortal.Client.Api.Filter
{
    public interface IFilterApiClient
    {
        /// <summary>
        /// adds the specified user filter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<EntryCreated>> AddUserFilterAsync(AddUserFilterRequest model);

        /// <summary>
        /// gets all user filters
        /// </summary>
        /// <param name="filterOption"></param>
        /// <returns></returns>
        Task<PagingContent<FilterModel>> GetUserFiltersAsync(FilterOption filterOption = null);

        /// <summary>
        /// deletes the specified filter by id
        /// </summary>
        /// <param name="filterId"></param>
        /// <returns></returns>
        Task<Message> DeleteUserFilterAsync(string filterId);
    }
}
