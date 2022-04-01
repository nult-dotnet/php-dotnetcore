using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetAsync();
        Task<Book?> GetAsync(string id);
        Task CreateAsync(Book newBook);
        Task UpdateAsync(string id, Book updateBook);
        Task DeleteAsync(string id);
        Task<Book> ValidateBook(string id, string name);
        Task<List<Book>> ListBookByCategoryId(string id);
        Task DeleteListCategory(string id);
    }
}