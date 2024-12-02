using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Models;

namespace ShoopingWeb.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role> GetRoleById(int id);
        Task<Role> CreateRole(RoleDto roleDto);
        Task<Role> UpdateRole(int id, RoleDto roleDto);
        Task<bool> DeleteRole(int id);

        Task<bool> AddPermissionToRole(int roleId, int permissionId);  // 给角色添加权限
        Task<Role> GetRoleWithPermissions(int roleId);  // 获取角色及其权限

    }
}
