using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.Models;
namespace BookStoreDesktop.Interfaces.Services
{
    public interface IRoleService
    {
        List<Role> GetAllRole();
        List<Role> GetRoleByName(string name);
        bool CreateRole(RoleDTO roleDTO);
        bool UpdateRole(RoleDTO roleDTO,int id);
        bool DeleteRole(int id);
    }
}
