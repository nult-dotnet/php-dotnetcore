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

namespace BookStoreApi.Test
{
    public class CategoryControllerTest
    {
        private readonly CategoryController _testController;
        private readonly Mock<ICategoryService> _mockCategoryService = new Mock<ICategoryService>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IBookService> _mockBookService = new Mock<IBookService>();
        private readonly Mock<ILogger<CategoryController>> _mockLogger = new Mock<ILogger<CategoryController>>();
        private readonly Mock<IMemoryCache> _memoryCache = new Mock<IMemoryCache>();
        public CategoryControllerTest()
        {
            _testController = new CategoryController(_mockCategoryService.Object, _mockBookService.Object, _mockMapper.Object, _mockLogger.Object,_memoryCache.Object);
        }
        //Get
        [Fact]
        public async Task GetCategoryById_ResultCategory_WhenCategoryExits()
        {
            //Arrange
            string categoryID = Convert.ToString(ObjectId.GenerateNewId());
            Category resultCategory = new Category
            {
                Id = categoryID,
                Name = "Khoa học công nghệ"
            };
            _mockCategoryService.Setup(x => x.GetCategoryById(categoryID)).ReturnsAsync(resultCategory);
            //Act
            ActionResult<Category> objectResult = await _testController.GetCategoryById(categoryID);
            OkObjectResult findCategory = Assert.IsType<OkObjectResult>(objectResult.Result);
            Category category = Assert.IsType<Category>(findCategory.Value);
            //Assert
            Assert.Equal(resultCategory, category);    
        }
        [Fact]
        public async Task GetCategoryById_NotFound()
        {
            //Arrange
            string categoryID = Convert.ToString(ObjectId.GenerateNewId());
            _mockCategoryService.Setup(x=>x.GetCategoryById(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            ActionResult<Category> category = await _testController.GetCategoryById(categoryID);
            //Assert
            Assert.Null(category.Value);
        }
        //Post
        [Fact]
        public async Task CreateCategory_WhenCategoryExist()
        {
            //Arrange
            Category newCategory = new Category();
            IEnumerable<Category> validateCategory = new List<Category>();
            CategoryDTO categoryDTO = new CategoryDTO
            {
                Name = "Khoa học công nghệ"
            };
            Category categoryExits = new Category
            {
                Id = Convert.ToString(ObjectId.GenerateNewId()),
                Name = "Khoa học công nghệ"
            };
            _mockMapper.Setup(x => x.Map(categoryDTO, newCategory));
            _mockCategoryService.Setup(x => x.ValidateCategory(newCategory.Id, newCategory.Name)).ReturnsAsync(validateCategory);
            //Act
            IActionResult actionResult = await _testController.CreateCategory(categoryDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        [Fact]
        public async Task CreateCategory_Success()
        {
            Category newCategory = new Category();
            CategoryDTO categoryDTO = new CategoryDTO
            {
                Name = "Khoa học công nghệ"
            };
            _mockMapper.Setup(x => x.Map(categoryDTO, newCategory));
            _mockCategoryService.Setup(x => x.ValidateCategory(newCategory.Id, newCategory.Name)).ReturnsAsync(() => null);
            //Act
            IActionResult actionResult = await _testController.CreateCategory(categoryDTO);
            //Assert
            Assert.IsType<CreatedAtActionResult>(actionResult);
        }
        //Delete
        [Fact]
        public async Task DeleteCategory_WhenCategoryNotFound()
        {
            //Arrange
            string categoryId = Convert.ToString(ObjectId.GenerateNewId());
            _mockCategoryService.Setup(x => x.DeleteCategory(It.IsAny<string>())).Returns(() => null);
            //Act
            IActionResult actionResult = await _testController.DeleteCategory(categoryId);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        [Fact]
        public async Task DeleteCategory_Success()
        {
            //Arrange
            string categoryId = Convert.ToString(ObjectId.GenerateNewId());
            Category findCategory = new Category();
            List<Book> listBook = new List<Book>();
            _mockCategoryService.Setup(x=>x.GetCategoryById(categoryId)).ReturnsAsync(findCategory);
            //Act
            IActionResult actionResult = await _testController.DeleteCategory(categoryId);
            //Assert
            Assert.IsType<NoContentResult>(actionResult);
        }
        //Put
        [Fact]
        public async Task UpdateCategory_WhenCategoryNotFound()
        {
            //Arrange
            CategoryDTO categoryDTO = new CategoryDTO
            {
                Name = "Khoa học công nghệ"
            };
            _mockCategoryService.Setup(x => x.GetCategoryById(It.IsAny<string>())).ReturnsAsync(() => null);
            string categoryId = Convert.ToString(ObjectId.GenerateNewId());
            //Act
            IActionResult actionResult = await _testController.UpdateCategory(categoryId, categoryDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        [Fact]
        public async Task UpdateCategory_WhenNameExist()
        {
            //Arrange
            string categoryID = Convert.ToString(ObjectId.GenerateNewId());
            CategoryDTO categoryDTO = new CategoryDTO
            {
                Name = "Lập trình"
            };
            IEnumerable<Category> validateCategory = new List<Category>();
            Category existCategory = new Category
            {
                Id = Convert.ToString(ObjectId.GenerateNewId()),
                Name = "Giáo trình"
            };
            Category findCategory = new Category();
            _mockCategoryService.Setup(x => x.ValidateCategory(findCategory.Id, categoryDTO.Name)).ReturnsAsync(validateCategory);
            //Act
            IActionResult actionResult = await _testController.UpdateCategory(categoryID,categoryDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        [Fact]
        public async Task UpdateCategory_Success()
        {
            //Arrange
            string categoryId = Convert.ToString(ObjectId.GenerateNewId());
            Category findCategory = new Category();
            CategoryDTO categoryDTO = new CategoryDTO
            {
                Name = "Lập trình"
            };
            _mockCategoryService.Setup(x=>x.GetCategoryById(categoryId)).ReturnsAsync(findCategory);
            _mockMapper.Setup(x => x.Map(categoryDTO, findCategory));
            _mockCategoryService.Setup(x => x.ValidateCategory(categoryId, categoryDTO.Name)).ReturnsAsync(() => null);
            _mockCategoryService.Setup(x => x.UpdateCategory(categoryId, findCategory));
            //Act
            IActionResult actionResult = await _testController.UpdateCategory(categoryId,categoryDTO);
            //Assert
            Assert.IsType<CreatedAtActionResult>(actionResult);
        }
    }
}