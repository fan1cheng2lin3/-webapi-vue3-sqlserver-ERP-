namespace ShoopingWeb.Models
{
    public class warehouse_log
    {

        public int log_id { get; set; }//
        public string? document_number { get; set; }//单号，可以是出库单号或入库单号
        public string? operation_type { get; set; }// 操作类型，出库或入库
        public DateTime? operation_date { get; set; } // 操作日期，出库日期或入库日期
        public string? warehouse_name { get; set; } //仓库名称
        public string? order_id { get; set; }//订单编号
        public string? supplier_unit { get; set; } //供货单位
        public string? salesperson { get; set; }// 业务员
        public DateTime? receipt_date { get; set; }// 收货日期，仅入库时填写
        public DateTime? audit_date { get; set; }// 审核日期，可以是出库审核或入库审核
        public DateTime? delivery_date { get; set; }// 送货日期，仅出库时填写


    }
}
