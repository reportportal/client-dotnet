using ReportPortal.Client.Api.User.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client.Api.User
{
    public interface IUserApiClient
    {
        Task<UserModel> GetUserAsync();
    }
}
