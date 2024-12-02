using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using ShoopingWeb.ViewModels;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersloginController : ControllerBase
    {
        private readonly IUsersloginService _userService;

        public UsersloginController(IUsersloginService userService)
        {
            _userService = userService;
        }


        //登陆
        [HttpPost("auth")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Autnenticate(model);
            if (response == null)
            {
                return BadRequest(new { message = "用户名或者密码不正确！" });
            }
            return response;
        }



        //注册
        [HttpPost("register")]
        public ActionResult<bool> Register(User user, string password)
        {
            var success = _userService.RegisterUser(user, password);
            if (!success)
            {
                return BadRequest(new { message = "注册失败，用户已存在或输入无效。" });
            }
            return Ok(true);
        }


    }
}
