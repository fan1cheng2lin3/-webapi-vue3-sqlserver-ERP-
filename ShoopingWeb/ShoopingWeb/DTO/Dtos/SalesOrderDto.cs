using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoopingWeb.DTO.Dtos
{
    public class SalesOrderDto
    {


        public int id { get; set; }

        [Column("sales_orders_id")]
        public int? sales_order_id { get; set; }
        public string? contact_name { get; set; }
        public string? contact_phone { get; set; }
        public string? unit_name { get; set; }
        public string? address { get; set; }


        public string? order_status { get; set; }
        public string? created_by { get; set; }

        public DateTime? created_at { get; set; }


        public DateTime? order_date { get; set; }

        public List<order_sales_TableDto> order_items { get; set; }  // 商品列表


    }

    public class order_sales_TableDto
    {


        public int? sales_order_id { get; set; }

        public string? name { get; set; }
        public string? product_code { get; set; }
        public string? product_type { get; set; }
        public int? count { get; set; }
        public decimal? unit_price { get; set; }
    }

}
