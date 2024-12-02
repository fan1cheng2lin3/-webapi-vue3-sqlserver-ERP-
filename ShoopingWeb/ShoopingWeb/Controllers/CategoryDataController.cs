using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Services.Implmentation;
using ShoopingWeb.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryDataController : ControllerBase
    {
        private readonly ICategoryDataService _service;
        private readonly ShopDbContext _context;

        public CategoryDataController(ICategoryDataService service, ShopDbContext context)
        {
            _service = service;
            _context = context;
        }
        

        // 查询分类数据
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryData(int categoryId)
        {
            var data = await _service.GetDataByCategoryIdAsync(categoryId);
            return Ok(data);
        }

        // 新增分类数据
        [HttpPost("{categoryId}")]
        public async Task<IActionResult> AddCategoryData(int categoryId, [FromBody] CategoryDataDto dto)
        {
            await _service.AddDataAsync(categoryId, dto.FieldName, dto.FieldValue);
            return Ok();
        }


        // 修改分类数据
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryData(int id, [FromBody] CategoryDataDto dto)
        {
            var categoryData = await _context.CategoryData.FindAsync(id);
            if (categoryData == null)
            {
                return NotFound(); // 如果没有找到分类数据，返回 404
            }

            categoryData.FieldName = dto.FieldName; // 更新 FieldName
            categoryData.FieldValue = dto.FieldValue; // 更新 FieldValue

            await _context.SaveChangesAsync();
            return NoContent(); // 返回成功的响应（204 No Content）
        }


        // 删除指定分类数据
        [HttpDelete("{categoryId}/{categoryDataId}")]
        public async Task<IActionResult> DeleteCategoryData(int categoryId, int categoryDataId)
        {
            // 查找分类数据项
            var categoryData = await _context.CategoryData
                                             .FirstOrDefaultAsync(cd => cd.CategoryId == categoryId && cd.Id == categoryDataId);

            if (categoryData == null)
            {
                return NotFound(new { message = "分类数据未找到" });
            }

            // 删除分类数据项
            _context.CategoryData.Remove(categoryData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "分类数据删除成功" });
        }




        //导出
        [HttpGet("export/{categoryId}")]
        public async Task<IActionResult> ExportCategoryData(int categoryId)
        {
            // 获取数据
            var data = await _service.GetDataByCategoryIdAsync(categoryId);

            // 创建内存流
            var memoryStream = new MemoryStream();

            // 使用 StreamWriter 和 CsvWriter 写入数据
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.Default, 1024, leaveOpen: true))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(data);
            }

            // 重置流位置到开头
            memoryStream.Position = 0;

            // 设置文件名
            var fileName = $"Category_{categoryId}_Data_{DateTime.Now:yyyyMMddHHmmss}.csv";

            // 返回文件流
            return File(memoryStream, "text/csv", fileName);
        }



    }
}
