using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using ShoopingWeb.Helpers;
using Microsoft.EntityFrameworkCore;
using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Data;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ShopDbContext _context;

        public RolesController(IRoleService roleService,ShopDbContext context )
        {
            _roleService = roleService;
            _context  = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRole = await _roleService.CreateRole(roleDto);
            return CreatedAtAction(nameof(GetRoles), new { id = createdRole.Id }, createdRole);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleService.GetRoleById(id);
            if (role == null) return NotFound();
            return Ok(role);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedRole = await _roleService.UpdateRole(id, roleDto);
            if (updatedRole == null)
                return NotFound();

            return Ok(updatedRole);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRole(int id)
        //{
        //    var result = await _roleService.DeleteRole(id); // 假设你的服务层提供了异步方法 DeleteRoleAsync
        //    if (!result) return NotFound();
        //    return Ok(new { message = "Role deleted successfully" });
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            // 查找角色
            var role = await _context.Role_Table.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // 删除与角色相关的权限记录
            var rolePermissions = _context.RolePermissions.Where(rp => rp.RoleId == id);
            _context.RolePermissions.RemoveRange(rolePermissions);  // 删除相关的权限记录

            // 删除角色
            _context.Role_Table.Remove(role);
            await _context.SaveChangesAsync();  // 保存更改

            return Ok(new { message = "Role and associated permissions deleted successfully" });
        }



        [HttpPost("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> AddPermissionToRole(int roleId, int permissionId)
        {
            var role = await _roleService.AddPermissionToRole(roleId, permissionId);
            if (role == null) return NotFound();
            return Ok(role);
        }


    }
}
