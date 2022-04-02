using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategory();
        Task<Category> GetCategoryById(string id);
        Task CreateCategory(Category newCategory);
        Task UpdateCategory(string id, Category updateCategory);
        Task DeleteCategory(string id);
        Task<IEnumerable<Category>> ValidateCategory(string id, string name);
    }
}