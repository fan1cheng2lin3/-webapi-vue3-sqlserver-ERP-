namespace ShoopingWeb.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        // 多对多关系：用户与角色
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // 多对多关系：用户与权限
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    }
}


