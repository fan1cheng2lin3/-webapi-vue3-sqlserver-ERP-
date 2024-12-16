﻿using Microsoft.AspNetCore.Http;
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
    public class File_ManagementController : ControllerBase
    {


        private readonly IFile_Management _service;
        private readonly ShopDbContext _context;

        public File_ManagementController(IFile_Management service, ShopDbContext context)
        {
            _service = service;
            _context = context;
        }


        // 修改
        [HttpPut("customer/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto dto)
        {
            await _service.UpdateCustomer(id, dto.Name, dto.Phone,dto.Address);
            return NoContent();
        }




        // 修改
        [HttpPut("productid/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,[FromBody] product_TableDto dto)
        {
            await _service.Updateproduct_Table(id, dto.Name, dto.Product_code, dto.Product_type, dto.supplier_name, Convert.ToInt32( dto.Unit_price), Convert.ToInt32(dto.Count));
            return NoContent();
        }




        // 修改
        [HttpPut("supplierid/{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] supplier_Table dto)
        {
            await _service.Updatesupplier_Table(id, dto.Name, dto.Supplier_phone, dto.Supplier_address);
            return NoContent();
        }




        // 修改
        [HttpPut("storehouseid/{id}")]
        public async Task<IActionResult> UpdateStorehouse(int id, [FromBody] storehouse_Table dto)
        {
            await _service.Updatestorehouse_Table(id, dto.Name, dto.storehouse_address, dto.Product_code, dto.Product_type, Convert.ToInt32(dto.Unit_price), Convert.ToInt32(dto.Count));
            return NoContent();
        }






        // 查询商品数据
        [HttpGet("product")]
        public async Task<IActionResult> GetCategoryData()
        {
            var data = await _service.GetDataByProduct_Table();
            return Ok(data);
        }


        // 查询商品数据
        [HttpGet("Customer")]
        public async Task<IActionResult> GetDataCustomer_Table()
        {
            var data = await _service.GetDataByCustomer_Table();
            return Ok(data);
        }


        // 查询商品数据
        [HttpGet("supplier")]
        public async Task<IActionResult> GetDataSupplier_Table()
        {
            var data = await _service.GetDataBySupplier_Table();
            return Ok(data);
        }

        // 查询商品数据
        [HttpGet("storehouse")]
        public async Task<IActionResult> GetDataStorehouse_Table()
        {
            var data = await _service.GetDataByStorehouse_Table();
            return Ok(data);
        }




        // 新增分类数据
        [HttpPost("appproduct")]
        public async Task<IActionResult> AddProducts([FromBody] List<product_Table> products)
        {
            if (products == null || products.Count == 0)
            {
                return BadRequest("提交的产品列表为空！");
            }

            foreach (var product in products)
            {
                // 校验单个产品数据（可以根据需要加上具体校验规则）
                if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Product_code))
                {
                    return BadRequest($"产品 {product.Name ?? "未知"} 的数据不完整！");
                }

                await _service.Addproduct(product.Name, product.Product_code, product.Product_type,
                                           product.supplier_name, Convert.ToInt32(product.Unit_price), Convert.ToInt32(product.Count));
            }

            return Ok("批量添加成功！");
        }



        // 新增分类数据
        [HttpPost("appCustomer_Table")]
        public async Task<IActionResult> AddCustomer_Table([FromBody] Customer dto)
        {
            await _service.AddCustomer_Table(dto.Name, dto.Phone, dto.Address);
            return Ok();
        }

        // 新增分类数据
        [HttpPost("appSupplier_Table")]
        public async Task<IActionResult> AddSupplier_Table([FromBody] supplier_Table dto)
        {
            await _service.AddSupplier_Table(dto.Name, dto.Supplier_phone, dto.Supplier_address);
            return Ok();
        }

        // 新增分类数据
        [HttpPost("appStorehouse_Table")]
        public async Task<IActionResult> AddStorehouse_Table([FromBody] storehouse_Table dto)
        {
            await _service.AddStorehouse_Table(dto.Name, dto.Product_code, dto.storehouse_address, dto.Product_type, Convert.ToInt32(dto.Unit_price), Convert.ToInt32(dto.Count));
            return Ok();
        }






        [HttpDelete("deleteproduct/{Id}")]
        public async Task<IActionResult> deleteproduct(int Id)
        {
            // 查找分类数据项
            var categoryData = await _context.product_Table
                                             .FirstOrDefaultAsync(cd => cd.ID == Id);

            if (categoryData == null)
            {
                return NotFound(new { message = "分类数据未找到" });
            }

            // 删除分类数据项
            _context.product_Table.Remove(categoryData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "分类数据删除成功" });
        }




        [HttpDelete("deleteCustomer_Table/{Id}")]
        public async Task<IActionResult> deleteCustomer_Table(int Id)
        {
            // 查找分类数据项
            var categoryData = await _context.Customer_Table
                                             .FirstOrDefaultAsync(cd => cd.Id == Id);

            if (categoryData == null)
            {
                return NotFound(new { message = "分类数据未找到" });
            }

            // 删除分类数据项
            _context.Customer_Table.Remove(categoryData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "分类数据删除成功" });
        }



        [HttpDelete("deleteSupplier_Table/{Id}")]
        public async Task<IActionResult> deleteSupplier_Table(int Id)
        {
            // 查找分类数据项
            var categoryData = await _context.supplier_Table
                                             .FirstOrDefaultAsync(cd => cd.ID == Id);

            if (categoryData == null)
            {
                return NotFound(new { message = "分类数据未找到" });
            }

            // 删除分类数据项
            _context.supplier_Table.Remove(categoryData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "分类数据删除成功" });
        }



        [HttpDelete("deleteStorehouse_Table/{Id}")]
        public async Task<IActionResult> deleteStorehouse_Table(int Id)
        {
            // 查找分类数据项
            var categoryData = await _context.storehouse_Table
                                             .FirstOrDefaultAsync(cd => cd.ID == Id);

            if (categoryData == null)
            {
                return NotFound(new { message = "分类数据未找到" });
            }

            // 删除分类数据项
            _context.storehouse_Table.Remove(categoryData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "分类数据删除成功" });
        }






    }
}