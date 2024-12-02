using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using System.Text;
using CsvHelper;
using System.Globalization;
using System.IO;

namespace ShoopingWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _repository;

        public CustomerController(ICustomerService repository)
        {
            _repository = repository;
        }

        // 查询
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _repository.GetAllAsync();
            return Ok(customers);
        }

        // 新增
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Address = dto.Address
            };
            await _repository.AddAsync(customer);
            return CreatedAtAction(nameof(GetAll), new { id = customer.Id }, customer);
        }

        // 修改
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null) return NotFound();

            customer.Name = dto.Name;
            customer.Phone = dto.Phone;
            customer.Address = dto.Address;
            await _repository.UpdateAsync(customer);
            return NoContent();
        }

        // 删除
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null) return NotFound();

            await _repository.DeleteAsync(customer);
            return NoContent();
        }

        // 导出
        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            var customers = await _repository.GetAllAsync();

            // 使用 MemoryStream 创建一个内存流
            var memoryStream = new MemoryStream();

            // 使用 ANSI 编码（Encoding.Default），即系统默认的编码格式
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.Default, 1024, leaveOpen: true))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                // 写入数据
                csvWriter.WriteRecords(customers);
            }

            // 重置 memoryStream 的位置到开始
            memoryStream.Position = 0;

            var fileName = $"Customers_{DateTime.Now:yyyyMMddHHmmss}.csv";

            // 返回文件流
            return File(memoryStream, "text/csv", fileName);
        }




    }
}
