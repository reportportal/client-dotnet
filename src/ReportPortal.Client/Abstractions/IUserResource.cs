using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface IUserResource
    {
        Task<User> GetAsync();
    }
}
