namespace ShoopingWeb.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task Addpurchase(string order_id, DateTime purchase_date, string staff, string payment_method, string settlement_method, string currency,
            string supplier_delivery_method_id, int cout, int unit_price);

    }
}
