using System.ComponentModel.DataAnnotations;

namespace ShoopingWeb.Models
{
    public class purchase_orders
    {
        [Key]
        public string? order_id { get; set; }// 订单编号
        public DateTime purchase_date { get; set; }//采购日期
        public string? staff { get; set; }//工作人员
        public string? payment_method { get; set; }//支付方式
        public string? settlement_method { get; set; }//结算方式
        public string? currency { get; set; }// 币种
        public string? supplier_delivery_method_id { get; set; }//供应商
        public int cout { get; set; }//
        public int unit_price { get; set; }//
    }
}
