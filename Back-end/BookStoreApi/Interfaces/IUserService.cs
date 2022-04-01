using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUserAsync();
        Task<User?> GetUserAsync(string id);
        Task CreateUserAsync(User newUser);
        Task UpdateUserAsync(string id, User updateUser);
        Task DeleteUserAsync(string id);
        Task<User?> ValidateEmailUser(string id, string email);
        Task<User?> ValidatePhoneUser(string id, string phone);
        Task<List<User>> GetListUserByRoleId(string roleId);
    }
}
