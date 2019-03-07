using System.Threading.Tasks;
using ReportPortal.Client.Api.User.Model;

namespace ReportPortal.Client.Api.User
{
    public interface IUserApiClient
    {
        Task<UserModel> GetUserAsync();
    }
}
