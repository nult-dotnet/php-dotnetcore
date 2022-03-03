 using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Services;
using BookStoreApi.Models;
using BookStoreApi.Interfaces;
using System.Linq;
using AutoMapper;
namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController:ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IBookService _booksService;
        private readonly IMapper _mapper;
        public BillController(IBillService billService,IBookService booksService,IMapper mapper)
        {
            _billService = billService;
            _booksService = booksService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<List<Bill>> GetAll() => await this._billService.GetBills();
        [HttpGet("{id:length(24)}")]
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
            List<BookInBill> listBook = new List<BookInBill>();
            foreach (var bookId in newBillDTO.BookId)
            {
                Book findBook = await this._booksService.GetAsync(bookId);
                BookInBill bookInbill = this._mapper.Map<BookInBill>(findBook);
                bookInbill.Quantity = newBillDTO.Quantity[j];
                if(findBook?.Category?.CategoryName != null) {
                    bookInbill.Category = findBook.Category.CategoryName;
                }
                findBook.Sold += newBillDTO.Quantity[j];
                await this._booksService.UpdateAsync(findBook.ID, findBook);
                sumBill += findBook.Price * newBillDTO.Quantity[j];
                listBook.Add(bookInbill);
                j++;
            }
            Bill newBill = new Bill
            {
                Books = listBook,
                Value = sumBill
            };
            await this._billService.CreateBill(newBill);
            return CreatedAtAction(nameof(GetBillById), new {id = newBill.Id},newBill);
        }
    }
}