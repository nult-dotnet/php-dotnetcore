using BookStoreApi.ApiActionResult;
using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IBillService
    {
        Task<IEnumerable<Bill>> GetAllBill();
        Task<ApiResult<Bill>> GetBillById(string id);
        Task<ApiResult<Bill>> AddBill(BillDTO billDTO);
    }
}