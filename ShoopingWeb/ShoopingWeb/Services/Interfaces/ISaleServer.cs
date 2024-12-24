using ShoopingWeb.Models;

namespace ShoopingWeb.Services.Interfaces
{
    public interface ISaleServer
    {

        //Task AddSales(string sales_order_id, string Name, string contact_phone, string unit_name, string address, string order_status, string created_by, DateTime created_at, DateTime order_date);
        Task AddOrderItem(order_sales_Table orderItem);
    }
}
