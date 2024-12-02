namespace ShoopingWeb.DTO.Dtos
{
    public class RoleDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<int> PermissionIds { get; set; }

    }

}
