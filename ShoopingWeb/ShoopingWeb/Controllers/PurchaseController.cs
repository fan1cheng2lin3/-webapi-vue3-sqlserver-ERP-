using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;

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
        public async Task<IActionResult> Addpurchase([FromBody] purchase_orders dto)
        {
            await _service.Addpurchase(dto.order_id, dto.purchase_date, dto.staff, dto.payment_method, dto.settlement_method, dto.currency,
    dto.supplier_delivery_method_id, dto.cout,dto.unit_price);
            return Ok();
        }





    }
}
