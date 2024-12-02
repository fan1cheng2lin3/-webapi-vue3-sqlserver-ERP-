using ShoopingWeb.ViewModels;
using ShoopingWeb.Models;

namespace ShoopingWeb.Services.Interfaces
{
    public interface IUserService
    {

        //AuthenticateResponse Autnenticate(AuthenticateRequest model);
        //User GetById(int id);

        bool RegisterUser(User user, string password);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(int id, User user);
        Task<bool> DeleteUser(int id);


        Task<User> AddRoleToUser(int userId, int roleId);  // 给用户添加角色
        Task<User> AddPermissionToUser(int userId, int permissionId);  // 给用户添加权限
        Task<User> GetUserWithRolesAndPermissions(int userId);  // 获取用户及其角色和权限


    }
}
