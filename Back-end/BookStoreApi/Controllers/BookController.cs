using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Interfaces;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private static void MergeChunks(string chunk1, string chunk2)
        {
            FileStream fs1 = null;
            FileStream fs2 = null;
            try
            {
                fs1 = System.IO.File.Open(chunk1, FileMode.Append);
                fs2 = System.IO.File.Open(chunk2, FileMode.Open);
                byte[] fs2Content = new byte[fs2.Length];
                fs2.Read(fs2Content, 0, (int)fs2.Length);
                fs1.Write(fs2Content, 0, (int)fs2.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                if (fs1 != null) fs1.Close();
                if (fs2 != null) fs2.Close();
                System.IO.File.Delete(chunk2);
            }
        }
        public BookController(IBookService booksService,ICategoryService categoryService,IMapper mapper,IMemoryCache memoryCache)
        {
            _bookService = booksService;
            _categoryService = categoryService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IEnumerable<Book>> GetListBook()
        {
            string cacheKey = "listBook";
            bool checkMemoryCacheAction = Memorycache.CheckMemoryCacheAction(this._memoryCache);
            bool checkMemoryCacheListBook = this._memoryCache.TryGetValue(cacheKey, out IEnumerable<Book> listBook);
            if(checkMemoryCacheAction || !checkMemoryCacheListBook)
            {
                listBook = await this._bookService.GetAsync();
                this._memoryCache.Set(cacheKey, listBook,Memorycache.SetMemoryCache());
                Memorycache.RemoveMemoryCacheAction(this._memoryCache);
            }
            return listBook;
        }
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Book>> GetItemBook(string id)
        {
            var book = await this._bookService.GetAsync(id);
            if(book is null)
            {
                ModelState.AddModelError("Error", "Book not found");
                return BadRequest(ModelState);
            }
            return Ok(book);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewBook([FromForm] BookDTO bookDTO,IFormFile File)
        {
            Book newBook = new Book();    
            this._mapper.Map(bookDTO, newBook);
            var findBook = await this._bookService.ValidateBook(newBook.Id,newBook.BookName);
            if(findBook.Count() > 0)    
            {
                ModelState.AddModelError("Error", "Name is exits");
                return BadRequest(ModelState);
            }
            Category findCategory = await this._categoryService.GetCategoryById(newBook.CategoryId);
            if(findCategory is null)
            {
                ModelState.AddModelError("Error", "Foreign key (CategoryId) does not exist");
                return BadRequest(ModelState);
            }
            //Upload image
                //var formCollection = await Request?.ReadFormAsync();
                //var image = formCollection.Files.FirstOrDefault();
                var folderName = Path.Combine("wwwroot", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Length <= 0)
                {
                    return BadRequest();
                }
                var fileName = Path.GetRandomFileName();
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var stream = System.IO.File.Create(fullPath))
                {
                    await File.CopyToAsync(stream);
                }
            findCategory.Quantity += 1;
            await this._categoryService.UpdateCategory(findCategory.Id, findCategory);
            CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
            //newBook.Category = category;
            newBook.ImagePath = fileName;
            await this._bookService.CreateAsync(newBook);
            Memorycache.SetMemoryCacheAction(this._memoryCache);
            return CreatedAtAction(nameof(GetItemBook), new { id = newBook.Id }, newBook);
        }
        [HttpGet("image/{dbPath}")]
        public IActionResult SeePicture(string dbPath)
        {
            var folderName = Path.Combine("wwwroot", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var pathImage = Path.Combine(pathToSave, dbPath).Replace("/", "\\");
            if (!System.IO.File.Exists(pathImage))
            {
                ModelState.AddModelError("Error", $"Could not find file {dbPath}");
            }
            var image = System.IO.File.OpenRead(pathImage);
            return File(image, "image/jpeg");
        }
        [HttpPut("detail/{id}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)]
        public async Task<IActionResult> UpdateBookItem_id(string id, [FromForm] BookDTO updateBook)
        {
            var book = await this._bookService.GetAsync(id);
            if(book is null)
            {
                ModelState.AddModelError("Error", "Book not found");
            }
            var validateBook = await this._bookService.ValidateBook(id, updateBook.Name);
            if(validateBook.Count() > 0)
            {
                ModelState.AddModelError("Error", "BookName is exist");
                return BadRequest(ModelState);
            }
            Category findCategory = await this._categoryService.GetCategoryById(updateBook.CategoryId);
            if(findCategory is null)
            {
                ModelState.AddModelError("Error", "Foreign key (CategoryId) does not exist");
                return BadRequest(ModelState);
            }
            //Upload file if file exist
            var formRequest = await Request.ReadFormAsync();
            var file = formRequest.Files.FirstOrDefault();
            if(file != null)
            {
                var folderName = Path.Combine("wwwroot", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileOld = Path.Combine(pathToSave, book.ImagePath).Replace("/", "\\");
                System.IO.File.Delete(fileOld);
                var fileNew = Path.GetRandomFileName();
                var pathFileNew = Path.Combine(pathToSave,fileNew);
                if(file.Length <= 0)
                {
                    return BadRequest();
                }
                using(var stream = System.IO.File.Create(pathFileNew))
                {
                    await file.CopyToAsync(stream);
                }
                book.ImagePath = fileNew;
            }
            if(findCategory.Id != book.CategoryId) {
                if(book.CategoryId != null)
                {
                    Category oldCategory = await this._categoryService.GetCategoryById(book.CategoryId);
                    oldCategory.Quantity -= 1;
                    await this._categoryService.UpdateCategory(oldCategory.Id, oldCategory);
                }
                findCategory.Quantity += 1;
                await this._categoryService.UpdateCategory(findCategory.Id, findCategory);
            }
            this._mapper.Map(updateBook,book);
            CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
            //book.Category = category;
            await this._bookService.UpdateAsync(book.Id, book);
            Memorycache.SetMemoryCacheAction(this._memoryCache);
            return CreatedAtAction(nameof(GetItemBook), new { id = book.Id}, book);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemBook(string id)
        {
            var book = await this._bookService.GetAsync(id);
            if(book is null)
            {
                ModelState.AddModelError("Error", "Book not found");
                return BadRequest(ModelState);
            }
            if(book.CategoryId != null)
            {
                Category category = await this._categoryService.GetCategoryById(book.CategoryId);
                category.Quantity -= 1;
                await this._categoryService.UpdateCategory(category.Id, category);
            }
            if(book.ImagePath != null)
            {
                var folderName = Path.Combine("wwwroot", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var pathToFile = Path.Combine(pathToSave, book.ImagePath).Replace("/", "\\");
                await this._bookService.DeleteAsync(id);
                System.IO.File.Delete(pathToFile);
            }
            Memorycache.SetMemoryCacheAction(this._memoryCache);
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatch(string id,[FromBody] JsonPatchDocument<BookDTO> updateBook)
        {
            Book findBook = await this._bookService.GetAsync(id);
            if(findBook is null)
            {
                ModelState.AddModelError("Error", "Book not found");
                return BadRequest(ModelState);
            }
            var CategoryIdOld = findBook.CategoryId;
            bool error = false;
            BookDTO bookUpdate = this._mapper.Map<BookDTO>(findBook);
            updateBook.ApplyTo(bookUpdate);
            this._mapper.Map(bookUpdate,findBook);
            Category findCategory = await this._categoryService.GetCategoryById(findBook.CategoryId);
            if(findCategory is null)
            {
                ModelState.AddModelError("Error", "Foreign key (CategoryId) does not exist");
                error = true;
            }
            var validateBook = await this._bookService.ValidateBook(findBook.Id, findBook.BookName);
            if (validateBook.Count() > 0)
            {
                ModelState.AddModelError("Error", "BookName is exist");
                error = true;
            }
            if (error)
            {
                return BadRequest(ModelState);
            }
            if(findBook.CategoryId != CategoryIdOld)
            {
                Category oldCategory = await this._categoryService.GetCategoryById(CategoryIdOld);
                oldCategory.Quantity -= 1;
                await this._categoryService.UpdateCategory(oldCategory.Id, oldCategory);
                findCategory.Quantity += 1;
                await this._categoryService.UpdateCategory(findCategory.Id, findCategory);
            }
            CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
            //findBook.Category = category;
            await this._bookService.UpdateAsync(findBook.Id, findBook);
            Memorycache.SetMemoryCacheAction(this._memoryCache);
            return CreatedAtAction(nameof(GetItemBook), new { id = findBook.Id }, findBook);
        }
    }
}