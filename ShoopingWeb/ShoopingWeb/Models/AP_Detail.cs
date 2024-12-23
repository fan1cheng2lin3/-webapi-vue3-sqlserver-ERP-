using System.ComponentModel.DataAnnotations;

namespace ShoopingWeb.Models
{
    public class AP_Detail
    {


        [Key]
        public int? detail_id { get; set; }
        public string? receivables { get; set; }//应收
        public string? invoice_number { get; set; }//发票号码
        public decimal? amount { get; set; }//应收金额
        public DateTime? due_date { get; set; }//到期日期
        public DateTime? created_at { get; set; }//创建时间




    }
}
