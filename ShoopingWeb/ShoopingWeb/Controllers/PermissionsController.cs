using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using ShoopingWeb.Helpers;
using ShoopingWeb.DTO.Dtos;
using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ShopDbContext _context;


        public PermissionsController(IPermissionService permissionService, ShopDbContext context)
        {
            _permissionService = permissionService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                var permissions = await _permissionService.GetAllPermissions();
                if (permissions == null || !permissions.Any())
                {
                    // 如果权限列表为空或null，可以选择返回204 NoContent或200 Ok但不带内容
                    return NoContent(); // 或者 return Ok(new List<Permission>()); 如果你希望总是返回200 Ok响应
                }
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                // 这里可以记录异常详情，例如使用日志记录库记录异常
                // log.Error(ex, "An error occurred while fetching permissions.");

                // 然后返回一个错误响应，例如500 InternalServerError
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching permissions.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto permission)
        {
            var createdPermission = await _permissionService.CreatePermission(permission);
            if (createdPermission == null) return BadRequest("无法创建权限");
            return CreatedAtAction(nameof(GetPermission), new { id = createdPermission.Id }, createdPermission);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermission(int id)
        {
            var permission = await _permissionService.GetPermissionById(id);
            if (permission == null) return NotFound();
            return Ok(permission);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionDto permission)
        {
            var updatedPermission = await _permissionService.UpdatePermission(id, permission);
            if (updatedPermission == null) return NotFound();
            return Ok(updatedPermission);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            // 查找权限
            var permission = await _context.Permission_Table.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            // 删除与权限相关的记录（例如RolePermissions表中的数据）
            var rolePermissions = _context.RolePermissions.Where(rp => rp.PermissionId == id);
            _context.RolePermissions.RemoveRange(rolePermissions);  // 删除相关的权限记录

            // 删除权限
            _context.Permission_Table.Remove(permission);
            await _context.SaveChangesAsync();  // 保存更改

            return Ok(new { message = "Permission and related roles deleted successfully" });
        }



    }
}
