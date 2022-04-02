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

namespace BookStoreApi.Test
{
    public class BillControllerTest
    {
        private readonly BillController _sut;
        private readonly Mock<IBillService> _mockBillService = new Mock<IBillService>();
        private readonly Mock<IBookService> _mockBookService = new Mock<IBookService>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly Mock<IBillDetailService> _billDetailService = new Mock<IBillDetailService>();
        private readonly Mock<IMemoryCache> _memoryCache = new Mock<IMemoryCache>();
        public BillControllerTest()
        {
            _sut = new BillController(_mockBillService.Object, _mockBookService.Object,_mockIMapper.Object,_billDetailService.Object,_memoryCache.Object);
        }
        [Fact]
        public async Task GetBillById_WhenNotFound()
        {
            //Arrange
            string billId = Convert.ToString(ObjectId.GenerateNewId());
            _mockBillService.Setup(x=>x.GetBillById(billId)).ReturnsAsync(() => null);
            //Act
            ActionResult<Bill> result = await this._sut.GetBillById(billId);
            //Assert
            Assert.Null(result.Value);
        }
        [Fact]
        public async Task GetBillById_Success()
        {
            //Arrange
            string billId = Convert.ToString(ObjectId.GenerateNewId());
            Bill findBill = new Bill();
            _mockBillService.Setup(x => x.GetBillById(billId)).ReturnsAsync(findBill);
            //Act
            ActionResult<Bill> okObjectResult = await this._sut.GetBillById(billId);
            OkObjectResult result = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            Bill bill = Assert.IsType<Bill>(result.Value);
            //Assert
            Assert.Equal(findBill,bill);
        }
        [Fact]
        public async Task CreateBill_WhenBookNotFound()
        {
            //Arrange
            BillDTO billDTO = new BillDTO();
            string bookId = Convert.ToString(ObjectId.GenerateNewId());
            List<string> listBookId = new List<string> { bookId };
            billDTO.BookId = listBookId;
            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.CreateBill(billDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateBill_WhenInvalidNumber()
        {
            //Arrange
            BillDTO billDTO = new BillDTO();
            Book book = new Book();
            int quantity = 0;
            string bookId = Convert.ToString(ObjectId.GenerateNewId());
            List<int> listQuantity = new List<int> { quantity };
            List<string> listBookId = new List<string> { bookId };
            billDTO.BookId = listBookId;
            billDTO.Quantity = listQuantity;
            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(book);
            //Act
            IActionResult result = await this._sut.CreateBill(billDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
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
            
            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(book);
            _mockIMapper.Setup(x => x.Map<BookInBill>(book)).Returns(bookInBill);

            List<BookInBill> listBook = new List<BookInBill> { bookInBill};
            Bill newBill = new Bill
            {
                //Books = listBook,
                Value = sumBill
            };
            //Act
            IActionResult result = await this._sut.CreateBill(billDTO);
            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}