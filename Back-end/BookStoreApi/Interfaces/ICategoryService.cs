using BookStoreApi.ApiActionResult;
using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategory();
        Task<ApiResult<Category>> GetCategoryById(string id);
        Task<ApiResult<Category>> AddCategory(CategoryDTO categoryDTO);
        Task<ApiResult<Category>> Delete(string id);
        Task<ApiResult<Category>> UpdateCategory(string id,CategoryDTO categoryDTO);
    }
}