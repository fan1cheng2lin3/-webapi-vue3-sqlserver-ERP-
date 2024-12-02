using ShoopingWeb.Models;
using ShoopingWeb.ViewModels;

namespace ShoopingWeb.Services.Interfaces
{
    public interface IUsersloginService
    {
        AuthenticateResponse Autnenticate(AuthenticateRequest model);
        User GetById(int id);

        bool RegisterUser(User user, string password);

    }
}
