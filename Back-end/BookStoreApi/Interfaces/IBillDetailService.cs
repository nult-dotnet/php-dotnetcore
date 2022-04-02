using BookStoreApi.Models;

namespace BookStoreApi.Interfaces
{
    public interface IBillDetailService
    {
        public Task AddBill(BillDetail billDetail);
    }
}