using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }
        [HttpGet]
        public async Task<IEnumerable<User>> GetUser() {
            return await this._userService.GetAllUser();
        } 
        [HttpGet("{id}")]
        public async Task<ApiResult<User>> GetUserById(string id){
            return await this._userService.GetUserById(id);
        }
        [HttpPost]
        public async Task<ApiResult<User>> PostItemUser([FromBody] CreateUser createUser)
        {
            return await this._userService.AddUser(createUser);
        }
        [HttpDelete("{id}")]
        public async Task<ApiResult<User>> DeleteItemUser(string id)
        {
            return await this._userService.Delete(id);

        }
        [HttpPut("{id}")]
        public async Task<ApiResult<User>> UpdateUserById(string id,[FromBody] UpdateUser updateUser)
        {
            return await this._userService.UpdateUser(id, updateUser);
        }
        [HttpPatch("{id}")]
        public async Task<ApiResult<User>> UpdatePatch(string id,[FromBody] JsonPatchDocument<UpdateUser> updateUser)
        {
            return await this._userService.UpdateUserPath(id, updateUser);
        }
    }
}