using BookStoreApi.Models;
using BookStoreApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services
{
    public class LogsService
    {
        private readonly IMongoCollection<Logs> _logsCollection;
        public LogsService()
        {
            var clientMongo = new MongoClient("mongodb://localhost:27017");
            var DatabaseMongo = clientMongo.GetDatabase("BookStore");
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