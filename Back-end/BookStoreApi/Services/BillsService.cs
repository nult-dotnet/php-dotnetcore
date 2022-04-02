using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.RepositoryPattern;
using BookStoreApi.Interfaces;
namespace BookStoreApi.Services
{
    public class BillsService : IBillService
    {
        private readonly IUnitOfWork<Bill> _unitOfWork;
        public BillsService()
        {
            this._unitOfWork = GetUnitOfWork<Bill>.UnitOfWork();
        }
        public async Task<IEnumerable<Bill>> GetBills() => await this._unitOfWork.Repository.Get();
        public async Task<Bill?> GetBillById(string id) => await this._unitOfWork.Repository.GetByID(id);
        public async Task CreateBill(Bill newBill) => await this._unitOfWork.Repository.Insert(newBill);
        public async Task UpdateBill(string id, Bill updateBill) => await this._unitOfWork.Repository.Update(updateBill);
    }
}