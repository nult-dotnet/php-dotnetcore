using MongoDB.Driver;
using BookStoreApi.Models;
using BookStoreApi.RepositoryPattern;
using BookStoreApi.Interfaces;
using BookStoreApi.ApiActionResult;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.DataAccess.UnitOfWork;
using BookStoreApi.MemoryCaches;

namespace BookStoreApi.Services
{
    public class RolesService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private IUnitOfWork unitOfWork = GetUnitOfWork.UnitOfWork();
        public RolesService(IMapper mapper, IMemoryCache memoryCache)
        {
            _roleRepository = GetRepository<Role>.Repository(unitOfWork);
            _userRepository = GetRepository<User>.Repository(unitOfWork);
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public RolesService(IMapper mapper, IMemoryCache memoryCache,IRepository<User> userRepository,IRepository<Role> roelRepository)
        {
            _roleRepository = roelRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<ApiResult<Role>> AddRole(RoleDTO roleDTO)
        {
            Role newRole = new Role();
            this._mapper.Map(roleDTO, newRole);
            IEnumerable<Role> validateRole = await this._roleRepository.Get(x => x.Id != newRole.Id && x.Name == newRole.Name);
            if (validateRole.Count() > 0)
            {
                return new ErrorResult<Role>(400, "Role is exist");
            }
            await this._roleRepository.Insert(newRole);
            Memorycache.SetMemoryCacheAction(this._memoryCache);
            unitOfWork.Save();
            return new SuccessResult<Role>(201, "Create success", newRole);
        }
        public async Task<ApiResult<Role>> Delete(string id)
        {
            try
            {
                unitOfWork.CreateTransaction();
                var findRole = await this._roleRepository.GetByID(id);
                if (findRole is null)
                {
                    return new ErrorResult<Role>(404, "Role not found");
                }
                IEnumerable<User> listUser = await this._userRepository.Get(x => x.RoleId == id);
                if (listUser.Count() > 0)
                {
                    foreach (User user in listUser)
                    {
                        user.RoleId = null;
                        await this._userRepository.Update(user);
                    }
                }
                await this._roleRepository.Delete(id);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<Role>(200, "Delete success");
            }
            catch(Exception ex)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
        public async Task<IEnumerable<Role>> GetAllRole()
        {
            string cacheKeyListRole = "listRole";
            bool checkMemoryCacheAction = Memorycache.CheckMemoryCacheAction(this._memoryCache);
            bool checkMemoryCacheListRole = this._memoryCache.TryGetValue(cacheKeyListRole, out IEnumerable<Role> listRole);
            if (checkMemoryCacheAction || !checkMemoryCacheListRole)
            {
                listRole = await this._roleRepository.Get();
                this._memoryCache.Set(cacheKeyListRole, listRole, Memorycache.SetMemoryCache());
                Memorycache.RemoveMemoryCacheAction(this._memoryCache);
            }
            return listRole;
        }
        public async Task<ApiResult<Role>> GetRoleById(string id)
        {
            var findRole = await this._roleRepository.GetByID(id);
            if (findRole is null)
            {
                return new ErrorResult<Role>(404, "Role not found");
            }
            return new SuccessResult<Role>(200, "Get success", findRole);
        }

        public async Task<ApiResult<Role>> UpdateRole(string id, RoleDTO roleDTO)
        {
            try
            {
                unitOfWork.CreateTransaction();
                var findRole = await this._roleRepository.GetByID(id);
                if (findRole is null)
                {
                    return new ErrorResult<Role>(404, "Role not found");
                }
                var validateRole = await this._roleRepository.Get(x => x.Id != findRole.Id && x.Name == findRole.Name);
                if (validateRole.Count() > 0)
                {
                    return new ErrorResult<Role>(400, "Role is exist");
                }
                this._mapper.Map(roleDTO, findRole);
                RoleShow roleShow = this._mapper.Map<RoleShow>(findRole);
                IEnumerable<User> listUser = await this._userRepository.Get(x => x.RoleId == id);
                if (listUser.Count() > 0)
                {
                    foreach (User item in listUser)
                    {
                        await this._userRepository.Update(item);
                    }
                }
                await this._roleRepository.Update(findRole);
                Memorycache.SetMemoryCacheAction(this._memoryCache);
                unitOfWork.Save();
                unitOfWork.Commit();
                return new SuccessResult<Role>(200, "Update success", findRole);
            }
            catch(Exception exp)
            {
                unitOfWork.Rollback();
                throw new NotImplementedException();
            }
        }
    }
}