using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.Settings;
namespace BookStoreApi.Services
{
    public class BillsService
    {
        private readonly IMongoCollection<Bill> _billCollection;
        public BillsService(IOptions<BookStoreDatabaseSetting> bookStoreDatabaseSetting)
        {
            var clientMongoDB = new MongoClient(bookStoreDatabaseSetting.Value.ConnectionString);
            var DatabaseMongo = clientMongoDB.GetDatabase(bookStoreDatabaseSetting.Value.DatabaseName);
            this._billCollection = DatabaseMongo.GetCollection<Bill>("Bills");
        }
        public async Task<List<Bill>> GetBills() => await this._billCollection.Find(_ => true).SortByDescending(x => x.TimeCreate).ToListAsync();
        public async Task<Bill?> GetBillById(string id) => await this._billCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateBill(Bill newBill) => await this._billCollection.InsertOneAsync(newBill);
        public async Task UpdateBill(string id,Bill updateBill) => await this._billCollection.ReplaceOneAsync(x=>x.Id == id,updateBill);
    }
}
