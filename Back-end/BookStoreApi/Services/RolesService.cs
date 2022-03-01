using MongoDB.Driver;
using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using BookStoreApi.Settings;
namespace BookStoreApi.Services
{
    public class RolesService
    {
        private readonly IMongoCollection<Role> _roleCollection;
        public RolesService(IOptions<BookStoreDatabaseSetting> bookStoreDatabase)   
        {
            var clientMongo = new MongoClient(bookStoreDatabase.Value.ConnectionString);
            var DatabaseMongo = clientMongo.GetDatabase(bookStoreDatabase.Value.DatabaseName);
            this._roleCollection = DatabaseMongo.GetCollection<Role>("Roles");
        }
        public async Task<List<Role>> GetRoles() => await this._roleCollection.Find(_ => true).SortByDescending(x => x.TimeCreate).ToListAsync();
        public async Task<Role?> GetRoleById(string id) => await this._roleCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateRole(Role newRole) => await this._roleCollection.InsertOneAsync(newRole);
        public async Task UpdateRoleById(string id, Role updateRole) => await this._roleCollection.ReplaceOneAsync(x=>x.Id == id,updateRole);

        public async Task DeleteRoleById(string id) => await this._roleCollection.DeleteOneAsync(x => x.Id == id);
        public async Task<Role> ValidateRoleName(string id,string name) => await this._roleCollection.Find(x=>x.Id !=id && x.Name == name).FirstOrDefaultAsync();
    }
}