using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoopingWeb.Models
{
    public class order_sales_Table
    {

        [Key]
        public int id { get; set; }

        public int sales_order_id { get; set; }  // 外键，关联销售订单


        public string? name { get; set; }
        public string? product_code { get; set; }
        public string? product_type { get; set; }
        public int? count { get; set; }
        public decimal? unit_price { get; set; }
    }


}
