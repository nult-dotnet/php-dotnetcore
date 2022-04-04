 using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Services;
using BookStoreApi.Models;
using BookStoreApi.Interfaces;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController:ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IBookService _booksService;
        private readonly IMapper _mapper;
        private readonly IBillDetailService _billDetailService;
        private readonly IMemoryCache _memoryCache;
        public BillController(IBillService billService,IBookService booksService,IMapper mapper,IBillDetailService billDetailService,IMemoryCache memoryCache)
        {
            _billService = billService;
            _booksService = booksService;
            _mapper = mapper;
            _billDetailService = billDetailService;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IEnumerable<Bill>> GetAll() {
            string cacheKey = "listBill";
            bool checkMemoryCacheAction = Memorycache.CheckMemoryCacheAction(this._memoryCache);
            bool checkMemoryCacheListBill = this._memoryCache.TryGetValue(cacheKey, out IEnumerable<Bill> listBill);
            if(checkMemoryCacheAction || !checkMemoryCacheListBill)
            {
                listBill = await this._billService.GetBills();
                this._memoryCache.Set(cacheKey, listBill, Memorycache.SetMemoryCache());
                Memorycache.RemoveMemoryCacheAction(this._memoryCache);
            }
            return listBill;
            
        } 
        [HttpGet("{id}")]
        public async Task<ActionResult<Bill>> GetBillById(string id)
        {
            Bill findBill = await this._billService.GetBillById(id);
            if(findBill is null)
            {
                ModelState.AddModelError("Error", "Bill not found");
                return BadRequest(ModelState);
            }
            return Ok(findBill);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] BillDTO newBillDTO)
        {
            var error = false;   
            int sumBill = 0;
            var i = -1;
            //Validate
            foreach(var item in newBillDTO.BookId)
            {
                if(item != null)
                {
                    i++;
                    Book findBook = await this._booksService.GetAsync(item);
                    if(findBook is null)
                    {
                        ModelState.AddModelError("Error", $"{item} Foreign key (BookId) does not exist");
                        error = true;
                        continue;
                    }
                    if(newBillDTO.Quantity[i] <= 0)
                    {
                        ModelState.AddModelError("Error", $"Invalid product number ({findBook.BookName} - Quantity = {newBillDTO.Quantity[i]})");
                        error = true;
                        continue;
                    }
                }
            }
            if (error)
            {
                return BadRequest(ModelState);
            }
            //Update data
            var j = 0;
            Bill newBill = new Bill();
            await this._billService.CreateBill(newBill);
            foreach (var bookId in newBillDTO.BookId)
            {
                
                Book findBook = await this._booksService.GetAsync(bookId);
                BillDetail billDetail = new BillDetail
                {
                    BillId = newBill.Id,
                    BookId = findBook.Id,
                    Quantity = newBillDTO.Quantity[j]
                };
                this._billDetailService.AddBill(billDetail);
                findBook.Sold += newBillDTO.Quantity[j];
                await this._booksService.UpdateAsync(findBook.Id, findBook);
                sumBill += findBook.Price * newBillDTO.Quantity[j];
                j++;
            }
            newBill.Value = sumBill;
            this._billService.UpdateBill(newBill.Id, newBill);
            Memorycache.SetMemoryCacheAction(this._memoryCache);
            return CreatedAtAction(nameof(GetBillById), new {id = newBill.Id},newBill);
        }
    }
}