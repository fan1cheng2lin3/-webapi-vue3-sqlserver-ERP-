namespace ShoopingWeb.Models
{
    public class Permission
    {
        public int Id { get; set; } // 权限ID
        public string? Name { get; set; } // 权限名称
        public string? Description { get; set; } // 权限描述

       

        // 多对多关系：权限与用户
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        // 多对多关系：权限与角色
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
