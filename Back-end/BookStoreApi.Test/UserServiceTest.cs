using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApi.Models;
using BookStoreApi.Controllers;
using BookStoreApi.Interfaces;
using Moq;
using AutoMapper;
using Xunit;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.Services;
using BookStoreApi.DataAccess.GenericRepository;
using Microsoft.Extensions.DependencyInjection;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Test
{
    public class UserServiceTest
    {
        private readonly UsersService _sut;
        private readonly Mock<IRepository<User>> _mockUserRepository = new Mock<IRepository<User>>();
        private readonly Mock<IRepository<Role>> _mockRoleRepository = new Mock<IRepository<Role>>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly IMemoryCache _memoryCache;
        public UserServiceTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
            _sut = new UsersService(_mockIMapper.Object,_memoryCache,_mockUserRepository.Object,_mockRoleRepository.Object);
        }
        //Get
        [Fact]
        public async Task GetUserById_WhenUserNotFound()
        {
            //Arrange
            User user = new User();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<User> result = await this._sut.GetUserById(user.Id);
            //Assert
            Assert.Equal(false,result.IsSuccess);
        }
        [Fact]
        public async Task GetUserById_Success()
        {
            //Arrange
            User user = new User();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(user);
            //Act
            ApiResult<User> result = await this._sut.GetUserById(user.Id);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        //Post
        [Fact]
        public async Task CreateUser_WhenRoleNotFound()
        {
            //Arrange
            User user = new User();
            CreateUser userDTO = new CreateUser();
            _mockRoleRepository.Setup(x => x.GetByID(user.RoleId)).ReturnsAsync(() => null);
            //Act
            ApiResult<User> result = await this._sut.AddUser(userDTO);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task CreateUser_Success()
        {
            //Arrange
            User user = new User();
            CreateUser userDTO = new CreateUser();
            Role role = new Role();
            _mockRoleRepository.Setup(x => x.GetByID(user.RoleId)).ReturnsAsync(role);
            //Act
            ApiResult<User> result = await this._sut.AddUser(userDTO);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        //Delete
        [Fact]
        public async Task DeleteUser_WhenUserNotFound()
        {
            //Arrange
            User user = new User();
            CreateUser userDTO = new CreateUser();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<User> result = await this._sut.Delete(user.Id);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task DeleteUser_Success()
        {
            //Arrange
            User user = new User();
            CreateUser userDTO = new CreateUser();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(user);
            //Act
            ApiResult<User> result = await this._sut.Delete(user.Id);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        [Fact]
        public async Task UpdateUser_WhenUserNotFound()
        {
            //Arrange
            User user = new User();
            UpdateUser userDTO = new UpdateUser();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<User> result = await this._sut.UpdateUser(user.Id,userDTO);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task UpdateUser_WhenRoleNotFound()
        {
            //Arrange
            User user = new User();
            UpdateUser userDTO = new UpdateUser();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(user);
            _mockRoleRepository.Setup(x => x.GetByID(user.RoleId)).ReturnsAsync(() => null);
            //Act
            ApiResult<User> result = await this._sut.UpdateUser(user.Id,userDTO);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task UpdateUser_Success()
        {
            //Arrange
            User user = new User();
            UpdateUser userDTO = new UpdateUser();
            Role role = new Role();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(user);
            _mockRoleRepository.Setup(x => x.GetByID(user.RoleId)).ReturnsAsync(role);
            //Act
            ApiResult<User> result = await this._sut.UpdateUser(user.Id, userDTO);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        [Fact]
        public async Task PatchUser_WhenUserNotFound()
        {
            //Arrange
            User user = new User();
            JsonPatchDocument<UpdateUser> updateUser = new JsonPatchDocument<UpdateUser>();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<User> result = await this._sut.UpdateUserPath(user.Id, updateUser);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task PatchUser_WhenRoleNotFound()
        {
            //Arrange
            User user = new User();
            UpdateUser userDTO = new UpdateUser();
            JsonPatchDocument<UpdateUser> updateUser = new JsonPatchDocument<UpdateUser>();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(user);
            _mockIMapper.Setup(x => x.Map<UpdateUser>(user)).Returns(userDTO);
            _mockRoleRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<User> result = await this._sut.UpdateUserPath(user.Id, updateUser);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task PatchUser_Success()
        {
            //Arrange
            User user = new User();
            Role role = new Role();
            UpdateUser userDTO = new UpdateUser();
            JsonPatchDocument<UpdateUser> updateUser = new JsonPatchDocument<UpdateUser>();
            _mockUserRepository.Setup(x => x.GetByID(user.Id)).ReturnsAsync(user);
            _mockIMapper.Setup(x => x.Map<UpdateUser>(user)).Returns(userDTO);
            _mockRoleRepository.Setup(x => x.GetByID(user.RoleId)).ReturnsAsync(role);
            //Act
            ApiResult<User> result = await this._sut.UpdateUserPath(user.Id, updateUser);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
    }
}
