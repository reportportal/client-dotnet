using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Models;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface IUserFilterResource
    {
        Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request);

        Task<Content<UserFilter>> GetAsync(FilterOption filterOption = null);

        Task<UserFilter> GetAsync(long id);

        Task<Message> DeleteAsync(long id);
    }
}
