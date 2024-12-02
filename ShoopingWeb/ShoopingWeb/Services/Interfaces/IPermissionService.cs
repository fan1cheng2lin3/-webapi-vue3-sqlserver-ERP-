using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Models;

namespace ShoopingWeb.Services.Interfaces
{
    public interface IPermissionService
    {

        Task<List<Permission>> GetAllPermissions();
        Task<Permission> GetPermissionById(int id);
        Task<Permission> CreatePermission(PermissionDto permission);
        Task<Permission> UpdatePermission(int id, PermissionDto permission);
        Task<bool> DeletePermission(int id);



    }
}
