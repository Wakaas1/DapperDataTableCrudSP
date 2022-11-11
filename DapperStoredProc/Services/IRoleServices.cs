using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using System.Collections.Generic;

namespace DapperStoredProc.Services
{
    public interface IRoleServices
    {
        int AddRole(Role model);
        int AddUserRole(int userId, int roleId);
        void RemoveRole(int userId, int roleId);
        IEnumerable<UserDetail> GetAllUsers(UserDetail model);
        List<RoleEdit> GetAllRole(int uId);
    }
    
}