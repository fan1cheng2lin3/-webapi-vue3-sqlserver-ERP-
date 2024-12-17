namespace ShoopingWeb.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task Addpurchase(string order_id, DateTime purchase_date, string staff, string payment_method, string settlement_method, string currency,
              string supplier_delivery_method_id, string Name, string Product_code, string Product_type, string supplier_name, int cout, int unit_price);

    }
}
