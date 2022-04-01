using BookStoreApi.Models;
using BookStoreApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.Interfaces;
namespace BookStoreApi.Services
{
    public class LogsService : ILogService
    {
        private readonly IMongoCollection<Logs> _logsCollection;
        public LogsService(IOptions<BookStoreDatabaseSetting> bookStoreDatabaseSetting)
        {
            var clientMongo = new MongoClient(bookStoreDatabaseSetting.Value.ConnectionString);
            var DatabaseMongo = clientMongo.GetDatabase(bookStoreDatabaseSetting.Value.DatabaseName);
            this._logsCollection = DatabaseMongo.GetCollection<Logs>("Logs");
        }
        public async Task<List<Logs>> GetLogs() => await this._logsCollection.Find(_=>true).SortByDescending(x=>x.Time).ToListAsync();
        public async Task CreateLog(int logLevel, string method, string url, string? input,string? message, string? output) {
            Logs newLog = new Logs
            {
                logLevel = logLevel,
                Method = method,
                Message = message,
                Input = input,
                Output = output,
                URL = url
            };
            await this._logsCollection.InsertOneAsync(newLog);
        } 
        public async Task<List<Logs>> GetLogsByDate(DateTime date) => await this._logsCollection.Find(x=>x.Time == date).ToListAsync();
        public async Task<List<Logs>> GetLogsByLogLevel(int level) => await this._logsCollection.Find(x => x.logLevel == level).SortByDescending(x=>x.Time).ToListAsync();
        public async Task<List<Logs>> GetLogsByDate_LogLevel (int level,DateTime date) => await this._logsCollection.Find(x=>x.logLevel==level && x.Time==date).ToListAsync();
    }
}