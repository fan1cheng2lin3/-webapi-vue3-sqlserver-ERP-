using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShoopingWeb.Data;
using ShoopingWeb.Helpers;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using ShoopingWeb.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoopingWeb.Services.Implmentation
{
    public class UsersloginService : IUsersloginService
    {

        private readonly ShopDbContext _context;
        private readonly AuthSettings _authSettings;


        public UsersloginService(ShopDbContext context, IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
            _context = context;

        }
        public AuthenticateResponse Autnenticate(AuthenticateRequest model)
        {
            try
            {
                // 验证用户账号密码
                var user = _context.user_Table.SingleOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user == null)
                {
                    return null;
                }

                // 创建令牌
                var token = GenerateJwtToken(user);
                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex)
            {
                // 记录日志 (根据需要替换为实际日志工具)
                Console.WriteLine($"Authentication error: {ex.Message}");
                throw;
            }
        }


        //创建令牌
        private string GenerateJwtToken(User user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //获取或者设置身份信息
                Subject = new ClaimsIdentity
                (
                    new[]
                    {
                        new Claim("sub",user.Id.ToString()),
                        new Claim("email",user.Email),
                    }
                ),

                Expires = DateTime.UtcNow.AddDays(1),
                //证书签名
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            //创建token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }




        public User GetById(int id)
        {
            return _context.user_Table.FirstOrDefault(u => u.Id == id);
        }


        public bool RegisterUser(User user, string password)
        {
            // 检查用户是否已存在
            var existingUser = _context.user_Table.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return false; // 用户已存在
            }

            // 密码加密（实际应用中需要加密）
            user.Password = password;

            _context.user_Table.Add(user);
            _context.SaveChanges();
            return true;
        }


    }
}
