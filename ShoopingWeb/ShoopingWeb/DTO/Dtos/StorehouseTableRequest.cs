namespace ShoopingWeb.DTO.Dtos
{
    // DTO定义：接收包含Name和多个storehouses的请求
    public class StorehouseTableRequest
    {
        public string? Name { get; set; }
        public List<StorehouseItem>? Storehouses { get; set; }
    }

    // StorehouseItem定义：每个商品的详细信息
    public class StorehouseItem
    {
        public string? StorehouseAddress { get; set; }
        public string? ProductCode { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
        public string? ProductType { get; set; }
    }
}
