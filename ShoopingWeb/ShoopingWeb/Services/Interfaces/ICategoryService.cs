using ShoopingWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoopingWeb.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddCategoryAsync(string name, string description);
        Task UpdateCategoryAsync(int id, string name, string description);
        Task DeleteCategoryAsync(int id);
    }
}
