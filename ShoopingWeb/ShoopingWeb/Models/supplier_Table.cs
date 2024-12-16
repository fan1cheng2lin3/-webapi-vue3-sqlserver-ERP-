using System.ComponentModel.DataAnnotations;

namespace ShoopingWeb.Models
{
    public class supplier_Table
    {

        [Key]
        public int ID { get; set; } // 外键关联到分类
        public string? Name { get; set; } // 字段名
        public string? Supplier_phone { get; set; } // 字段值
        public string? Supplier_address { get; set; } // 字段值


    }
}
