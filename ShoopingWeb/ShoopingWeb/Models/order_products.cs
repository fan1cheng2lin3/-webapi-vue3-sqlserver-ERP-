using System.ComponentModel.DataAnnotations;

namespace ShoopingWeb.Models
{
    public class order_products
    {

        [Key]
        public int id { get; set; }
        public int order_id { get; set; }
        public string? Name { get; set; } // 字段名
        public string? Product_code { get; set; } // 产品代码
        public string? Product_type { get; set; } // 产品类型
        public string? supplier_name { get; set; } // 供应商名称
        public int count { get; set; } // 数量
        public decimal unit_price { get; set; } // 单价，使用 decimal 类型
    }

}
