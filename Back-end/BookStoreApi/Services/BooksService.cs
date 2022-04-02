using BookStoreApi.Models;
using BookStoreApi.Interfaces;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using BookStoreApi.RepositoryPattern;
namespace BookStoreApi.Services
{
    public class BooksService : IBookService
    {
        private readonly IUnitOfWork<Book> _unitOfWork;
        public BooksService()
        {
            this._unitOfWork = GetUnitOfWork<Book>.UnitOfWork();
        }
        public async Task<IEnumerable<Book>> GetAsync() => await this._unitOfWork.Repository.Get();
        public async Task<Book?> GetAsync(string id) => await this._unitOfWork.Repository.GetByID(id);
        public async Task CreateAsync(Book newBook) => await this._unitOfWork.Repository.Insert(newBook);
        public async Task UpdateAsync(string id, Book updateBook) => await this._unitOfWork.Repository.Update(updateBook);
        public async Task DeleteAsync(string id) => await this._unitOfWork.Repository.Delete(id);
        public async Task<IEnumerable<Book?>> ValidateBook(string id, string name) => await this._unitOfWork.Repository.Get(x => x.Id != id && x.BookName == name);
        public async Task<IEnumerable<Book>> ListBookByCategoryId(string id) => await this._unitOfWork.Repository.Get(x => x.CategoryId == id);

    }
}