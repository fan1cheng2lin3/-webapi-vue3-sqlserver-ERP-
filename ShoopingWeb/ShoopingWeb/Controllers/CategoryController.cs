using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.DTO.Dtos;
using ShoopingWeb.Services.Implmentation;
using ShoopingWeb.Services.Interfaces;

namespace ShoopingWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ShopDbContext _context;

        public CategoryController(ICategoryService service,ShopDbContext context)
        {
            _service = service;
            _context = context;
        }

        // 获取所有分类

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // 新增分类
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto dto)
        {
            await _service.AddCategoryAsync(dto.Name, dto.Description);
            return Ok();
        }

        // 修改分类
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            await _service.UpdateCategoryAsync(id, dto.Name, dto.Description);
            return NoContent();
        }

        // 删除分类
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // 查找分类
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // 删除与分类相关的数据项
            var categoryData = _context.CategoryData.Where(cd => cd.CategoryId == id);
            _context.CategoryData.RemoveRange(categoryData);  // 删除相关的分类数据

            // 删除分类
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();  // 保存更改

            return Ok(new { message = "Category and associated data deleted successfully" });
        }




    }
}
