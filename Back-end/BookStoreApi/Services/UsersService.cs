using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.Interfaces;
using BookStoreApi.RepositoryPattern;
using BookStoreApi.ApiActionResult;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.DataAccess.UnitOfWork;
using BookStoreApi.MemoryCaches;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.SignalR;
using BookStoreApi.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using BookStoreApi.Authenticate;

namespace BookStoreApi.Services
{
    public class UsersService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private IUnitOfWork unitOfWork = GetUnitOfWork.UnitOfWork();
        private readonly AppSettings _appSettings;
        public UsersService(IMapper mapper, IMemoryCache memoryCache,IOptions<AppSettings> appSettings)
        {
            _userRepository = GetRepository<User>.Repository(unitOfWork);
            _roleRepository = GetRepository<Role>.Repository(unitOfWork);
            _mapper = mapper;
            _memoryCache = memoryCache;
            _appSettings = appSettings.Value;
        }
        public UsersService(IMapper mapper, IMemoryCache memory,IRepository<User> userRepository,IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _memoryCache = memory;
        }
        public async Task<ApiResult<User>> AddUser(CreateUser createUser)
        {
            try
            {
                unitOfWork.CreateTransaction();
                User newUser = new User();
                this._mapper.Map(createUser, newUser);
                var validateEmail = await this._userRepository.Get(x => x.Id != newUser.Id && x.Email == newUser.Email);
                if (validateEmail.Count() > 0)
                {
                    return new ErrorResult<User>(400, "Email is exist");
                }
                var validatePhone = await this._userRepository.Get(x => x.Id != newUser.Id && x.Phone == newUser.Phone);
                if (validatePhone.Count() > 0)
                {
                    return new ErrorResult<User>(400, "Phone is exist");
                }
                var findRole = await this._roleRepository.GetByID(newUser.RoleId);
                if (findRole is null)
                {
                    return new ErrorResult<User>(400, "Foreign key (RoleId) does not exist");
                }
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(createUser.Password);
                RoleShow roleShow = this._mapper.Map<RoleShow>(findRole);
                //newUser.Role = roleShow;
                findRole.Quantity += 1;
                await this._roleRepository.Update(findRole);
                await this._userRepository.Insert(newUser);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listRole");
                
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<User>(201, "Create succecss", newUser);
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }

        public async Task<ApiResult<User>> Delete(string id)
        {
            try
            {
                unitOfWork.CreateTransaction();
                var user = await this._userRepository.GetByID(id);
                if (user is null)
                {
                    return new ErrorResult<User>(404, "User not found");
                }
                if (user.RoleId != null)
                {
                    Role findRole = await this._roleRepository.GetByID(user.RoleId);
                    findRole.Quantity -= 1;
                    await this._roleRepository.Update(findRole);
                }
                await this._userRepository.Delete(id);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listRole");
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<User>(200, "Delete success");
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<UserShow>> GetAllUser()
        {
            string cacheKey = "listUser";
            bool checkMemoryCacheAction = Memorycache.CheckMemoryCacheAction(this._memoryCache);
            bool checkMemoryCacheListUser = this._memoryCache.TryGetValue(cacheKey, out IEnumerable<User> listUser);
            if (checkMemoryCacheAction || !checkMemoryCacheListUser)
            {
                listUser = await this._userRepository.Get();
                this._memoryCache.Set(cacheKey, listUser, Memorycache.SetMemoryCache());
                Memorycache.RemoveMemoryCacheAction(this._memoryCache);
            }
            IEnumerable<UserShow> listUserShow = this._mapper.Map<IEnumerable<User>,IEnumerable<UserShow>>(listUser);
            return listUserShow;
        }
        public async Task<ApiResult<UserShow>> GetUserById(string id)
        {
            var user = await this._userRepository.GetByID(id);
            if (user is null)
            {
                return new ErrorResult<UserShow>(404, "User not found");
            }
            var userShow = this._mapper.Map<User,UserShow>(user);
            return new SuccessResult<UserShow>(200, "Get success", userShow);
        }

        public async Task<ApiResult<User>> UpdateUser(string id, UpdateUser updateUser)
        {
            try
            {
                unitOfWork.CreateTransaction();
                User findUser = await this._userRepository.GetByID(id);
                if (findUser is null)
                {
                    return new ErrorResult<User>(404, "User not found");
                }
                bool error = false;
                var validatePhone = await this._userRepository.Get(x=>x.Id != id && x.Phone == updateUser.Phone);
                if (validatePhone.Count() > 0)
                {
                    return new ErrorResult<User>(404, "Phone is exist");
                }
                var findRole = await this._roleRepository.GetByID(updateUser.RoleId);
                if (findRole is null)
                {
                    return new ErrorResult<User>(400, "Foreign key (RoleId) does not exist");
                }
                if (findRole.Id != findUser.RoleId)
                {
                    if (findUser.RoleId != null)
                    {
                        Role roleOld = await this._roleRepository.GetByID(findUser.RoleId);
                        roleOld.Quantity -= 1;
                        await this._roleRepository.Update(roleOld);
                    }
                    findRole.Quantity += 1;
                    await this._roleRepository.Update(findRole);
                }
                this._mapper.Map(updateUser, findUser);
                RoleShow roleShow = this._mapper.Map<RoleShow>(findRole);
                //findUser.Role = roleShow;
                await this._userRepository.Update(findUser);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listRole");
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<User>(200, "Update success", findUser);
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }

        public async Task<ApiResult<User>> UpdateUserPath(string id, JsonPatchDocument<UpdateUser> updateUser)
        {
            try
            {
                unitOfWork.CreateTransaction();
                User findUser = await this._userRepository.GetByID(id);
                if (findUser is null)
                {
                    return new ErrorResult<User>(404, "User not found");
                }
                UpdateUser userDTO = this._mapper.Map<UpdateUser>(findUser);
                updateUser.ApplyTo(userDTO);
                this._mapper.Map(userDTO, findUser);
                bool error = false;
                var validatePhone = await this._userRepository.Get(x => x.Id != id && x.Phone == findUser.Phone);
                if (validatePhone.Count() > 0)
                {
                    return new ErrorResult<User>(404, "Phone is exist");
                }
                var findRole = await this._roleRepository.GetByID(findUser.RoleId);
                if (findRole is null)
                {
                    return new ErrorResult<User>(400, "Foreign key (RoleId) does not exist");
                }
                if (findRole.Id != findUser.RoleId)
                {
                    if (findUser.RoleId != null)
                    {
                        Role roleOld = await this._roleRepository.GetByID(findUser.RoleId);
                        roleOld.Quantity -= 1;
                        await this._roleRepository.Update(roleOld);
                    }
                    findRole.Quantity += 1;
                    await this._roleRepository.Update(findRole);
                }
                this._mapper.Map(updateUser, findUser);
                //findUser.Role = roleShow;
                await this._userRepository.Update(findUser);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                this._memoryCache.Remove("listRole");
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<User>(200, "Update success", findUser);
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }   

        public async Task<ApiResult<string>> Authenticate(AuthenticateRequest login)
        {
            var users = await this._userRepository.Get(x => x.Email == login.Email);
            if (users.Count() == 0) 
                return new ErrorResult<string>(400, "Login failed");
            User user = users.First();
            bool checkPass = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            if (!checkPass)
                return new ErrorResult<string>(400, "Login failed");
            var token = GenerateJwtToken(user);
            return new SuccessResult<string>(200, "Login success", token);
        }
    }
}