using BookStoreApi.ApiActionResult;
using BookStoreApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStoreApi.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBook();
        Task<ApiResult<Book>> GetBookById(string id);
        Task<ApiResult<Book>> AddBook(BookDTO createBook, IFormFile File);
        Task<ApiResult<Book>> Delete(string id);
        Task<ApiResult<Book>> UpdateBook(string id, BookDTO updateBook,HttpRequest Request);
        Task<ApiResult<Book>> UpdateBookPath(string id, JsonPatchDocument<BookDTO> updateBook);
    }
}