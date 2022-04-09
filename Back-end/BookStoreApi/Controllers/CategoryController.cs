using BookStoreApi.Services;
using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BookStoreApi.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategory() {
            return await this._categoryService.GetAllCategory();
        }
        [HttpGet("{id}")]
        public async Task<ApiResult<Category>> GetCategoryById(string id)
        {
            return await this._categoryService.GetCategoryById(id);
        }
        [HttpPost]
        public async Task<ApiResult<Category>> CreateCategory([FromBody] CategoryDTO createCategory)
        {
            return await this._categoryService.AddCategory(createCategory);
        }
        [HttpDelete("{id}")]
        public async Task<ApiResult<Category>> DeleteCategory(string id)
        {
            return await this._categoryService.Delete(id);
        }
        [HttpPut("{id}")]
        public async Task<ApiResult<Category>> UpdateCategory(string id,[FromBody] CategoryDTO updateCategory)
        {
            return await this._categoryService.UpdateCategory(id, updateCategory);
        }
    }
}