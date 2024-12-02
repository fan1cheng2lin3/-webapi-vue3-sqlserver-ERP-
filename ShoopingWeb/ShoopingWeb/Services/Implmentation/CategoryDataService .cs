using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;

namespace ShoopingWeb.Services.Implmentation
{
    public class CategoryDataService : ICategoryDataService
    {
        private readonly ShopDbContext _context;

        public CategoryDataService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryData>> GetDataByCategoryIdAsync(int categoryId)
        {
            return await _context.CategoryData
                .Where(cd => cd.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task AddDataAsync(int categoryId, string fieldName, string fieldValue)
        {
            var categoryData = new CategoryData
            {
                CategoryId = categoryId,
                FieldName = fieldName,
                FieldValue = fieldValue
            };
            await _context.CategoryData.AddAsync(categoryData);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDataAsync(int id, string fieldValue)
        {
            var categoryData = await _context.CategoryData.FindAsync(id);
            if (categoryData == null) throw new Exception("Category data not found");

            categoryData.FieldValue = fieldValue;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDataAsync(int id)
        {
            var categoryData = await _context.CategoryData.FindAsync(id);
            if (categoryData == null) throw new Exception("Category data not found");

            _context.CategoryData.Remove(categoryData);
            await _context.SaveChangesAsync();
        }
    }
}
