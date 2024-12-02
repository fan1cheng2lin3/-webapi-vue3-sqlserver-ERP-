namespace ShoopingWeb.Models
{
    public class Role
    {
        public int Id { get; set; } // 角色ID
        public string? Name { get; set; } // 角色名称
        public string? Description { get; set; } // 角色描述

     
        // 多对多关系：角色与用户
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // 多对多关系：角色与权限
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
