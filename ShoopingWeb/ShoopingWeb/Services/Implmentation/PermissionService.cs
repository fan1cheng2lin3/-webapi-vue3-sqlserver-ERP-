using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using ShoopingWeb.DTO.Dtos;
using System.Data;

namespace ShoopingWeb.Services.Implmentation
{
    public class PermissionService : IPermissionService
    {
        private readonly ShopDbContext _context;

        public PermissionService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllPermissions()
        {
            return await _context.Permission_Table.ToListAsync();
        }

        public async Task<Permission> GetPermissionById(int id)
        {
            return await _context.Permission_Table.FindAsync(id);
        }


        public async Task<Permission> CreatePermission(PermissionDto permissionDto)
        {
            // 创建权限对象
            var permission = new Permission
            {
                Name = permissionDto.Name,
                Description = permissionDto.Description
            };

            _context.Permission_Table.Add(permission);
            await _context.SaveChangesAsync();

           
            await _context.SaveChangesAsync();

            return permission;
        }

        public async Task<Permission> UpdatePermission(int id, PermissionDto permission)
        {
            var existingPermission = await _context.Permission_Table.FindAsync(id);
            if (existingPermission == null) return null;

            existingPermission.Name = permission.Name;
            existingPermission.Description = permission.Description;

        
            await _context.SaveChangesAsync();
            return existingPermission;
        }

        public async Task<bool> DeletePermission(int id)
        {
            var permission = await _context.Permission_Table.FindAsync(id);
            if (permission == null) return false;

            _context.Permission_Table.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
