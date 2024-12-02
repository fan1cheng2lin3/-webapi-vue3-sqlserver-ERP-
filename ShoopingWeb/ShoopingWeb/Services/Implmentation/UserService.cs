using Microsoft.EntityFrameworkCore;
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
    public class UserService : IUserService
    {
        private readonly ShopDbContext _context;
        private readonly AuthSettings _authSettings;


        public UserService(ShopDbContext context, IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
            _context = context;

        }
        //public AuthenticateResponse Autnenticate(AuthenticateRequest model)
        //{
        //    try
        //    {
        //        // 验证用户账号密码
        //        var user = _context.user_Table.SingleOrDefault(u => u.Email == model.Email && u.Password == model.Password);

        //        if (user == null)
        //        {
        //            return null;
        //        }

        //        // 创建令牌
        //        var token = GenerateJwtToken(user);
        //        return new AuthenticateResponse(user, token);
        //    }
        //    catch (Exception ex)
        //    {
        //        // 记录日志 (根据需要替换为实际日志工具)
        //        Console.WriteLine($"Authentication error: {ex.Message}");
        //        throw;
        //    }
        //}


        ////创建令牌
        //private string GenerateJwtToken(User user)
        //{
        //    byte[] key = Encoding.ASCII.GetBytes(_authSettings.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        //获取或者设置身份信息
        //        Subject = new ClaimsIdentity
        //        (
        //            new[]
        //            {
        //                new Claim("sub",user.Id.ToString()),
        //                new Claim("email",user.Email),
        //            }
        //        ),

        //        Expires = DateTime.UtcNow.AddDays(1),
        //        //证书签名
        //        SigningCredentials = new SigningCredentials
        //        (
        //            new SymmetricSecurityKey(key),
        //            SecurityAlgorithms.HmacSha256Signature
        //        )
        //    };

        //    //创建token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);

        //}




        //public User GetById(int id)
        //{
        //    return _context.user_Table.FirstOrDefault(u => u.Id == id);
        //}


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




        public async Task<List<User>> GetAllUsers()
        {
            return await _context.user_Table.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.user_Table.FindAsync(id);
        }

        public async Task<User> CreateUser(User user)
        {
            _context.user_Table.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(int id, User user)
        {
            var existingUser = await _context.user_Table.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.Email = user.Email;
            existingUser.Password = user.Password;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.user_Table.FindAsync(id);
            if (user == null) return false;

            _context.user_Table.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<User> AddRoleToUser(int userId, int roleId)
        {
            var user = await _context.user_Table.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            // 检查该角色是否已经分配给用户
            var existingRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (existingRole != null) return user; // 如果角色已经存在，直接返回用户

            var role = await _context.Role_Table.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null) return null;

            // 添加角色
            user.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> AddPermissionToUser(int userId, int permissionId)
        {
            var user = await _context.user_Table.Include(u => u.UserPermissions).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            // 检查该权限是否已经分配给用户
            var existingPermission = user.UserPermissions.FirstOrDefault(up => up.PermissionId == permissionId);
            if (existingPermission != null) return user; // 如果权限已经存在，直接返回用户

            var permission = await _context.Permission_Table.FirstOrDefaultAsync(p => p.Id == permissionId);
            if (permission == null) return null;

            // 添加权限
            user.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = permissionId });
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetUserWithRolesAndPermissions(int userId)
        {
            return await _context.user_Table
                                 .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                 .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
                                 .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
