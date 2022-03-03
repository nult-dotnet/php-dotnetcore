using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using BookStoreApi.Interfaces;
namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController:ControllerBase
    {
        private readonly IRoleService _rolesService;
        private readonly IUserService _usersService;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService,IUserService usersService,IMapper mapper)
        {
            _rolesService = roleService;
            _usersService = usersService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<List<Role>> GetRoles() => await this._rolesService.GetRoles();

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(string id)
        {
            var findRole = await this._rolesService.GetRoleById(id);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Role not found");
                return BadRequest(ModelState);
            }
            return Ok(findRole);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO createRole)
        {
            Role newRole = new Role();
            this._mapper.Map(createRole, newRole);
            Role validateRole = await this._rolesService.ValidateRoleName(newRole.Id, newRole.Name);
            if(validateRole != null)
            {
                ModelState.AddModelError("Error", "Role is exist");
                return BadRequest(ModelState);
            }
            await this._rolesService.CreateRole(newRole);
            return CreatedAtAction(nameof(GetRoleById), new { id = newRole.Id }, newRole);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var findRole = await this._rolesService.GetRoleById(id);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Role not found");
                return BadRequest(ModelState);
            }
            List<User> listUser = await this._usersService.GetListUserByRoleId(id);
            if (listUser != null)
            {
                foreach (User user in listUser)
                {
                    user.RoleId = null;
                    user.Role = null;
                    await this._usersService.UpdateUserAsync(user.Id, user);
                }
            }
            await this._rolesService.DeleteRoleById(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id,[FromBody] RoleDTO updateRole)
        {
            var findRole = await this._rolesService.GetRoleById(id);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Role not found");
                return BadRequest(ModelState);
            }
            var validateRole = await this._rolesService.ValidateRoleName(id, updateRole.Name);
            if(validateRole != null)
            {
                ModelState.AddModelError("Error", "Role is exist");
                return BadRequest(ModelState);
            }
            this._mapper.Map(updateRole, findRole);
            RoleShow roleShow = this._mapper.Map<RoleShow>(findRole);
            List<User> listUser = await this._usersService.GetListUserByRoleId(id);
            if(listUser != null)
            {
                foreach (User item in listUser)
                {
                    item.Role = roleShow;
                    await this._usersService.UpdateUserAsync(item.Id, item);
                }
            }
            await this._rolesService.UpdateRoleById(findRole.Id, findRole);
            return CreatedAtAction(nameof(GetRoleById), new { id = findRole.Id },findRole);
        }
    }
}