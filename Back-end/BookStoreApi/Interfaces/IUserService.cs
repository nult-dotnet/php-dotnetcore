using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUserAsync();
        Task<User?> GetUserAsync(string id);
        Task CreateUserAsync(User newUser);
        Task UpdateUserAsync(string id, User updateUser);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<User>> ValidateEmailUser(string id, string email);
        Task<IEnumerable<User>> ValidatePhoneUser(string id, string phone);
        Task<IEnumerable<User>> GetListUserByRoleId(string roleId);
    }
}
