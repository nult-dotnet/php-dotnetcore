using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.Settings;
namespace BookStoreApi.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _userCollection;
        public UsersService(IOptions<BookStoreDatabaseSetting> bookStoreDataBaseSetting)
        {
            var mongoClient = new MongoClient(bookStoreDataBaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDataBaseSetting.Value.DatabaseName);
            this._userCollection = mongoDatabase.GetCollection<User>("Users");
        }
        public async Task<List<User>> GetUserAsync() => await this._userCollection.Find(_ => true).SortByDescending(x => x.TimeCreate).ToListAsync();
        public async Task<User?> GetUserAsync(string id) => await this._userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateUserAsync(User newUser) => await this._userCollection.InsertOneAsync(newUser);
        public async Task UpdateUserAsync(string id,User updateUser) => await this._userCollection.ReplaceOneAsync(x=>x.Id == id,updateUser);
        public async Task DeleteUserAsync(string id) => await this._userCollection.DeleteOneAsync(x=>x.Id == id);
        public async Task<User> ValidateEmailUser(string id, string email) => await this._userCollection.Find(x => x.Id != id && x.Email == email).FirstOrDefaultAsync();
        public async Task<User> ValidatePhoneUser(string id, string phone) => await this._userCollection.Find(x => x.Id != id && x.Phone == phone).FirstOrDefaultAsync();
        public async Task<List<User>> GetListUserByRoleId(string roleId) => await this._userCollection.Find(x => x.RoleId == roleId).ToListAsync();
    }
}