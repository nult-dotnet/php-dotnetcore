using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface ILogService
    {
        Task<List<Logs>> GetLogs();
        Task CreateLog(int logLevel, string method, string url, string? input, string? message, string? output);
        Task<List<Logs>> GetLogsByDate(DateTime date);
        Task<List<Logs>> GetLogsByLogLevel(int level);
        Task<List<Logs>> GetLogsByDate_LogLevel(int level, DateTime date);
    }
}
