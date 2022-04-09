using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using BookStoreApi.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.MemoryCaches;
using BookStoreApi.ApiActionResult;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IEnumerable<Role>> GetRoles() {
            return await this._roleService.GetAllRole();
        }  
        [HttpGet("{id}")]
        public async Task<ApiResult<Role>> GetRoleById(string id)
        {
            return await this._roleService.GetRoleById(id);
        }
        [HttpPost]
        public async Task<ApiResult<Role>> CreateRole([FromBody] RoleDTO createRole)
        {
            return await this._roleService.AddRole(createRole);
        }
        [HttpDelete("{id}")]
        public async Task<ApiResult<Role>> DeleteRole(string id)
        {
            return await this._roleService.Delete(id);
        }
        [HttpPut("{id}")]
        public async Task<ApiResult<Role>> UpdateRole(string id,[FromBody] RoleDTO updateRole)
        {
            return await this._roleService.UpdateRole(id, updateRole);
        }
    }
}