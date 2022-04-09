using BookStoreApi.ApiActionResult;
using BookStoreApi.Models;
namespace BookStoreApi.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRole();
        Task<ApiResult<Role>> GetRoleById(string id);
        Task<ApiResult<Role>> AddRole(RoleDTO roleDTO);
        Task<ApiResult<Role>> Delete(string id);
        Task<ApiResult<Role>> UpdateRole(string id, RoleDTO roleDTO);
    }
}