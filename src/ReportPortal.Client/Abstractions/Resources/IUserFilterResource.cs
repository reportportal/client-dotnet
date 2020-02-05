using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    public interface IUserFilterResource
    {
        Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request);

        Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption = null);

        Task<UserFilterResponse> GetAsync(long id);

        Task<MessageResponse> DeleteAsync(long id);
    }
}
