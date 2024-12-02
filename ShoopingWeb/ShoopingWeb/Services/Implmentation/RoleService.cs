using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;


namespace ShoopingWeb.Services.Implmentation
{
    public class RoleService : IRoleService
    {

        private readonly ShopDbContext _context;

        public RoleService(ShopDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Role_Table
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }
        public async Task<Role> GetRoleById(int id)
        {
            return await _context.Role_Table.FindAsync(id);
        }



        public async Task<Role> CreateRole(RoleDto roleDto)
        {
            // 创建角色对象
            var role = new Role
            {
                Name = roleDto.Name,
                Description = roleDto.Description,
                RolePermissions = roleDto.PermissionIds.Select(permissionId => new RolePermission
                {
                    PermissionId = permissionId
                }).ToList()
            };

            _context.Role_Table.Add(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<Role> UpdateRole(int id, RoleDto roleDto)
        {
            var role = await _context.Role_Table.FindAsync(id);
            if (role == null) return null;

            role.Name = roleDto.Name;
            role.Description = roleDto.Description;

            // 清除旧的权限关联
            _context.RolePermissions.RemoveRange(_context.RolePermissions.Where(rp => rp.RoleId == role.Id));

            // 添加新的权限关联
            foreach (var permissionId in roleDto.PermissionIds)
            {
                _context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
            }


            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<bool> DeleteRole(int id)
        {
            var role = await _context.Role_Table.FindAsync(id);
            if (role == null) return false;

            _context.Role_Table.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> AddPermissionToRole(int roleId, int permissionId)
        {
            // 查询是否已经有该角色和权限的组合
            var existingRolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (existingRolePermission != null)
            {
                // 如果已经存在，跳过插入
                return false; // 返回 false 表示已经存在
            }

            // 如果没有，执行插入操作
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            await _context.RolePermissions.AddAsync(rolePermission);
            await _context.SaveChangesAsync();
            return true; // 返回 true 表示成功添加
        }




        public async Task<Role> GetRoleWithPermissions(int roleId)
        {
            return await _context.Role_Table
                                 .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
                                 .FirstOrDefaultAsync(r => r.Id == roleId);
        }

    }
}
