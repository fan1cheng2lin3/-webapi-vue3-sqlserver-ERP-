namespace ShoopingWeb.DTO.Dtos
{
    public class CategoryDataDto
    {
        public int CategoryId { get; set; }
        public string? FieldName { get; set; }
        public string? FieldValue { get; set; }

        public string? Supplier_phone { get; set; } // 字段值
        public string? Supplier_address { get; set; } // 字段值

        public string? Product_code { get; set; } // 字段值
        public string? Product_type { get; set; } // 字段值
        public float? Unit_price { get; set; } // 字段值
        public int? Count { get; set; } // 字段值
        public float? Total_order { get; set; } // 字段值

        public string? Customer_address { get; set; } // 字段值
        public string? Customer_phone_number { get; set; } // 字段值

    }
}
