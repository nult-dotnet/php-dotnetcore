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

namespace BookStoreApi.Test
{
    public class RoleControllerTest
    {
        private readonly RoleController _sut;
        private readonly Mock<IUserService> _mockUserService = new Mock<IUserService>();
        private readonly Mock<IRoleService> _mockRoleService = new Mock<IRoleService>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly Mock<IMemoryCache> _memoryCache = new Mock<IMemoryCache>();
        public RoleControllerTest()
        {
            _sut = new RoleController(_mockRoleService.Object, _mockUserService.Object, _mockIMapper.Object,_memoryCache.Object);
        }
        //Get
        [Fact]
        public async Task GetRole_NotFound()
        {
            //Arrange
            string RoleId = Convert.ToString(ObjectId.GenerateNewId());
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            ActionResult<Role> result = await _sut.GetRoleById(RoleId);
            //Assert
            Assert.Null(result.Value);
        }
        [Fact]
        public async Task GetRole_Success()
        {
            //Arrange
            string roleId = Convert.ToString(ObjectId.GenerateNewId());
            Role findRole = new Role();
            _mockRoleService.Setup(x=>x.GetRoleById(roleId)).ReturnsAsync(findRole);
            //Act
            ActionResult<Role> result = await this._sut.GetRoleById(roleId);
            //Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Role role = Assert.IsType<Role>(okObjectResult.Value);
            Assert.Equal(findRole.Id, role.Id);
        }
        //Post
        [Fact]
        public async Task CreateRole_WhenRoleExist()
        {
            //Arrange
            Role role = new Role();
            IEnumerable<Role> validateRole = new List<Role>();
            RoleDTO roleDTO = new RoleDTO();
            _mockIMapper.Setup(x => x.Map(roleDTO, role));
            _mockRoleService.Setup(x => x.ValidateRoleName(role.Id,role.Name)).ReturnsAsync(validateRole);
            //Act
            IActionResult result = await this._sut.CreateRole(roleDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateRole_Success()
        {
            //Arrange
            RoleDTO roleDTO = new RoleDTO();
            Role newRole = new Role();
            _mockIMapper.Setup(x => x.Map(roleDTO, newRole));
            _mockRoleService.Setup(x => x.ValidateRoleName(newRole.Id, newRole.Name)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.CreateRole(roleDTO);
            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
        //Delete
        [Fact]
        public async Task DeleteRole_WhenRoleNotFound()
        {
            //Arrange
            string roleId = Convert.ToString(ObjectId.GenerateNewId());
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.DeleteRole(roleId);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteRole_Success()
        {
            //Arrange
            string roleID = Convert.ToString(ObjectId.GenerateNewId());
            Role role = new Role();
            //Act
            _mockRoleService.Setup(x => x.GetRoleById(roleID)).ReturnsAsync(role);
            IActionResult result = await this._sut.DeleteRole(roleID);
            //Assert
            Assert.IsType<NoContentResult>(result);
        }
        //Put
        [Fact]
        public async Task UpdateRole_WhenRoleNotFound()
        {
            //Arrange
            string roleId = Convert.ToString(ObjectId.GenerateNewId());
            RoleDTO roleDTO = new RoleDTO();
            _mockRoleService.Setup(x => x.GetRoleById(roleId)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.UpdateRole(roleId, roleDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateRole_WhenRoleExist()
        {
            //Arrange
            string roleId = Convert.ToString(ObjectId.GenerateNewId());
            RoleDTO roleDTO = new RoleDTO();
            IEnumerable<Role> validateRole = new List<Role>();
            Role role = new Role();
            _mockRoleService.Setup(x=>x.GetRoleById(roleId)).ReturnsAsync(role);
            _mockRoleService.Setup(x => x.ValidateRoleName(roleId, roleDTO.Name)).ReturnsAsync(validateRole);
            //Act
            IActionResult result = await this._sut.UpdateRole(roleId, roleDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateRole_Success()
        {
            //Arrange
            string roleId = Convert.ToString(ObjectId.GenerateNewId());
            RoleDTO roleDTO = new RoleDTO();
            Role role = new Role();
            _mockRoleService.Setup(x => x.GetRoleById(roleId)).ReturnsAsync(role);
            _mockRoleService.Setup(x => x.ValidateRoleName(roleId, roleDTO.Name)).ReturnsAsync(() => null);
            _mockIMapper.Setup(x => x.Map(roleDTO, role));
            //Act
            IActionResult result = await this._sut.UpdateRole(roleId, roleDTO);
            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}