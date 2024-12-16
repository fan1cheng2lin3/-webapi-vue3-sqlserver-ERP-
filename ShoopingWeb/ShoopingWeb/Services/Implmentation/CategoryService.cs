using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;

namespace ShoopingWeb.Services.Implmentation
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopDbContext _context;

        public CategoryService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task AddCategoryAsync(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Category description is required.", nameof(description));
            }

            var category = new Category 
            { 
                Name = name,
                Description = description
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(int id, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Category description is required.", nameof(description));
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            category.Name = name;
            category.Description = description;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
