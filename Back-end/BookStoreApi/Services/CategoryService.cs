using MongoDB.Driver;
using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using BookStoreApi.Settings;
using BookStoreApi.Interfaces;
namespace BookStoreApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        public CategoryService(IOptions<BookStoreDatabaseSetting> bookStoreDataBaseSetting)
        {
            var clientMongo = new MongoClient(bookStoreDataBaseSetting.Value.ConnectionString);
            var DatabaseMongo = clientMongo.GetDatabase(bookStoreDataBaseSetting.Value.DatabaseName);
            this._categoryCollection = DatabaseMongo.GetCollection<Category>("Category");
        }
        public async Task<List<Category>> GetCategory() => await this._categoryCollection.Find(_ => true).SortByDescending(x => x.TimeCreate).ToListAsync();
        public virtual async Task<Category?> GetCategoryById(string id) => await this._categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateCategory(Category newCategory) => await this._categoryCollection.InsertOneAsync(newCategory);
        public virtual async Task UpdateCategory(string id,Category updateCategory) => await this._categoryCollection.ReplaceOneAsync(x=>x.Id==id,updateCategory);
        public virtual async Task DeleteCategory(string id) => await this._categoryCollection.DeleteOneAsync(x=>x.Id==id);
        public virtual async Task<Category?> ValidateCategory(string id, string name) => await this._categoryCollection.Find(x => x.Id != id && x.Name==name).FirstOrDefaultAsync();
    }
}