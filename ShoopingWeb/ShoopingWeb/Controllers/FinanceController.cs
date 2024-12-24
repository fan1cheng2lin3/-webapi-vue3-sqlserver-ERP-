using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {

        private readonly ShopDbContext _context;

        public FinanceController(ShopDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAP_Detail")]
        public async Task<IActionResult> GetAP_Detail()
        {
            var data = await _context.AP_Detail
                .ToListAsync();
            return Ok(data);
        }



        [HttpGet("GetAR_Detail")]
        public async Task<IActionResult> GetAR_Detail()
        {
            var data = await _context.AR_Detail
                .ToListAsync();
            return Ok(data);
        }

        [HttpPost("AP_Detail")]
        public async Task<IActionResult> Addpurchase([FromBody] AP_Detail dto)
        {
            // 使用当前时间戳生成一个基础数字
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"); // 获取到毫秒级别的时间戳
            var timestamp2 = DateTime.UtcNow; // 获取到毫秒级别的时间戳
            var random = new Random();
            var randomNumber = random.Next(10000000, 99999999); // 生成一个 8 位的随机数

            // 拼接时间戳和随机数来构造唯一的发票号
            var invoiceNumber = timestamp.Substring(8, 6) + randomNumber.ToString().Substring(0, 2);

            // 创建主订单
            var mainOrder = new AP_Detail
            {
                receivables = dto.receivables,
                invoice_number = invoiceNumber,  // 使用生成的随机发票号
                amount = dto.amount,
                due_date = dto.due_date,
                created_at = timestamp2,
            };

            await _context.AP_Detail.AddAsync(mainOrder);
            await _context.SaveChangesAsync();

            return Ok();


        }




        [HttpPost("AR_Detail")]
        public async Task<IActionResult> AddSale([FromBody] AR_Detail dto)
        {
            // 使用当前时间戳生成一个基础数字
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"); // 获取到毫秒级别的时间戳
            var timestamp2 = DateTime.UtcNow; // 获取到毫秒级别的时间戳
            var random = new Random();
            var randomNumber = random.Next(10000000, 99999999); // 生成一个 8 位的随机数

            // 拼接时间戳和随机数来构造唯一的发票号
            var invoiceNumber = timestamp.Substring(8, 6) + randomNumber.ToString().Substring(0, 2);

            // 创建主订单
            var mainOrder = new AR_Detail
            {
                receivables = dto.receivables,
                invoice_number = invoiceNumber,  // 使用生成的随机发票号
                amount = dto.amount,
                due_date = dto.due_date,
                created_at = timestamp2,
            };

            await _context.AR_Detail.AddAsync(mainOrder);
            await _context.SaveChangesAsync();

            return Ok();


        }


    }
}
