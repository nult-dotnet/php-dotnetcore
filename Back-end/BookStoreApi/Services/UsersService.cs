using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.Interfaces;
using BookStoreApi.RepositoryPattern;
namespace BookStoreApi.Services
{
    public class UsersService : IUserService
    {
        private readonly IUnitOfWork<User> _unitOfWork;
        public UsersService()
        {
            this._unitOfWork = GetUnitOfWork<User>.UnitOfWork();
        }
        public async Task<IEnumerable<User>> GetUserAsync() => await this._unitOfWork.Repository.Get();
        public async Task<User?> GetUserAsync(string id) => await this._unitOfWork.Repository.GetByID(id);
        public async Task CreateUserAsync(User newUser) => await this._unitOfWork.Repository.Insert(newUser);
        public async Task UpdateUserAsync(string id,User updateUser) => await this._unitOfWork.Repository.Update(updateUser);
        public async Task DeleteUserAsync(string id) => await this._unitOfWork.Repository.Delete(id);
        public async Task<IEnumerable<User?>> ValidateEmailUser(string id, string email) => await this._unitOfWork.Repository.Get(x=>x.Id != id && x.Email == email); 
        public async Task<IEnumerable<User?>> ValidatePhoneUser(string id, string phone) => await this._unitOfWork.Repository.Get(x=>x.Id != id && x.Phone == phone);
        public async Task<IEnumerable<User>> GetListUserByRoleId(string roleId) => await this._unitOfWork.Repository.Get(x=>x.RoleId == roleId);
    }
}