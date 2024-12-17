namespace ShoopingWeb.DTO.Dtos
{
    public class PurchaseOrderDto
    {
        public string? OrderId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string? Staff { get; set; }
        public string? PaymentMethod { get; set; }
        public string? SettlementMethod { get; set; }
        public string? Currency { get; set; }
        public string? SupplierDeliveryMethodId { get; set; }
        public List<ProductDto>? Products { get; set; }
    }

    public class ProductDto
    {
        public string? Name { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductType { get; set; }
        public string? SupplierName { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
