using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Cors;

namespace BookStoreApi.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController:ControllerBase
    {
        private readonly RolesService _rolesService;
        private readonly UsersService _usersService;
        private readonly IMapper _mapper;
        public RoleController(RolesService roleService,UsersService usersService,IMapper mapper)
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

        [EnableCors("MyPolicy")]
        [HttpPost("cache")]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ContentResult GetTime() => Content(DateTime.Now.Millisecond.ToString());
        [HttpPut]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO createRole)
        {
            Role newRole = new Role();
            this._mapper.Map(createRole, newRole);
            Role validateRole = await this._rolesService.ValidateRoleName(newRole.Id, newRole.Name);
            if(validateRole is null)
            {
                await this._rolesService.CreateRole(newRole);
                return CreatedAtAction(nameof(GetRoleById), new { id = newRole.Id }, newRole);
            }
            ModelState.AddModelError("Error", "Role is exist");
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var findRole = await this._rolesService.GetRoleById(id);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Id not found");
                return BadRequest(ModelState);
            }
            await this._rolesService.DeleteRoleById(id);
            List<User> listUser = await this._usersService.GetListUserByRoleId(id);
            foreach(User user in listUser)
            {
                user.RoleId = null;
                user.Role = null;
                await this._usersService.UpdateUserAsync(user.Id,user);
            }
            return StatusCode(200,"Delete success");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id,[FromBody] RoleDTO updateRole)
        {
            var findRole = await this._rolesService.GetRoleById(id);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Id not found");
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
            List<User> findUser = await this._usersService.GetListUserByRoleId(id);
            foreach(User item in findUser)
            {
                item.Role = roleShow;
                await this._usersService.UpdateUserAsync(item.Id, item);
            }
            await this._rolesService.UpdateRoleById(findRole.Id, findRole);
            return CreatedAtAction(nameof(GetRoleById), new { id = findRole.Id },findRole);
        }
    }
}