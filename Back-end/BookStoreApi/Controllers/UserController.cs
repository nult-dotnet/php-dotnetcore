﻿using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Interfaces;
namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IRoleService roleService,IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<List<User>> GetUser() => await this._userService.GetUserAsync();
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> GetUserById(string id){
            var user = await this._userService.GetUserAsync(id);
            if(user is null)
            {
                ModelState.AddModelError("Error", "Id not found");
                return BadRequest(ModelState);
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> PostItemUser([FromBody] CreateUser createUser)
        {
            var error = false;
            User newUser = new User();
            this._mapper.Map(createUser, newUser);
            var validateEmail = await this._userService.ValidateEmailUser(newUser.Id, newUser.Email);
            if(validateEmail != null)
            {
                ModelState.AddModelError("Error", "Email is exist");
                error = true;
            }
            var validatePhone = await this._userService.ValidatePhoneUser(newUser.Id, newUser.Phone);
            if(validatePhone != null)
            {
                ModelState.AddModelError("Error", "Phone is exist");
                error = true;
            }
            var findRole = await this._roleService.GetRoleById(newUser.RoleId);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Foreign key (RoleId) does not exist");
                error = true;
            }
            if (error)
            {
                return BadRequest(ModelState);
            }
            RoleShow roleShow = this._mapper.Map<RoleShow>(findRole);
            newUser.Role = roleShow;
            findRole.Quantity += 1;
            await this._roleService.UpdateRoleById(findRole.Id, findRole);
            await this._userService.CreateUserAsync(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteItemUser(string id)
        {
            var user = await this._userService.GetUserAsync(id);
            if(user is null)
            {
                ModelState.AddModelError("Error", "User not found");
                return BadRequest(ModelState);  
            }
            if(user.RoleId != null)
            {
                Role findRole = await this._roleService.GetRoleById(user.RoleId);
                findRole.Quantity -= 1;
                await this._roleService.UpdateRoleById(findRole.Id, findRole);
            }
            await this._userService.DeleteUserAsync(id);
            return NoContent();

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserById(string id,[FromBody] UpdateUser updateUser)
        {
            User findUser = await this._userService.GetUserAsync(id);
            if(findUser is null)
            {
                ModelState.AddModelError("Error", "User not found");
                return BadRequest(ModelState);
            }
            bool error = false;
            var validatePhone = await this._userService.ValidatePhoneUser(id,updateUser.Phone);
            if(validatePhone != null)
            {
                ModelState.AddModelError("Error", "Phone is exist");
                error = true;
            }
            var findRole = await this._roleService.GetRoleById(updateUser.RoleId);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Foreign key (RoleId) does not exist");
                error = true;
            }
            if (error)
            {
                return BadRequest(ModelState);
            }
           
            if(findRole.Id != findUser.RoleId)
            {
                if(findUser.RoleId != null)
                {
                    Role roleOld = await this._roleService.GetRoleById(findUser.RoleId);
                    roleOld.Quantity -= 1;
                    await this._roleService.UpdateRoleById(roleOld.Id, roleOld);
                }
                findRole.Quantity += 1;
                await this._roleService.UpdateRoleById(findRole.Id, findRole);
            }
            this._mapper.Map(updateUser, findUser);
            RoleShow roleShow = this._mapper.Map<RoleShow>(findRole);
            findUser.Role = roleShow;
            await this._userService.UpdateUserAsync(findUser.Id, findUser);
            return CreatedAtAction(nameof(GetUserById), new { id = findUser.Id }, findUser);
        }
        [HttpPatch("{id:length(24)}")]
        public async Task<IActionResult> UpdatePatch(string id,[FromBody] JsonPatchDocument<UpdateUser> updateUser)
        {
            User findUser = await this._userService.GetUserAsync(id);
            if(findUser is null)
            {
                ModelState.AddModelError("Error", "User not found");
                return BadRequest(ModelState);
            }
            var roleIdOld = findUser.RoleId;
            bool error = false;

            UpdateUser userDTO = this._mapper.Map<UpdateUser>(findUser);
            updateUser.ApplyTo(userDTO);
            this._mapper.Map(userDTO, findUser);

            Role findRole = await this._roleService.GetRoleById(findUser.RoleId);
            if(findRole is null)
            {
                ModelState.AddModelError("Error", "Foreign key (RoleId) does not exist");
                error = true;
            }
            User validatePhone = await this._userService.ValidatePhoneUser(findUser.Id, findUser.Phone);
            if(validatePhone != null)
            {
                ModelState.AddModelError("Error", "Phone is exist");
                error = true;
            }
            if (error)
            {
                return BadRequest(ModelState);
            }
            if (findRole.Id != roleIdOld)
            {
                Role roleOld = await this._roleService.GetRoleById(roleIdOld);
                roleOld.Quantity -= 1;
                await this._roleService.UpdateRoleById(roleOld.Id, roleOld);
                findRole.Quantity += 1;
                await this._roleService.UpdateRoleById(findRole.Id, findRole);
            }
            RoleShow roleShow = this._mapper.Map<RoleShow>(findRole);
            findUser.Role = roleShow;
            await this._userService.UpdateUserAsync(findUser.Id,findUser);
            return CreatedAtAction(nameof(GetUserById), new { id = findUser.Id }, findUser);
        }
    }
}