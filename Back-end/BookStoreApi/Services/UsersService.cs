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

namespace BookStoreApi.Services
{
    public class UsersService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private IUnitOfWork unitOfWork = GetUnitOfWork.UnitOfWork();
        public UsersService(IMapper mapper, IMemoryCache memoryCache)
        {
            _userRepository = GetRepository<User>.Repository(unitOfWork);
            _roleRepository = GetRepository<Role>.Repository(unitOfWork);
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public  UsersService(IMapper mapper, IMemoryCache memory,IRepository<User> userRepository,IRepository<Role> roleRepository)
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
                var validatePhone = await this._userRepository.Get(x => x.Id != newUser.Id && x.Email == newUser.Phone);
                if (validatePhone.Count() > 0)
                {
                    return new ErrorResult<User>(400, "Phone is exist");
                }
                var findRole = await this._roleRepository.GetByID(newUser.RoleId);
                if (findRole is null)
                {
                    return new ErrorResult<User>(400, "Foreign key (RoleId) does not exist");
                }
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

        public async Task<IEnumerable<User>> GetAllUser()
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
            return listUser;
        }

        public async Task<ApiResult<User>> GetUserById(string id)
        {
            var user = await this._userRepository.GetByID(id);
            if (user is null)
            {
                return new ErrorResult<User>(404, "User not found");
            }
            return new SuccessResult<User>(200, "Get success", user);
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
    }
}