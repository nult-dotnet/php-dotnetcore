using BookStoreApi.Interfaces;
using BookStoreApi.Models;
using BookStoreApi.RepositoryPattern;
namespace BookStoreApi.Services
{
    public class BillDetailService : IBillDetailService
    {
        private readonly IUnitOfWork<BillDetail> _unitOfWork;
        public BillDetailService()
        {
            this._unitOfWork = GetUnitOfWork<BillDetail>.UnitOfWork();
        }
        public async Task AddBill(BillDetail billDetail) => await this._unitOfWork.Repository.Insert(billDetail);
    }
}