namespace ShoopingWeb.Models
{
    public class Category
    {
        public int Id { get; set; } // 主键
        public string? Name { get; set; } // 分类名称
        public string? Description { get; set; } // 分类描述
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 创建时间
    }

}
