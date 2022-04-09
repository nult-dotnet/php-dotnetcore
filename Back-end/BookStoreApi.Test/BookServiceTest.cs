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
using BookStoreApi.Services;
using BookStoreApi.DataAccess.GenericRepository;
using Microsoft.Extensions.DependencyInjection;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Test
{
    public class BookServiceTest
    {
        private readonly BooksService _sut;
        private readonly Mock<IRepository<Book>>_mockBookRepository = new Mock<IRepository<Book>>();
        private readonly Mock<IRepository<Category>> _mockCategoryRepository = new Mock<IRepository<Category>>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly IMemoryCache _memoryCache;
        public BookServiceTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
            _sut = new BooksService(_mockIMapper.Object, _memoryCache,_mockBookRepository.Object,_mockCategoryRepository.Object);
        }
        [Fact]
        public async Task GetBookById_NotFound()
        {
            //Arrange
            Book book = new Book();
           _mockBookRepository.Setup(x => x.GetByID(book.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Book> result = await this._sut.GetBookById(book.Id);
            //Assert
            Assert.Equal(false,result.IsSuccess);
        }
        [Fact]
        public async Task GeBookById_Success()
        {
            //Arrange
            Book book = new Book();
            _mockBookRepository.Setup(x => x.GetByID(book.Id)).ReturnsAsync(book);
            //Act
            ApiResult<Book> result = await this._sut.GetBookById(book.Id);
            //Assert
            Assert.Equal(true, result.IsSuccess);
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
            _mockCategoryRepository.Setup(x => x.GetByID(newBook.CategoryId)).ReturnsAsync(() => null);
            //Act
            ApiResult<Book> result = await this._sut.AddBook(bookDTO, file);
            //Assert
            Assert.Equal(false,result.IsSuccess);
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
            IFormFile file = new FormFile(stream, 0, stream.Length, "", fileName);  
            _mockIMapper.Setup(x => x.Map(bookDTO, newBook));
            _mockCategoryRepository.Setup(x => x.GetByID(newBook.CategoryId)).ReturnsAsync(category);
            //Act
            ApiResult<Book> result = await this._sut.AddBook(bookDTO, file);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        [Fact]
        public async Task DeleteBook_NotFound()
        {
            //Arrange
            Book book = new Book();
            _mockBookRepository.Setup(x => x.GetByID(book.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Book> result = await this._sut.Delete(book.Id);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task DeleteBook_Success()
        {
            //Arrange
            Book book = new Book();
            _mockBookRepository.Setup(x => x.GetByID(book.Id)).ReturnsAsync(book);
            //Act
            ApiResult<Book> result = await this._sut.Delete(book.Id);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
    }
}
