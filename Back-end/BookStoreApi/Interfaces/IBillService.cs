using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IBillService
    {
        Task<IEnumerable<Bill>> GetBills();
        Task<Bill?> GetBillById(string id);
        Task CreateBill(Bill newBill);
        Task UpdateBill(string id, Bill updateBill);
    }
}