using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApi.Interfaces;
using BookStoreApi.Controllers;
using BookStoreApi.Models;
using Moq;
using AutoMapper;
using Xunit;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Caching.Memory;

namespace BookStoreApi.Test
{
    public class BookControllerTest
    {
        private readonly BookController _sut;
        private readonly Mock<IBookService> _mockBookService = new Mock<IBookService>();
        private readonly Mock<ICategoryService> _mockCategoryService = new Mock<ICategoryService>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly Mock<IMemoryCache> _memoryCache = new Mock<IMemoryCache>();
        public BookControllerTest()
        {
            _sut = new BookController(_mockBookService.Object, _mockCategoryService.Object, _mockIMapper.Object,_memoryCache.Object);
        }
        [Fact]
        public async Task GetBookById_NotFound()
        {
            //Arrange
            string bookId = Convert.ToString(ObjectId.GenerateNewId());
            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(() => null);
            //Act
            ActionResult<Book> result = await this._sut.GetItemBook(bookId);
            //Assert
            Assert.Null(result.Value);
        }
        [Fact]
        public async Task GeBookById_Success()
        {
            //Arrange
            string bookId = Convert.ToString(ObjectId.GenerateNewId());
            Book book = new Book();
            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(book);
            //Act
            ActionResult<Book> result = await this._sut.GetItemBook(bookId);
            //Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Book resultBook = Assert.IsType<Book>(okObjectResult.Value);
            Assert.Equal(book,resultBook);
        }
        [Fact]
        public async Task CreateBook_WhenNameExist()
        {
            //Arrange
            BookDTO bookDTO = new BookDTO();
            Book newBook = new Book();
            IEnumerable<Book> validateBook = new List<Book>();
            //Set file upload
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            IFormFile file = new FormFile(stream, 0, stream.Length, "", fileName);

            _mockIMapper.Setup(x => x.Map(bookDTO, newBook));
            _mockBookService.Setup(x => x.ValidateBook(newBook.Id, newBook.BookName)).ReturnsAsync(validateBook);
            //Act
            IActionResult result = await this._sut.CreateNewBook(bookDTO,file);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateBook_WhenCategoryNotFound()
        {
            //Arrange
            BookDTO bookDTO = new BookDTO();
            Book newBook = new Book();
            //Set file upload
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            IFormFile file = new FormFile(stream, 0, stream.Length,"", fileName);

            _mockIMapper.Setup(x => x.Map(bookDTO, newBook));
            _mockBookService.Setup(x => x.ValidateBook(newBook.Id, newBook.BookName)).ReturnsAsync(() => null);
            _mockCategoryService.Setup(x => x.GetCategoryById(newBook.CategoryId)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.CreateNewBook(bookDTO, file);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateBook_Success()
        {
            //Arrange
            BookDTO bookDTO = new BookDTO();
            Book newBook = new Book();
            Category category = new Category();

                //Set file upload
                var fileName = "test.pdf";
                var buffer = new byte[3];
                var stream = new MemoryStream(buffer);
                IFormFile file = new FormFile(stream, 1, stream.Length, "", fileName);

            _mockIMapper.Setup(x => x.Map(bookDTO, newBook));
            _mockBookService.Setup(x => x.ValidateBook(newBook.Id, newBook.BookName)).ReturnsAsync(() => null);
            _mockCategoryService.Setup(x => x.GetCategoryById(newBook.CategoryId)).ReturnsAsync(category);

             CategoryShow categoryShow = new CategoryShow();
            _mockIMapper.Setup(x => x.Map<CategoryShow>(category)).Returns(categoryShow);
            //newBook.Category = categoryShow;
            newBook.ImagePath = fileName;
            //Act
            IActionResult result = await this._sut.CreateNewBook(bookDTO, file);
            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
        [Fact]
        public async Task DeleteBook_NotFound()
        {
            //Arrange
            string bookId = Convert.ToString(ObjectId.GenerateNewId());
            _mockBookService.Setup(x=>x.GetAsync(bookId)).ReturnsAsync(()=>null);
            //Act
            IActionResult result = await this._sut.DeleteItemBook(bookId);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteBook_Success()
        {
            //Arrange
            Book book = new Book();
            string bookId = Convert.ToString(ObjectId.GenerateNewId());
            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(book);
            //Act
            IActionResult result = await this._sut.DeleteItemBook(bookId);
            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
