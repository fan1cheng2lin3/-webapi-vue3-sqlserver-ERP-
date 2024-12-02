
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using ShoopingWeb.ViewModels;
using ShoopingWeb.Helpers;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;




        public UsersController(IUserService userService)
        {
            _userService = userService;
        }



        /// <summary>
        /// 获取所有用户的列表。
        /// </summary>
        /// <returns></returns>
        /// 


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedUser = await _userService.UpdateUser(id, user);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result) return NotFound();
            return Ok(new { message = "User deleted successfully" });
        }


        [HttpPost("register")]
        public ActionResult<bool> Register([FromBody] User user)
        {
            // 这里确保 User 类型能够正确反序列化
            var success = _userService.RegisterUser(user, user.Password);
            if (!success)
            {
                return BadRequest(new { message = "注册失败，用户已存在或输入无效。" });
            }
            return Ok(true);
        }




        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AddRoleToUser(int userId, int roleId)
        {
            var user = await _userService.AddRoleToUser(userId, roleId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("{userId}/permissions/{permissionId}")]
        public async Task<IActionResult> AddPermissionToUser(int userId, int permissionId)
        {
            var user = await _userService.AddPermissionToUser(userId, permissionId);
            if (user == null) return NotFound();
            return Ok(user);
        }


    }
}
