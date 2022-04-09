using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Interfaces;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService booksService)
        {
            _bookService = booksService;
        }
        [HttpGet]
        public async Task<IEnumerable<Book>> GetListBook()
        {
            return await this._bookService.GetAllBook();
        }
        [HttpGet("detail/{id}")]
        public async Task<ApiResult<Book>> GetItemBook(string id)
        {
            return await this._bookService.GetBookById(id);
        }
        [HttpPost]
        public async Task<ApiResult<Book>> CreateNewBook([FromForm] BookDTO bookDTO,IFormFile File)
        {
            return await this._bookService.AddBook(bookDTO, File);
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
        public async Task<ApiResult<Book>> UpdateBookItem_id(string id, [FromForm] BookDTO updateBook)
        {
            return await this._bookService.UpdateBook(id, updateBook,Request);
        }
        [HttpDelete("{id}")]
        public async Task<ApiResult<Book>> DeleteItemBook(string id)
        {
            return await this._bookService.Delete(id);
        }
        [HttpPatch("{id}")]
        public async Task<ApiResult<Book>> UpdatePatch(string id,[FromBody] JsonPatchDocument<BookDTO> updateBook)
        {
            return await this._bookService.UpdateBookPath(id,updateBook);
        }
    }
}