using BookStoreApi.ApiActionResult;
using BookStoreApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStoreApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<ApiResult<User>> GetUserById(string id);
        Task<ApiResult<User>> AddUser(CreateUser createUser);
        Task<ApiResult<User>> Delete(string id);
        Task<ApiResult<User>> UpdateUser(string id, UpdateUser updateUser);
        Task<ApiResult<User>> UpdateUserPath(string id, JsonPatchDocument<UpdateUser> updateUser);
    }
}