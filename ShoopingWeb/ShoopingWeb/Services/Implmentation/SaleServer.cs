using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using System.Net;

namespace ShoopingWeb.Services.Implmentation
{
    public class SaleServer: ISaleServer
    {


        private readonly ShopDbContext _context;

        public SaleServer(ShopDbContext context)
        {
            _context = context;
        }




        //public async Task AddSales(string sales_order_id, string contact_name, string contact_phone, string unit_name, string address, string order_status, string created_by, DateTime created_at, DateTime order_date)
        //{
        //    var categoryData = new sales_orders
        //    {
        //        sales_order_id = sales_order_id,
        //        contact_name = contact_name,
        //        contact_phone = contact_phone,
        //        unit_name = unit_name,
        //        address = address,
        //        order_status = order_status,
        //        created_by = created_by,
        //        created_at = created_at,
        //        order_date = order_date

        //    };
        //    await _context.sales_orders.AddAsync(categoryData);
        //    await _context.SaveChangesAsync();
        //}

        public async Task AddOrderItem(order_sales_Table orderItem)
        {
            _context.order_sales_Table.Add(orderItem);  // 假设你使用 Entity Framework
            await _context.SaveChangesAsync();
        }


    }
}
