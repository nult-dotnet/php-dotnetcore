using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRoles();
        Task<Role> GetRoleById(string id);
        Task CreateRole(Role newRole);
        Task UpdateRoleById(string id, Role updateRole);
        Task DeleteRoleById(string id);
        Task<Role> ValidateRoleName(string id, string name);
    }
}