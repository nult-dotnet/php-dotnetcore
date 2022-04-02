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

namespace BookStoreApi.Test
{
    public class UserControllerTest
    {
        private readonly UserController _sut;
        private readonly Mock<IUserService> _mockUserService = new Mock<IUserService>();
        private readonly Mock<IRoleService> _mockRoleService = new Mock<IRoleService>();
        private readonly Mock<IMapper> _mockIMapper = new Mock<IMapper>();
        private readonly Mock<IMemoryCache> _memoryCache = new Mock<IMemoryCache>();
        public UserControllerTest()
        {
            _sut = new UserController(_mockUserService.Object, _mockRoleService.Object,_mockIMapper.Object,_memoryCache.Object);
        }
        //Get
        [Fact]
        public async Task GetUserById_WhenUserNotFound()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            _mockUserService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            ActionResult<User> result = await this._sut.GetUserById(userId);
            //Assert
            Assert.Null(result.Value);
        }
        [Fact]
        public async Task GetUserById_Success()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            User user = new User();
            _mockUserService.Setup(x=>x.GetUserAsync(userId)).ReturnsAsync(user);
            //Act
            ActionResult<User> result = await this._sut.GetUserById(userId);
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            User resultUser = Assert.IsType<User>(okObjectResult.Value);
            //Assert
            Assert.Equal(user,resultUser);
        }
        //Post
        [Fact]
        public async Task CreateUser_WhenEmailExist()
        {
            //Arrange
            CreateUser userDTO = new CreateUser();
            User newUser = new User();
            IEnumerable<User> validateUser = new List<User>();
            User validateEmailUser = new User();
            _mockIMapper.Setup(x => x.Map(userDTO, newUser));
            _mockUserService.Setup(x=>x.ValidateEmailUser(newUser.Id,newUser.Email)).ReturnsAsync(validateUser);
            //Act
            IActionResult result =  await this._sut.PostItemUser(userDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateUser_WhenPhoneExist()
        {
            //Arrange
            CreateUser userDTO = new CreateUser();
            User newUser = new User();
            IEnumerable<User> validateUser = new List<User>();
            User validatePhonelUser = new User();
            _mockIMapper.Setup(x => x.Map(userDTO, newUser));
            _mockUserService.Setup(x => x.ValidatePhoneUser(newUser.Id, newUser.Phone)).ReturnsAsync(validateUser);
            //Act
            IActionResult result = await this._sut.PostItemUser(userDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateUser_WhenRoleNotFound()
        {
            //Arrange
            CreateUser userDTO = new CreateUser();
            User newUser = new User();
            _mockIMapper.Setup(x => x.Map(userDTO, newUser));
            _mockRoleService.Setup(x => x.GetRoleById(newUser.RoleId)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.PostItemUser(userDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateUser_Success()
        {
            //Arrange
            CreateUser userDTO = new CreateUser();
            User newUser = new User();
            Role role = new Role();
            _mockIMapper.Setup(x => x.Map(userDTO, newUser));
            _mockRoleService.Setup(x => x.GetRoleById(newUser.RoleId)).ReturnsAsync(role);
            _mockUserService.Setup(x => x.ValidatePhoneUser(newUser.Id, newUser.Phone)).ReturnsAsync(()=>null);
            _mockUserService.Setup(x => x.ValidateEmailUser(newUser.Id, newUser.Email)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.PostItemUser(userDTO);
            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
        //Delete
        [Fact]
        public async Task DeleteUser_WhenUserNotFound()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            _mockUserService.Setup(x => x.DeleteUserAsync(userId)).Returns(() => null);
            //Act
            IActionResult result = await this._sut.DeleteItemUser(userId);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteUser_Success()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            User user = new User();
            _mockUserService.Setup(x=>x.GetUserAsync(userId)).ReturnsAsync(user);
            //Act
            IActionResult result = await this._sut.DeleteItemUser(userId);
            //Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task UpdateUser_WhenUserNotFound()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            UpdateUser userDTO = new UpdateUser();
            _mockUserService.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.UpdateUserById(userId, userDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateUser_WhenPhoneExist()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            UpdateUser userDTO = new UpdateUser();
            User user = new User();
            IEnumerable<User> validateUser = new List<User>();
            User validatePhonelUser = new User();
            _mockUserService.Setup(x=>x.GetUserAsync(userId)).ReturnsAsync(user);
            _mockUserService.Setup(x => x.ValidatePhoneUser(user.Id, user.Phone)).ReturnsAsync(validateUser);
            //Act
            IActionResult result = await this._sut.UpdateUserById(userId,userDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateUser_WhenRoleNotFound()
        {
            //Arrange
            UpdateUser userDTO = new UpdateUser();
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            User user = new User();
            _mockUserService.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(user);
            _mockUserService.Setup(x => x.ValidatePhoneUser(user.Id, user.Phone)).ReturnsAsync(() => null); ;
            _mockRoleService.Setup(x => x.GetRoleById(user.RoleId)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.UpdateUserById(userId,userDTO);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateUser_Success()
        {
            //Arrange
            UpdateUser userDTO = new UpdateUser();
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            User user = new User();
            Role role = new Role();
            _mockUserService.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(user);
            _mockUserService.Setup(x => x.ValidatePhoneUser(user.Id, user.Phone)).ReturnsAsync(() => null); ;
            _mockRoleService.Setup(x => x.GetRoleById(user.RoleId)).ReturnsAsync(role);
            //Act
            IActionResult result = await this._sut.UpdateUserById(userId, userDTO);
            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
        [Fact]
        public async Task PatchUser_WhenUserNotFound()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            JsonPatchDocument<UpdateUser> updateUser = new JsonPatchDocument<UpdateUser>();
            _mockUserService.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.UpdatePatch(userId, updateUser);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task PatchUser_WhenRoleNotFound()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            User user = new User();
            UpdateUser userDTO = new UpdateUser();
            JsonPatchDocument<UpdateUser> updaterUser = new JsonPatchDocument<UpdateUser>();
            _mockIMapper.Setup(x => x.Map<UpdateUser>(user)).Returns(userDTO);
            updaterUser.ApplyTo(userDTO);
            _mockUserService.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(user);
            _mockIMapper.Setup(x => x.Map(userDTO, user));
            _mockRoleService.Setup(x => x.GetRoleById(user.Id)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.UpdatePatch(userId, updaterUser);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task PatchUser_WhenPhoneExist()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            User user = new User();
            Role role = new Role();
            IEnumerable<User> validateUser = new List<User>();
            UpdateUser userDTO = new UpdateUser();
            JsonPatchDocument<UpdateUser> updaterUser = new JsonPatchDocument<UpdateUser>();
            _mockIMapper.Setup(x => x.Map<UpdateUser>(user)).Returns(userDTO);
            updaterUser.ApplyTo(userDTO);
            _mockUserService.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(user);
            _mockIMapper.Setup(x => x.Map(userDTO, user));
            _mockRoleService.Setup(x => x.GetRoleById(user.Id)).ReturnsAsync(role);
            _mockUserService.Setup(x => x.ValidatePhoneUser(user.Id, user.Phone)).ReturnsAsync(validateUser);
            //Act
            IActionResult result = await this._sut.UpdatePatch(userId, updaterUser);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task PatchUser_Success()
        {
            //Arrange
            string userId = Convert.ToString(ObjectId.GenerateNewId());
            User user = new User();
            Role role = new Role();
            UpdateUser userDTO = new UpdateUser();
            JsonPatchDocument<UpdateUser> updaterUser = new JsonPatchDocument<UpdateUser>();
            _mockIMapper.Setup(x => x.Map<UpdateUser>(user)).Returns(userDTO);
            updaterUser.ApplyTo(userDTO);
            _mockUserService.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(user);
            _mockIMapper.Setup(x => x.Map(userDTO, user));
            _mockRoleService.Setup(x => x.GetRoleById(user.Id)).ReturnsAsync(role);
            _mockUserService.Setup(x => x.ValidatePhoneUser(user.Id, user.Phone)).ReturnsAsync(() => null);
            //Act
            IActionResult result = await this._sut.UpdatePatch(userId, updaterUser);
            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}
