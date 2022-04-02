using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.Interfaces.Services;
using BookStoreDesktop.Models;
using BookStoreDesktop.Automapper;
using BookStoreDesktop.RepositoryPattern;
namespace BookStoreDesktop.Services
{
    public class RoleService : IRoleService
    {
        private readonly UnitOfWork _unitOfWork;
        public RoleService()
        {
            this._unitOfWork = new UnitOfWork(); 
        }
        public bool CreateRole(RoleDTO roleDTO)
        {
            Role newRole = new Role();
            ConfigMapper.configMapper().Map(roleDTO,newRole);
            var validateName = this._unitOfWork.RoleRepository.Get(x => x.Name == newRole.Name && x.Id != newRole.Id); 
            if(validateName.ToList().Count > 0)
            {
                MessageBox.Show("Tên chức vụ đã có, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            this._unitOfWork.RoleRepository.Insert(newRole);
            this._unitOfWork.Save();
            return true;
        }

        public bool DeleteRole(int id)
        {
           Role role = this._unitOfWork.RoleRepository.GetByID(id);
            if(role is null)
            {
                MessageBox.Show("Không tìm thấy chức vụ phù hợp, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            this._unitOfWork.RoleRepository.Delete(id);
            this._unitOfWork.Save();
            return true;
        }
        public List<Role> GetAllRole()
        {
            var listRole = this._unitOfWork.RoleRepository.Get();
            return listRole.ToList();
        }

        public List<Role> GetRoleByName(string name)
        {
            var listRole = this._unitOfWork.RoleRepository.Get(x => x.Name.Contains(name));
            return listRole.ToList();
        }

        public bool UpdateRole(RoleDTO roleDTO, int id)
        {
            Role role = this._unitOfWork.RoleRepository.GetByID(id);
            if(role is null)
            {
                MessageBox.Show("Không tìm thấy chức vụ phù hợp, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            ConfigMapper.configMapper().Map(roleDTO, role);
            var validateName = this._unitOfWork.RoleRepository.Get(x=>x.Name == role.Name && x.Id!=role.Id);
            if(validateName.ToList().Count > 0)
            {
                MessageBox.Show("Tên chức vụ đã có, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            this._unitOfWork.RoleRepository.Update(role);
            this._unitOfWork.Save();
            return true;
        }
    }
}
