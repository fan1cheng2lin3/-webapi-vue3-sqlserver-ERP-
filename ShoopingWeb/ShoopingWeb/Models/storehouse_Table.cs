using System.ComponentModel.DataAnnotations;

namespace ShoopingWeb.Models
{
    public class storehouse_Table
    {
        [Key]
        public int ID { get; set; } // 外键关联到分类
        public string? Name { get; set; } // 字段名
        public string? storehouse_address { get; set; } // 字段值

        public string? Product_code { get; set; } // 字段值
        public string? Product_type { get; set; } // 字段值
        public int? Unit_price { get; set; } // 字段值
        public int? Count { get; set; } // 字段值



         

    }
}
