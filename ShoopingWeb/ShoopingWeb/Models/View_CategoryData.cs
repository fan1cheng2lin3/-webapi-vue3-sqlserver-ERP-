using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ShoopingWeb.Models
{
    public class View_CategoryData
    {
        [Key]
        public int CategoryId { get; set; } // 外键关联到分类
        public string? Name { get; set; } // 字段名
        public string? FieldName { get; set; } // 字段名
     
        public string? FieldValue { get; set; } // 字段值
        public string? Supplier_phone { get; set; } // 字段值
        public string? Supplier_address { get; set; } // 字段值
        
        public string? Product_code { get; set; } // 字段值
        public string? Product_type { get; set; } // 字段值
        public float? Unit_price { get; set; } // 字段值
        public int? Count { get; set; } // 字段值
        public float? Total_order { get; set; } // 字段值


        public string? Customer_address { get; set; } // 字段值
        public string? Customer_phone_number { get; set; } // 字段值

    }
}
