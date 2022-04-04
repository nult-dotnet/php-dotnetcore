
using LibraryAbstractDBProvider;
using LibraryAbstractDBProvider.DBContext;
using MongoDB.Driver;

namespace BookStoreApi.DBContext
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _mongoDB;
        public MongoDBContext()
        {
            var mongoClient = new MongoClient(GetStringAppsetting.ConnectString().GetSection("MongoDB:ConnectionString").Value);
            this._mongoDB = mongoClient.GetDatabase(GetStringAppsetting.ConnectString().GetSection("MongoDB:DatabaseName").Value);
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return this._mongoDB.GetCollection<T>(name);
        }
    }
}