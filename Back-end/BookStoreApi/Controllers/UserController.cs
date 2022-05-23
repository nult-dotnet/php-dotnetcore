using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;
using BookStoreApi.ApiActionResult;
using Microsoft.AspNetCore.SignalR;
using BookStoreApi.SignalR;
using BookStoreApi.Authenticate;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private IHubContext<BroadcastHub, IHubClient> _hubContext;
        public UserController(IUserService userService, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _userService = userService;
            _hubContext= hubContext;
        }
        [HttpGet]
        public async Task<IEnumerable<UserShow>> GetUser() {
            return await this._userService.GetAllUser();
        } 
        [HttpGet("{id}")]
        public async Task<ApiResult<UserShow>> GetUserById(string id){
            return await this._userService.GetUserById(id);
        }
        [HttpPost]
        public async Task<ApiResult<User>> PostItemUser([FromBody] CreateUser createUser)
        {
            ApiResult<User> response = await this._userService.AddUser(createUser);
            await _hubContext.Clients.All.BroadcastMessage();
            return response;
        }
        [HttpDelete("{id}")]
        public async Task<ApiResult<User>> DeleteItemUser(string id)
        {
            ApiResult<User> response = await this._userService.Delete(id);
            await _hubContext.Clients.All.BroadcastMessage();
            return response;
        }
        [HttpPut("{id}")]
        public async Task<ApiResult<User>> UpdateUserById(string id,[FromBody] UpdateUser updateUser)
        {
            ApiResult<User> response = await this._userService.UpdateUser(id, updateUser);
            await _hubContext.Clients.All.BroadcastMessage();
            return response;
        }
        [HttpPatch("{id}")]
        public async Task<ApiResult<User>> UpdatePatch(string id,[FromBody] JsonPatchDocument<UpdateUser> updateUser)
        {
            return await this._userService.UpdateUserPath(id, updateUser);
        }
        [HttpPost("login")]
        public async Task<ApiResult<string>> Login(AuthenticateRequest login)
        {
            return await this._userService.Authenticate(login);
        }
    }
}