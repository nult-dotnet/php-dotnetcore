
using LibraryAbstractDBProvider;
using LibraryAbstractDBProvider.DBContext;
using MongoDB.Driver;

namespace BookStoreApi.DBContext
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _mongoDB;
        private readonly List<Func<Task>> _commands;
        public MongoDBContext()
        {
            var mongoClient = new MongoClient(GetStringAppsetting.ConnectString().GetSection("MongoDB:ConnectionString").Value);
            this._mongoDB = mongoClient.GetDatabase(GetStringAppsetting.ConnectString().GetSection("MongoDB:DatabaseName").Value);
            _commands = new List<Func<Task>>();
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return this._mongoDB.GetCollection<T>(name);
        }
        public int SaveChange()
        {
            var qtd = _commands.Count;
            foreach (var command in _commands)
            {
                command();
            }
            _commands.Clear();
            return qtd;
        }
    }
}