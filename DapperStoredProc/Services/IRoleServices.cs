using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using System.Collections.Generic;

namespace DapperStoredProc.Services
{
    public interface IRoleServices
    {
        int AddRole(int userId, int roleId);
        void RemoveRole(int userId, int roleId);
        IEnumerable<UserDetail> GetAllUsers(UserDetail model);
        IEnumerable<Role> GetAllRole();
    }
    
}