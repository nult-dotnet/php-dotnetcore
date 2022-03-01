using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BooksService _bookService;
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        public BookController(BooksService booksService,CategoryService categoryService,IMapper mapper)
        {
            _bookService = booksService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<List<Book>> GetListBook()
        {
            return await this._bookService.GetAsync();
        }
        [HttpGet("detail/{id:length(24)}")]
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
        public async Task<IActionResult> CreateNewBook([FromBody] BookDTO bookDTO)
        {
            Book newBook = new Book();
            this._mapper.Map(bookDTO, newBook);
            var findBook = await this._bookService.ValidateBook(newBook.ID,newBook.BookName);
            if(findBook != null)
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
            findCategory.Quantity += 1;
            await this._categoryService.UpdateCategory(findCategory.Id, findCategory);
            CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
            newBook.Category = category;
            await this._bookService.CreateAsync(newBook);
            return CreatedAtAction(nameof(GetItemBook), new { id = newBook.ID }, newBook);
        }
        [HttpPut("detail/{id:length(24)}")]
        public async Task<IActionResult> UpdateBookItem_id(string id, [FromBody] BookDTO updateBook)
        {
            var book = await this._bookService.GetAsync(id);
            if(book is null)
            {
                ModelState.AddModelError("Error", "Book not found");
            }
            var validateBook = await this._bookService.ValidateBook(id, updateBook.Name);
            if(validateBook != null)
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
            book.Category = category;
            await this._bookService.UpdateAsync(book.ID, book);
            return CreatedAtAction(nameof(GetItemBook), new { id = book.ID}, book);
        }
        
        //Test API post and put
        [HttpPut("detail")]
        public async Task<IActionResult> UpdateBookItem_noid([FromBody] BookDTO updateBook)
        {
            var book = await this._bookService.GetAsync(updateBook.ID);
            if (book is null)
            {
                ModelState.AddModelError("Error", "Book not found");
            }
            var validateBook = await this._bookService.ValidateBook(updateBook.ID, updateBook.Name);
            if (validateBook != null)
            {
                ModelState.AddModelError("Error", "BookName is exist");
                return BadRequest(ModelState);
            }
            Category findCategory = await this._categoryService.GetCategoryById(updateBook.CategoryId);
            if (findCategory is null)
            {
                ModelState.AddModelError("Error", "Foreign key (CategoryId) does not exist");
                return BadRequest(ModelState);
            }
            if (findCategory.Id != book.CategoryId)
            {
                if (book.CategoryId != null)
                {
                    Category oldCategory = await this._categoryService.GetCategoryById(book.CategoryId);
                    oldCategory.Quantity -= 1;
                    await this._categoryService.UpdateCategory(oldCategory.Id, oldCategory);
                }
                findCategory.Quantity += 1;
                await this._categoryService.UpdateCategory(findCategory.Id, findCategory);
            }
            this._mapper.Map(updateBook, book);
            CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
            book.Category = category;
            await this._bookService.UpdateAsync(book.ID, book);
            return CreatedAtAction(nameof(GetItemBook), new { id = book.ID }, book);
        }
        [HttpPost("detailpost")]
        public async Task<IActionResult> UpdateBookItem_noid_post([FromBody] BookDTO updateBook)
        {
            var book = await this._bookService.GetAsync(updateBook.ID);
            if (book is null)
            {
                ModelState.AddModelError("Error", "Book not found");
            }
            var validateBook = await this._bookService.ValidateBook(updateBook.ID, updateBook.Name);
            if (validateBook != null)
            {
                ModelState.AddModelError("Error", "BookName is exist");
                return BadRequest(ModelState);
            }
            Category findCategory = await this._categoryService.GetCategoryById(updateBook.CategoryId);
            if (findCategory is null)
            {
                ModelState.AddModelError("Error", "Foreign key (CategoryId) does not exist");
                return BadRequest(ModelState);
            }
            if (findCategory.Id != book.CategoryId)
            {
                if (book.CategoryId != null)
                {
                    Category oldCategory = await this._categoryService.GetCategoryById(book.CategoryId);
                    oldCategory.Quantity -= 1;
                    await this._categoryService.UpdateCategory(oldCategory.Id, oldCategory);
                }
                findCategory.Quantity += 1;
                await this._categoryService.UpdateCategory(findCategory.Id, findCategory);
            }
            this._mapper.Map(updateBook, book);
            CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
            book.Category = category;
            await this._bookService.UpdateAsync(book.ID, book);
            return CreatedAtAction(nameof(GetItemBook), new { id = book.ID }, book);
        }
        [HttpPut("detailandpayload")]
        public async Task<ActionResult<Book>> UpdateBookItem_returnpayload([FromBody] BookDTO updateBook)
        {
            var book = await this._bookService.GetAsync(updateBook.ID);
            if (book is null)
            {
                ModelState.AddModelError("Error", "Book not found");
            }
            var validateBook = await this._bookService.ValidateBook(updateBook.ID, updateBook.Name);
            if (validateBook != null)
            {
                ModelState.AddModelError("Error", "BookName is exist");
                return BadRequest(ModelState);
            }
            Category findCategory = await this._categoryService.GetCategoryById(updateBook.CategoryId);
            if (findCategory is null)
            {
                ModelState.AddModelError("Error", "Foreign key (CategoryId) does not exist");
                return BadRequest(ModelState);
            }
            if (findCategory.Id != book.CategoryId)
            {
                if (book.CategoryId != null)
                {
                    Category oldCategory = await this._categoryService.GetCategoryById(book.CategoryId);
                    oldCategory.Quantity -= 1;
                    await this._categoryService.UpdateCategory(oldCategory.Id, oldCategory);
                }
                findCategory.Quantity += 1;
                await this._categoryService.UpdateCategory(findCategory.Id, findCategory);
            }
            this._mapper.Map(updateBook, book);
            CategoryShow category = this._mapper.Map<CategoryShow>(findCategory);
            book.Category = category;
            await this._bookService.UpdateAsync(book.ID, book);
            return Ok(book);
        }
        //
        [HttpDelete("{id:length(24)}")]
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
            await this._bookService.DeleteAsync(id);
            return StatusCode(200,"Delete success");
        }
        [HttpPatch("{id:length(24)}")]
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
            Book validateBook = await this._bookService.ValidateBook(findBook.ID, findBook.BookName);
            if (validateBook != null)
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
            findBook.Category = category;
            await this._bookService.UpdateAsync(findBook.ID, findBook);
            return CreatedAtAction(nameof(GetItemBook), new { id = findBook.ID }, findBook);
        }
    }
}