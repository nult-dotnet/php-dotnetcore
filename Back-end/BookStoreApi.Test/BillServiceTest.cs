using System
    ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApi.Interfaces;
using BookStoreApi.Models;
using BookStoreApi.Controllers;
using Moq;
using AutoMapper;
using Xunit;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.Services;
using Microsoft.Extensions.DependencyInjection;
using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Test
{
    public class BillServiceTest
    {
        private readonly BillsService _sut;
        private readonly Mock<IRepository<Bill>> _mockBillService = new Mock<IRepository<Bill>>();
        private readonly Mock<IRepository<Book>> _mockBookService = new Mock<IRepository<Book>>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly Mock<IRepository<BillDetail>> _billDetailService = new Mock<IRepository<BillDetail>>();
        private readonly IMemoryCache _memoryCache;
        public BillServiceTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
            _sut = new BillsService(_mockIMapper.Object, _memoryCache,_mockBillService.Object,_billDetailService.Object,_mockBookService.Object);
        }
        [Fact]
        public async Task GetBillById_WhenNotFound()
        {
            //Arrange
            Bill bill = new Bill();
            _mockBillService.Setup(x=>x.GetByID(bill.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Bill> result = await this._sut.GetBillById(bill.Id);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task GetBillById_Success()
        {
            //Arrange
            Bill bill = new Bill();
            _mockBillService.Setup(x => x.GetByID(bill.Id)).ReturnsAsync(bill);
            //Act
            ApiResult<Bill> result = await this._sut.GetBillById(bill.Id);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        [Fact]
        public async Task CreateBill_WhenBookNotFound()
        {
            //Arrange
            BillDTO billDTO = new BillDTO();
            Book book = new Book();
            List<string> listBookId = new List<string> { book.Id };
            billDTO.BookId = listBookId;
            _mockBookService.Setup(x => x.GetByID(book.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Bill> result = await this._sut.AddBill(billDTO);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task CreateBill_WhenInvalidNumber()
        {
            //Arrange
            BillDTO billDTO = new BillDTO();
            int quantity = 0;
            Book book = new Book();
            List<int> listQuantity = new List<int> { quantity };
            List<string> listBookId = new List<string> { book.Id };
            billDTO.BookId = listBookId;
            billDTO.Quantity = listQuantity;
            _mockBookService.Setup(x => x.GetByID(book.Id)).ReturnsAsync(book);
            //Act
            ApiResult<Bill> result = await this._sut.AddBill(billDTO);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task CreateBill_Success()
        {
            //Arrange
            BillDTO billDTO = new BillDTO();
            Book book = new Book();
            BookInBill bookInBill = new BookInBill();
            int quantity = 1;
            int sumBill = 100;
            string bookId = Convert.ToString(ObjectId.GenerateNewId());
            List<int> listQuantity = new List<int> { quantity };
            List<string> listBookId = new List<string> { bookId };

            billDTO.BookId = listBookId;
            billDTO.Quantity = listQuantity;
            
            _mockBookService.Setup(x => x.GetByID(bookId)).ReturnsAsync(book);
            _mockIMapper.Setup(x => x.Map<BookInBill>(book)).Returns(bookInBill);

            List<BookInBill> listBook = new List<BookInBill> { bookInBill};
            Bill newBill = new Bill
            {
                Value = sumBill
            };
            //Act
            ApiResult<Bill> result = await this._sut.AddBill(billDTO);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
    }
}