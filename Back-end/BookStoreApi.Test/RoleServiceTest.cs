using System;
using System.Threading.Tasks;
using BookStoreApi.Interfaces;
using BookStoreApi.Models;
using BookStoreApi.Controllers;
using Moq;
using AutoMapper;
using Xunit;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.Services;
using Microsoft.Extensions.DependencyInjection;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Test
{
    public class RoleServiceTest    
    {
        private readonly RolesService _sut;
        private readonly Mock<IRepository<User>> _mockUserRepository = new Mock<IRepository<User>>();
        private readonly Mock<IRepository<Role>> _mockRoleRepository = new Mock<IRepository<Role>>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly IMemoryCache _memoryCache;
        public RoleServiceTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
            _sut = new RolesService(_mockIMapper.Object, _memoryCache,_mockUserRepository.Object,_mockRoleRepository.Object);
        }
        //Get
        [Fact]
        public async Task GetRole_NotFound()
        {
            //Arrange
            Role role = new Role();
            _mockRoleRepository.Setup(x => x.GetByID(role.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Role> result = await _sut.GetRoleById(role.Id);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task GetRole_Success()
        {
            //Arrange
            Role role = new Role();
            _mockRoleRepository.Setup(x => x.GetByID(role.Id)).ReturnsAsync(role);
            //Act
            ApiResult<Role> result = await _sut.GetRoleById(role.Id);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        [Fact]
        public async Task CreateRole_Success()
        {
            //Arrange
            RoleDTO roleDTO = new RoleDTO();
            Role newRole = new Role();
            _mockIMapper.Setup(x => x.Map(roleDTO, newRole));
            //Act
            ApiResult<Role> result = await _sut.AddRole(roleDTO);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        //Delete
        [Fact]
        public async Task DeleteRole_WhenRoleNotFound()
        {
            //Arrange
            Role role = new Role();
            _mockRoleRepository.Setup(x => x.GetByID(role.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Role> result = await _sut.Delete(role.Id);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task DeleteRole_Success()
        {
            //Arrange
            Role role = new Role();
            _mockRoleRepository.Setup(x => x.GetByID(role.Id)).ReturnsAsync(role);
            //Act
            ApiResult<Role> result = await _sut.Delete(role.Id);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
        //Put
        [Fact]
        public async Task UpdateRole_WhenRoleNotFound()
        {
            //Arrange
            RoleDTO roleDTO = new RoleDTO();
            Role role = new Role();
            _mockRoleRepository.Setup(x => x.GetByID(role.Id)).ReturnsAsync(() => null);
            //Act
            ApiResult<Role> result = await _sut.UpdateRole(role.Id,roleDTO);
            //Assert
            Assert.Equal(false, result.IsSuccess);
        }
        [Fact]
        public async Task UpdateRole_Success()
        {
            //Arrange
            RoleDTO roleDTO = new RoleDTO();
            Role role = new Role();
            _mockRoleRepository.Setup(x => x.GetByID(role.Id)).ReturnsAsync(role);
            //Act
            ApiResult<Role> result = await _sut.UpdateRole(role.Id, roleDTO);
            //Assert
            Assert.Equal(true, result.IsSuccess);
        }
    }
}