 using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Services;
using BookStoreApi.Models;
using BookStoreApi.Interfaces;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController:ControllerBase
    {
        private readonly IBillService _billService;
        public BillController(IBillService billService)
        {
            _billService = billService;
        }
        [HttpGet]
        public async Task<IEnumerable<Bill>> GetAll() {
            return await this._billService.GetAllBill();
            
        } 
        [HttpGet("{id}")]
        public async Task<ApiResult<Bill>> GetBillById(string id)
        {
            return await this._billService.GetBillById(id);
        }
        [HttpPost]
        public async Task<ApiResult<Bill>> CreateBill([FromBody] BillDTO newBillDTO)
        {
            return await this._billService.AddBill(newBillDTO);
        }
    }
}