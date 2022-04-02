using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAsync();
        Task<Book?> GetAsync(string id);
        Task CreateAsync(Book newBook);
        Task UpdateAsync(string id, Book updateBook);
        Task DeleteAsync(string id);
        Task<IEnumerable<Book>> ValidateBook(string id, string name);
        Task<IEnumerable<Book>> ListBookByCategoryId(string id);
    }
}