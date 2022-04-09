using Moq;
using BookStoreApi.Controllers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MongoDB.Bson;
using BookStoreApi.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using BookStoreApi.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.Services;
using BookStoreApi.DataAccess.GenericRepository;
using Microsoft.Extensions.DependencyInjection;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Test
{
    public class CategoryServiceTest
    {
        private readonly CategoryService _sut;
        private readonly Mock<IRepository<Category>> _mockCategoryRepository = new Mock<IRepository<Category>>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IRepository<Book>> _mockBookRepository = new Mock<IRepository<Book>>();
        private readonly Mock<ILogger<CategoryService>> _mockLogger = new Mock<ILogger<CategoryService>>();
        private readonly IMemoryCache _memoryCache;
        public CategoryServiceTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            _memoryCache = serviceProvider.GetService<IMemoryCache>();
            _sut = new CategoryService(_mockCategoryRepository.Object, _mockBookRepository.Object, _mockMapper.Object, _memoryCache, _mockLogger.Object);
        }
        //Get
        [Fact]
        public async Task GetCategoryById_ResultCategory_WhenCategoryExits()
        {
            //Arrange
            Category category = new Category();
            _mockCategoryRepository.Setup(x => x.GetByID(category.Id)).ReturnsAsync(category);
            //Act
            ApiResult<Category> objectResult = await _sut.GetCategoryById(category.Id);
            //Assert
            Assert.Equal(true, objectResult.IsSuccess);    
        }
        [Fact]
        public async Task GetCategoryById_NotFound()
        {
            //Arrange
            Category category = new Category();
            _mockCategoryRepository.Setup(x=>x.GetByID(category.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Category> objectResult = await _sut.GetCategoryById(category.Id);
            //Assert
            Assert.Equal(false, objectResult.IsSuccess);
        }
        //Post
        [Fact]
        public async Task CreateCategory_Success()
        {
            Category newCategory = new Category();
            CategoryDTO categoryDTO = new CategoryDTO();
            _mockMapper.Setup(x => x.Map(categoryDTO, newCategory));
            //Act
            ApiResult<Category> actionResult = await _sut.AddCategory(categoryDTO);
            //Assert
            Assert.Equal(true, actionResult.IsSuccess);
        }
        //Delete
        [Fact]
        public async Task DeleteCategory_WhenCategoryNotFound()
        {
            Category category = new Category();
            _mockCategoryRepository.Setup(x => x.GetByID(category.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Category> objectResult = await _sut.Delete(category.Id);
            //Assert
            Assert.Equal(false, objectResult.IsSuccess);
        }
        [Fact]
        public async Task DeleteCategory_Success()
        {
            Category category = new Category();
            _mockCategoryRepository.Setup(x => x.GetByID(category.Id)).ReturnsAsync(category);
            //Act
            ApiResult<Category> objectResult = await _sut.Delete(category.Id);
            //Assert
            Assert.Equal(true, objectResult.IsSuccess);
        }
        //Put
        [Fact]
        public async Task UpdateCategory_WhenCategoryNotFound()
        {
            Category category = new Category();
            CategoryDTO categoryDTO = new CategoryDTO();
            _mockCategoryRepository.Setup(x => x.GetByID(category.Id)).ReturnsAsync(() =>  null);
            //Act
            ApiResult<Category> objectResult = await _sut.UpdateCategory(category.Id,categoryDTO);
            //Assert
            Assert.Equal(false, objectResult.IsSuccess);
        }
        [Fact]
        public async Task UpdateCategory_Success()
        {
            Category category = new Category();
            CategoryDTO categoryDTO = new CategoryDTO();
            _mockCategoryRepository.Setup(x => x.GetByID(category.Id)).ReturnsAsync(category);
            //Act
            ApiResult<Category> objectResult = await _sut.UpdateCategory(category.Id, categoryDTO);
            //Assert
            Assert.Equal(true, objectResult.IsSuccess);
        }
    }
}