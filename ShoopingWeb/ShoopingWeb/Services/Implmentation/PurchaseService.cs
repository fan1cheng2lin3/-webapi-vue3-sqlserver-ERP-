using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;

namespace ShoopingWeb.Services.Implmentation
{
    public class PurchaseService: IPurchaseService
    {
        private readonly ShopDbContext _context;

        public PurchaseService(ShopDbContext context)
        {
            _context = context;
        }




        public async Task Addpurchase(string order_id, DateTime purchase_date, string staff, string payment_method, string settlement_method, string currency,
            string supplier_delivery_method_id, int cout, int unit_price)
        {
            var categoryData = new purchase_orders
            {
                order_id = order_id,
                purchase_date = purchase_date,
                staff = staff,
                payment_method = payment_method,
                settlement_method = settlement_method,
                currency = currency,
                supplier_delivery_method_id = supplier_delivery_method_id,
                cout = cout,
                unit_price = unit_price

            };
            await _context.purchase_orders.AddAsync(categoryData);
            await _context.SaveChangesAsync();
        }



    }
}
