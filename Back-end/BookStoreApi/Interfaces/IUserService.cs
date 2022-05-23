using BookStoreApi.ApiActionResult;
using BookStoreApi.Authenticate;
using BookStoreApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStoreApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserShow>> GetAllUser();
        Task<ApiResult<UserShow>> GetUserById(string id);
        Task<ApiResult<User>> AddUser(CreateUser createUser);
        Task<ApiResult<User>> Delete(string id);
        Task<ApiResult<User>> UpdateUser(string id, UpdateUser updateUser);
        Task<ApiResult<User>> UpdateUserPath(string id, JsonPatchDocument<UpdateUser> updateUser);
        Task<ApiResult<string>> Authenticate(AuthenticateRequest login);
    }
}