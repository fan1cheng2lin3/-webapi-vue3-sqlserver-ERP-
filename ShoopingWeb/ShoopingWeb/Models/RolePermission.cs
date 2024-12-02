namespace ShoopingWeb.Models
{
    public class RolePermission
    {
        public int RoleId { get; set; } // 角色ID（外键）
        public Role? Role { get; set; } // 角色对象

        public int PermissionId { get; set; } // 权限ID（外键）
        public Permission? Permission { get; set; } // 权限对象

    }
}
