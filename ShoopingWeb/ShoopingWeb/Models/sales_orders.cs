using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoopingWeb.Models
{
    public class sales_orders
    {

        [Key]

        public int? id { get; set; }

        public int? sales_order_id { get; set; }
        public string? contact_name { get; set; }
        public string? contact_phone { get; set; }
        public string? unit_name { get; set; }
        public string? address { get; set; }

   
        public string? order_status { get; set; }
        public string? created_by { get; set; }

        public DateTime? created_at { get; set; }


        public DateTime? order_date { get; set; }

        //public List<order_sales_Table> order_items { get; set; }  // 商品列表

    }
}
