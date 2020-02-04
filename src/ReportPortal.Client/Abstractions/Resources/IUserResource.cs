using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    public interface IUserResource
    {
        Task<UserResponse> GetAsync();
    }
}
