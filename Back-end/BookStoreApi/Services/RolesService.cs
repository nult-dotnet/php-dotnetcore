using MongoDB.Driver;
using BookStoreApi.Models;
using BookStoreApi.RepositoryPattern;
using BookStoreApi.Interfaces;
namespace BookStoreApi.Services
{
    public class RolesService : IRoleService
    {
        private readonly IUnitOfWork<Role> _unitOfWork;

        public RolesService()   
        {
          this._unitOfWork = GetUnitOfWork<Role>.UnitOfWork();
        }
        public async Task<IEnumerable<Role>> GetRoles() => await this._unitOfWork.Repository.Get(orderBy:x=>x.OrderByDescending(x=>x.TimeCreate));
        public async Task<Role?> GetRoleById(string id) => await this._unitOfWork.Repository.GetByID(id);
        public async Task CreateRole(Role newRole) => await this._unitOfWork.Repository.Insert(newRole);
        public async Task UpdateRoleById(string id, Role updateRole) => await this._unitOfWork.Repository.Update(updateRole);

        public async Task DeleteRoleById(string id) => await this._unitOfWork.Repository.Delete(id);
        public async Task<IEnumerable<Role?>> ValidateRoleName(string id, string name) => await this._unitOfWork.Repository.Get(x=>x.Id != id && x.Name == name);
    }
}