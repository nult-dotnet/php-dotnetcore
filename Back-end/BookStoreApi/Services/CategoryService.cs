using MongoDB.Driver;
using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using BookStoreApi.RepositoryPattern;
using BookStoreApi.Interfaces;
using BookStoreApi.ApiActionResult;
using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.DataAccess.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using BookStoreApi.MemoryCaches;

namespace BookStoreApi.Services
{
    public class CategoryService : ICategoryService
    {
        public readonly IRepository<Category> _categoryRepository;
        public readonly IRepository<Book> _bookRepository;
        private IUnitOfWork unitOfWork = GetUnitOfWork.UnitOfWork();
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        public CategoryService(IMapper mapper,IMemoryCache memoryCache, ILogger<CategoryService> logger)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _logger = logger;
            _categoryRepository = GetRepository<Category>.Repository(unitOfWork);
            _bookRepository = GetRepository<Book>.Repository(unitOfWork);
        }
        public CategoryService(IRepository<Category> categoryRepository,IRepository<Book> bookRepository, IMapper mapper, IMemoryCache memoryCache, ILogger<CategoryService> logger)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }
        public async Task<ApiResult<Category>> AddCategory(CategoryDTO categoryDTO)
        {
            this._logger.LogInformation(MyLogEvents.InsertItem, "{e} - Run api: https://localhost:44313/api/category", MyLogEventTitle.InsertItem);
            this._logger.LogInformation(MyLogEvents.InsertItem, "{e} - Input: {input}", MyLogEventTitle.InsertItem, MyLogEvents.ShowObject(categoryDTO));
            Category newCategory = new Category();
            this._mapper.Map(categoryDTO, newCategory);
            IEnumerable<Category> validateCategory = await this._categoryRepository.Get(x => x.Id != newCategory.Id && x.Name == newCategory.Name);
            if (validateCategory.Count() > 0)
            {
                this._logger.LogWarning(MyLogEvents.Error, "{e} - Category is exist", MyLogEventTitle.Error);
                return new ErrorResult<Category>(400, "Category is exist");
            }
            await this._categoryRepository.Insert(newCategory);
            this._logger.LogInformation(MyLogEvents.InsertItem, "{e} - Output: {output}", MyLogEventTitle.InsertItem, MyLogEvents.ShowObject(newCategory));
            //Memory Cache
            Memorycache.SetMemoryCacheAction(this._memoryCache);
            unitOfWork.Save();
            return new SuccessResult<Category>(201, "Create success", newCategory);
        }

        public async Task<ApiResult<Category>> Delete(string id)
        {
            try
            {
                unitOfWork.CreateTransaction();
                this._logger.LogInformation(MyLogEvents.DeleteItem, "{e} - Run api: https://localhost:44313/api/category/{id}", MyLogEventTitle.DeleteItem, id);
                Category findCategory = await this._categoryRepository.GetByID(id);
                if (findCategory is null)
                {
                    this._logger.LogWarning(MyLogEvents.Error, "{e} - Category not found", MyLogEventTitle.Error);
                    return new ErrorResult<Category>(404, "Category not found");
                }
                var listBook = await this._bookRepository.Get(x=>x.CategoryId == id);
                if (listBook.Count() > 0)
                {
                    foreach (Book book in listBook)
                    {
                        book.CategoryId = null;
                        //book.Category = null;
                        await this._bookRepository.Update(book);
                    }
                }
                await this._categoryRepository.Delete(id);
                //Memory Cache
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<Category>(200, "Delete success");
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            string cacheKey = "listCategory";
            bool checkcacheAction = Memorycache.CheckMemoryCacheAction(this._memoryCache);
            bool checkcacheListCategory = this._memoryCache.TryGetValue(cacheKey, out IEnumerable<Category> listCategory);
            if (checkcacheAction || !checkcacheListCategory)
            {
                this._logger.LogInformation(MyLogEvents.ListItems, "{e} - Run api: https://localhost:44313/api/category", MyLogEventTitle.ListItems);
                listCategory = await this._categoryRepository.Get();
                _memoryCache.Set(cacheKey, listCategory, Memorycache.SetMemoryCache());
                Memorycache.RemoveMemoryCacheAction(this._memoryCache);
            }
            return listCategory;
        }

        public async Task<ApiResult<Category>> GetCategoryById(string id)
        {
            this._logger.LogInformation(MyLogEvents.GetItem, "{e} - Run api: https://localhost:44313/api/category/{id}", MyLogEventTitle.GetItem, id);
            Category findCategory = await this._categoryRepository.GetByID(id);
            if (findCategory is null)
            {
                return new ErrorResult<Category>(404, "Category not found");
                
            }
            this._logger.LogInformation(MyLogEvents.GetItem, "{e} - Output: {output}", MyLogEventTitle.GetItem, MyLogEvents.ShowObject(findCategory));
            return new SuccessResult<Category>(200, "Get success", findCategory);
        }

        public async Task<ApiResult<Category>> UpdateCategory(string id, CategoryDTO categoryDTO)
        {
            try
            {
                unitOfWork.CreateTransaction();
                this._logger.LogInformation(MyLogEvents.UpdateItem, "{e} - Run api: https://localhost:44313/api/category/{id}", MyLogEventTitle.UpdateItem, id);
                this._logger.LogInformation(MyLogEvents.UpdateItem, "{e} - Input:{input}", MyLogEventTitle.UpdateItem, MyLogEvents.ShowObject(categoryDTO));
                Category findCategory = await this._categoryRepository.GetByID(id);
                if (findCategory is null)
                {
                    this._logger.LogWarning(MyLogEvents.Error, "{e} - Category not found", MyLogEventTitle.Error);

                    return new ErrorResult<Category>(404, "Category not found");
                }
                IEnumerable<Category> validateCategory = await this._categoryRepository.Get(x=>x.Id != id && x.Name == categoryDTO.Name);
                if (validateCategory.Count() > 0)
                {
                    this._logger.LogWarning(MyLogEvents.Error, "{e} - Category is exist", MyLogEventTitle.Error);

                    return new ErrorResult<Category>(400, "Category exist");
                }
                this._mapper.Map(categoryDTO, findCategory);
                CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
                var listBook = await this._bookRepository.Get(x => x.CategoryId == id);
                if (listBook.Count() > 0)
                {
                    foreach (Book book in listBook)
                    {
                        //book.Category = category;
                        await this._bookRepository.Update(book);
                    }
                }
                await this._categoryRepository.Update(findCategory);
                this._logger.LogInformation(MyLogEvents.UpdateItem, "{e} - Output: {output}", MyLogEventTitle.UpdateItem, MyLogEvents.ShowObject(findCategory));
                //Memory Cache
                Memorycache.SetMemoryCacheAction(this._memoryCache);

                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<Category>(200, "Update success", findCategory);
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
    }
}