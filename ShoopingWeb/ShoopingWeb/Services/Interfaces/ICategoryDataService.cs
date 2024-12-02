using ShoopingWeb.Models;

namespace ShoopingWeb.Services.Interfaces
{
    public interface ICategoryDataService
    {
        Task<IEnumerable<CategoryData>> GetDataByCategoryIdAsync(int categoryId);
        Task AddDataAsync(int categoryId, string fieldName, string fieldValue);
        Task UpdateDataAsync(int id, string fieldValue);
        Task DeleteDataAsync(int id);
    }
}
