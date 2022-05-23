using BookStoreApi.Models;
using BookStoreApi.Interfaces;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using BookStoreApi.RepositoryPattern;
using BookStoreApi.ApiActionResult;
using Microsoft.AspNetCore.JsonPatch;
using BookStoreApi.DataAccess.GenericRepository;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.DataAccess.UnitOfWork;
using BookStoreApi.MemoryCaches;

namespace BookStoreApi.Services
{
    public class BooksService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private IUnitOfWork unitOfWork = GetUnitOfWork.UnitOfWork();
        public BooksService(IMapper mapper, IMemoryCache memoryCache)
        {
            _bookRepository = GetRepository<Book>.Repository(unitOfWork);
            _categoryRepository = GetRepository<Category>.Repository(unitOfWork);
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public BooksService(IMapper mapper,IMemoryCache memoryCache,IRepository<Book> bookRepository ,IRepository<Category> categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<ApiResult<Book>> AddBook(BookDTO createBook,IFormFile File)
        {
            try
            {
                unitOfWork.CreateTransaction();
                Book newBook = new Book();
                this._mapper.Map(createBook, newBook);
                var findBook = await this._bookRepository.Get(x => x.Id != newBook.Id && x.BookName == newBook.BookName);
                if (findBook.Count() > 0)
                {
                    return new ErrorResult<Book>(404, "Bookname is exist");
                }
                Category findCategory = await this._categoryRepository.GetByID(newBook.CategoryId);
                if (findCategory is null)
                {
                    return new ErrorResult<Book>(404, "Foreign key (CategoryId) does not exist");
                }
                var folderName = Path.Combine("wwwroot", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Length <= 0)
                {
                    return new ErrorResult<Book>(404, "Invalid file");
                }
                var fileName = Path.GetRandomFileName();
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var stream = System.IO.File.Create(fullPath))
                {
                    await File.CopyToAsync(stream);
                }
                findCategory.Quantity += 1;
                await this._categoryRepository.Update(findCategory);
                CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
                newBook.ImagePath = fileName;
                await this._bookRepository.Insert(newBook);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listCategory");
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<Book>(201, "Create success",newBook);
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
        public async Task<ApiResult<Book>> Delete(string id)
        {
            try
            {
                unitOfWork.CreateTransaction();
                var book = await this._bookRepository.GetByID(id);
                if (book is null)
                {
                    return new ErrorResult<Book>(404, "Book not found");
                }
                if (book.CategoryId != null)
                {
                    Category category = await this._categoryRepository.GetByID(book.CategoryId);
                    category.Quantity -= 1;
                    await this._categoryRepository.Update(category);
                }
                if (book.ImagePath != null)
                {
                    var folderName = Path.Combine("wwwroot", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var pathToFile = Path.Combine(pathToSave, book.ImagePath).Replace("/", "\\");
                    await this._bookRepository.Delete(id);
                    System.IO.File.Delete(pathToFile);
                }
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listCategory");
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<Book>(200, "Delete success");
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
        public async Task<IEnumerable<Book>> GetAllBook()
        {
            string cacheKey = "listBook";
            bool checkMemoryCacheAction = Memorycache.CheckMemoryCacheAction(this._memoryCache);
            bool checkMemoryCacheListBook = this._memoryCache.TryGetValue(cacheKey, out IEnumerable<Book> listBook);
            if (checkMemoryCacheAction || !checkMemoryCacheListBook)
            {
                listBook = await this._bookRepository.Get();
                this._memoryCache.Set(cacheKey, listBook, Memorycache.SetMemoryCache());
                Memorycache.RemoveMemoryCacheAction(this._memoryCache);
            }
            return listBook;
        }
        public async Task<ApiResult<Book>> GetBookById(string id)
        {
            var book = await this._bookRepository.GetByID(id);
            if (book is null)
            {
                return new ErrorResult<Book>(404, "Book not found");
            }
            return new SuccessResult<Book>(200,"Get success",book);
        }
        public async Task<ApiResult<Book>> UpdateBook(string id, BookDTO updateBook,HttpRequest Request)
        {
            try
            {
                unitOfWork.CreateTransaction();
                var book = await this._bookRepository.GetByID(id);
                if (book is null)
                {
                    return new ErrorResult<Book>(404, "Book not found");
                }
                var findBook = await this._bookRepository.Get(x => x.Id != id && x.BookName == updateBook.Name);
                if (findBook.Count() > 0)
                {
                    return new ErrorResult<Book>(404, "Bookname is exist");
                }
                Category findCategory = await this._categoryRepository.GetByID(updateBook.CategoryId);
                if (findCategory is null)
                {
                    return new ErrorResult<Book>(404, "Foreign key (CategoryId) does not exist");
                }
                //Upload file if file exist
                var formRequest = await Request.ReadFormAsync();
                var file = formRequest.Files.FirstOrDefault();
                if (file != null)
                {
                    var folderName = Path.Combine("wwwroot", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileOld = Path.Combine(pathToSave, book.ImagePath).Replace("/", "\\");
                    System.IO.File.Delete(fileOld);
                    var fileNew = Path.GetRandomFileName();
                    var pathFileNew = Path.Combine(pathToSave, fileNew);
                    if (file.Length <= 0)
                    {
                        return new ErrorResult<Book>(404, "Invalid file");
                    }
                    using (var stream = System.IO.File.Create(pathFileNew))
                    {
                        await file.CopyToAsync(stream);
                    }
                    book.ImagePath = fileNew;
                }
                if (findCategory.Id != book.CategoryId)
                {
                    if (book.CategoryId != null)
                    {
                        Category oldCategory = await this._categoryRepository.GetByID(book.CategoryId);
                        oldCategory.Quantity -= 1;
                        await this._categoryRepository.Update(oldCategory);
                    }
                    findCategory.Quantity += 1;
                    await this._categoryRepository.Update(findCategory);
                }
                this._mapper.Map(updateBook, book);
                CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
                //book.Category = category;
                await this._bookRepository.Update(book);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listCategory");
                return new SuccessResult<Book>(200, "Update success", book);
            }
            catch(Exception ex)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
        public async Task<ApiResult<Book>> UpdateBookPath(string id, JsonPatchDocument<BookDTO> updateBook)
        {
            try
            {
                unitOfWork.CreateTransaction();
                var book = await this._bookRepository.GetByID(id);
                if (book is null)
                {
                    return new ErrorResult<Book>(404, "Book not found");
                }
                BookDTO bookUpdate = this._mapper.Map<BookDTO>(book);
                updateBook.ApplyTo(bookUpdate);
                this._mapper.Map(bookUpdate, book);
                var findBook = await this._bookRepository.Get(x => x.Id != id && x.BookName == book.BookName);
                if (findBook.Count() > 0)
                {
                    return new ErrorResult<Book>(404, "Bookname is exist");
                }
                Category findCategory = await this._categoryRepository.GetByID(book.CategoryId);
                if (findCategory is null)
                {
                    return new ErrorResult<Book>(404, "Foreign key (CategoryId) does not exist");
                }
                if (findCategory.Id != book.CategoryId)
                {
                    if (book.CategoryId != null)
                    {
                        Category oldCategory = await this._categoryRepository.GetByID(book.CategoryId);
                        oldCategory.Quantity -= 1;
                        await this._categoryRepository.Update(oldCategory);
                    }
                    findCategory.Quantity += 1;
                    await this._categoryRepository.Update(findCategory);
                }
                await this._bookRepository.Update(book);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listCategory");
                return new SuccessResult<Book>(200, "Update success", book);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
    }
}