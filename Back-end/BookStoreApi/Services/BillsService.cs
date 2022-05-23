using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.RepositoryPattern;
using BookStoreApi.Interfaces;
using BookStoreApi.DataAccess.GenericRepository;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.DataAccess.UnitOfWork;
using BookStoreApi.ApiActionResult;
using BookStoreApi.MemoryCaches;

namespace BookStoreApi.Services
{
    public class BillsService : IBillService
    {
        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<BillDetail> _billDetailRepository;
        private readonly IMemoryCache _memoryCache;
        private IUnitOfWork unitOfWork = GetUnitOfWork.UnitOfWork();
        public BillsService(IMapper mapper, IMemoryCache memoryCache)
        {
            _billRepository= GetRepository<Bill>.Repository(unitOfWork);
            _bookRepository= GetRepository<Book>.Repository(unitOfWork);
            _mapper = mapper;
            _billDetailRepository = GetRepository<BillDetail>.Repository(unitOfWork);
            _memoryCache = memoryCache;
        }
        public BillsService(IMapper mapper,IMemoryCache memoryCache,IRepository<Bill> billRepository,IRepository<BillDetail> billDetailRepository,IRepository<Book> bookRepository)
        {
            _billRepository = billRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _billDetailRepository = billDetailRepository;
            _memoryCache = memoryCache;
        }
        public async Task<ApiResult<Bill>> AddBill(BillDTO billDTO)
        {
            try
            {
                unitOfWork.CreateTransaction();
                var error = false;
                int sumBill = 0;
                var i = -1;
                //Validate
                foreach (var item in billDTO.BookId)
                {
                    if (item != null)
                    {
                        i++;
                        Book findBook = await this._bookRepository.GetByID(item);
                        if (findBook is null)
                        {
                            return new ErrorResult<Bill>(400, $"{item} Foreign key (BookId) does not exist");
                        }
                        if (billDTO.Quantity[i] <= 0)
                        {
                            return new ErrorResult<Bill>(400, $"Invalid product number ({findBook.BookName} - Quantity = {billDTO.Quantity[i]})");
                        }
                    }
                }
                //Update data
                var j = 0;
                Bill newBill = new Bill();
                foreach (var bookId in billDTO.BookId)
                {

                    Book findBook = await this._bookRepository.GetByID(bookId);
                    BillDetail billDetail = new BillDetail
                    {
                        BillId = newBill.Id,
                        BookId = findBook.Id,
                        Quantity = billDTO.Quantity[j]
                    };
                    this._billDetailRepository.Update(billDetail);
                    findBook.Sold += billDTO.Quantity[j];
                    await this._bookRepository.Update(findBook);
                    sumBill += findBook.Price * billDTO.Quantity[j];
                    j++;
                }
                newBill.Value = sumBill;
                this._billRepository.Insert(newBill);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<Bill>(201, "Create success", newBill);
            }
            catch(Exception ex)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
        public async Task<IEnumerable<Bill>> GetAllBill()
        {
            string cacheKey = "listBill";
            bool checkMemoryCacheAction = Memorycache.CheckMemoryCacheAction(this._memoryCache);
            bool checkMemoryCacheListBill = this._memoryCache.TryGetValue(cacheKey, out IEnumerable<Bill> listBill);
            if (checkMemoryCacheAction || !checkMemoryCacheListBill)
            {
                listBill = await this._billRepository.Get();
                this._memoryCache.Set(cacheKey, listBill, Memorycache.SetMemoryCache());
                Memorycache.RemoveMemoryCacheAction(this._memoryCache);
            }
            return listBill;
        }

        public async Task<ApiResult<Bill>> GetBillById(string id)
        {
            Bill findBill = await this._billRepository.GetByID(id);
            if (findBill is null)
            {
                return new ErrorResult<Bill>(404, "Bill not found");
            }
            return new SuccessResult<Bill>(200, "Get success", findBill);
        }
    }
}