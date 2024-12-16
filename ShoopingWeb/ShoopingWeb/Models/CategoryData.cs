namespace ShoopingWeb.Models
{
    public class CategoryData
    {
        public int Id { get; set; } // 主键
        public int CategoryId { get; set; } // 外键关联到分类
        public string? FieldName { get; set; } // 字段名
        public string? FieldValue { get; set; } // 字段值
        public string? Supplier_phone { get; set; } // 字段值
        public string? Supplier_address { get; set; } // 字段值
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 创建时间

        public Category? Category { get; set; } // 导航属性
    }

}
