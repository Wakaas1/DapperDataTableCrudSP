using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using System.Collections.Generic;

namespace DapperStoredProc.Services
{
    public interface IRoleServices
    {
        int AddRole(Role model);
        int DeleteRole(int Id);
        IEnumerable<Role> GetAllRole();
        Role GetRoleById(int Id);
        int UpdateRole(Role model);
        IEnumerable<Users> UserList(Users model);
    }
    
}