using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.Data;
using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using System.Net;
using System.Xml.Linq;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {

        private readonly ISaleServer _service;
        private readonly ShopDbContext _context;

        public SaleController(ISaleServer service, ShopDbContext context)
        {
            _service = service;
            _context = context;
        }


        // 新增采购流程数据
        [HttpPost("AddSales")]
        public async Task<IActionResult> Addpurchase([FromBody] SalesOrderDto dto)
        {
            // 1. 创建并插入主表 purchase_orders 数据
            var mainOrder = new sales_orders
            {
                sales_order_id = Convert.ToInt32(dto.sales_order_id),
                order_date = dto.order_date,
                contact_name = dto.contact_name,
                contact_phone = dto.contact_phone,
                unit_name = dto.unit_name,
                address = dto.address,
                order_status = dto.order_status,
                created_by = dto.created_by
            };

            await _context.sales_orders.AddAsync(mainOrder);
            await _context.SaveChangesAsync();

            // 2. 插入 order_products 数据
            foreach (var product in dto.order_items)
            {
                var orderProduct = new order_sales_Table
                {
                    sales_order_id = Convert.ToInt32(dto.sales_order_id), // 外键
                    name = product.name,
                    product_code = product.product_code,
                    product_type = product.product_type,
                    count = product.count,
                    unit_price = product.unit_price
                };

                await _context.order_sales_Table.AddAsync(orderProduct);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }




        //// 新增分类数据
        //[HttpPost("appsaletable")]
        //public async Task<IActionResult> AddSales([FromBody] sales_orders products)
        //{
        //    // 检查 created_at 和 order_date 是否为空，如果为空则设置为当前时间
        //    if (products.created_at == null)
        //    {
        //        products.created_at = DateTime.Now;
        //    }
        //    if (products.order_date == null)
        //    {
        //        products.order_date = DateTime.Now;
        //    }

        //    // 批量添加销售订单数据
        //    await _service.AddSales(
        //        products.sales_order_id,
        //        products.contact_name,
        //        products.contact_phone,
        //        products.unit_name,
        //        products.address,
        //        products.order_status,
        //        products.created_by,
        //        products.created_at.Value, // 直接使用 Value 属性，因为已经确保不为 null
        //        products.order_date.Value  // 直接使用 Value 属性，因为已经确保不为 null
        //    );

        //    // 提取 sales_order_id，并批量添加订单商品信息
        //    if (products.order_items != null && products.order_items.Any())
        //    {
        //        foreach (var item in products.order_items)
        //        {
        //            // 确保每个商品项的 sales_order_id 被正确设置
        //            item.sales_order_id = products.sales_order_id;

        //            var orderItem = new order_sales_Table
        //            {
        //                sales_order_id = item.sales_order_id,  // 使用外部传递的 sales_order_id
        //                name = item.name,
        //                product_code = item.product_code,
        //                product_type = item.product_type,
        //                count = item.count,
        //                unit_price = item.unit_price
        //            };
        //            await _service.AddOrderItem(orderItem);  // 假设你有一个方法来处理订单商品的保存
        //        }
        //    }

        //    return Ok("批量添加成功！");
        //}





    }
}
