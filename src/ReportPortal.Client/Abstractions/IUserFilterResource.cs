using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface IUserFilterResource
    {
        Task<List<UserFilterCreatedResponse>> AddAsync(AddUserFilterRequest request);

        Task<UserFilterContainer> GetAsync(FilterOption filterOption = null);

        Task<Message> DeleteAsync(string filterId);
    }
}
