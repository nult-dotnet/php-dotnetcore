using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategory();
        Task<Category> GetCategoryById(string id);
        Task CreateCategory(Category newCategory);
        Task UpdateCategory(string id, Category updateCategory);
        Task DeleteCategory(string id);
        Task<Category> ValidateCategory(string id, string name);
    }
}