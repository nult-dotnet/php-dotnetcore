using BookStoreApi.Models;
using BookStoreApi.Settings;
using BookStoreApi.Interfaces;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace BookStoreApi.Services
{
    public class BooksService : IBookService
    {
        private readonly IMongoCollection<Book> _bookCollection;
        public BooksService(IOptions<BookStoreDatabaseSetting> bookStoreDataBaseSetting)
        {
            var clientMongo = new MongoClient(bookStoreDataBaseSetting.Value.ConnectionString);
            var DatabaseMongo = clientMongo.GetDatabase(bookStoreDataBaseSetting.Value.DatabaseName);
            this._bookCollection = DatabaseMongo.GetCollection<Book>("Books");
        }
        public async Task<List<Book>> GetAsync() => await _bookCollection.Find(_ => true).SortByDescending(x => x.TimeCreate).ToListAsync();
        public async Task<Book?> GetAsync(string id) => await _bookCollection.Find(x => x.ID == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Book newBook) => await _bookCollection.InsertOneAsync(newBook);
        public async Task UpdateAsync(string id,Book updateBook) => await this._bookCollection.ReplaceOneAsync(x => x.ID == id, updateBook);
        public async Task DeleteAsync(string id) => await this._bookCollection.DeleteOneAsync(x => x.ID == id);
        public async Task<Book?> ValidateBook(string id, string name) => await this._bookCollection.Find(x => x.ID != id && x.BookName == name).FirstOrDefaultAsync();
        public async Task<List<Book>> ListBookByCategoryId(string id) => await this._bookCollection.Find(x => x.CategoryId == id).ToListAsync();
        public async Task DeleteListCategory(string id) => await this._bookCollection.DeleteManyAsync(x => x.CategoryId == id);
    }
}