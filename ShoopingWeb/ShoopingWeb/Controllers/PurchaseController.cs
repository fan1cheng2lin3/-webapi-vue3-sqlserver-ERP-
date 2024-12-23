using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using ShoopingWeb.DTO.Dtos;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _service;
        private readonly ShopDbContext _context;

        public PurchaseController(IPurchaseService service, ShopDbContext context)
        {
            _service = service;
            _context = context;
        }



        // 新增采购流程数据
        [HttpPost("Addpurchase")]
        public async Task<IActionResult> Addpurchase([FromBody] PurchaseOrderDto dto)
        {
            // 1. 创建并插入主表 purchase_orders 数据
            var mainOrder = new purchase_orders
            {
                order_id = Convert.ToInt32(dto.OrderId),
                purchase_date = dto.PurchaseDate,
                staff = dto.Staff,
                payment_method = dto.PaymentMethod,
                settlement_method = dto.SettlementMethod,
                currency = dto.Currency,
                supplier_delivery_method_id = dto.SupplierDeliveryMethodId
            };

            await _context.purchase_orders.AddAsync(mainOrder);
            await _context.SaveChangesAsync();

            // 2. 插入 order_products 数据
            foreach (var product in dto.Products)
            {
                var orderProduct = new order_products
                {
                    order_id = Convert.ToInt32(dto.OrderId), // 外键
                    Name = product.Name,
                    Product_code = product.ProductCode,
                    Product_type = product.ProductType,
                    supplier_name = product.SupplierName,
                    count = product.Count,
                    unit_price = product.UnitPrice
                };

                await _context.order_products.AddAsync(orderProduct);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("Updatepurchase/{orderId}")]
        public async Task<IActionResult> Updatepurchase(int orderId, [FromBody] PurchaseOrderDto dto)
        {
            // 1. 查找并更新主表 purchase_orders 数据
            var mainOrder = await _context.purchase_orders.FindAsync(orderId);
            if (mainOrder == null)
            {
                return NotFound("订单未找到");
            }

            // 更新主订单信息
            mainOrder.purchase_date = dto.PurchaseDate;
            mainOrder.staff = dto.Staff;
            mainOrder.payment_method = dto.PaymentMethod;
            mainOrder.settlement_method = dto.SettlementMethod;
            mainOrder.currency = dto.Currency;
            mainOrder.supplier_delivery_method_id = dto.SupplierDeliveryMethodId;

            // 更新主表数据
            _context.purchase_orders.Update(mainOrder);

            // 2. 更新 order_products 数据
            // 删除原有的产品信息
            var existingProducts = _context.order_products.Where(p => p.order_id == orderId).ToList();
            _context.order_products.RemoveRange(existingProducts);

            // 插入更新后的产品信息
            foreach (var product in dto.Products)
            {
                var orderProduct = new order_products
                {
                    order_id = orderId,
                    Name = product.Name,
                    Product_code = product.ProductCode,
                    Product_type = product.ProductType,
                    supplier_name = product.SupplierName,
                    count = product.Count,
                    unit_price = product.UnitPrice
                };
                await _context.order_products.AddAsync(orderProduct);
            }

            await _context.SaveChangesAsync();
            return Ok("订单更新成功");
        }







    }
}
