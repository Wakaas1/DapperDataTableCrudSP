using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using System.Collections.Generic;

namespace DapperStoredProc.Services
{
    public interface IUserServices
    {
        int AddUser(Users model);
        int UpdateUser(Users model);
        Users GetUserByID(int Id);
        Users GetUserByEmail(string model);
        int DeleteUser(int Id);
        string CreatePasswordHash(string password);
        int UpadateUserImage(Users model);
        bool VerifyPasswordHash(string dbpassword, string password);
        void UpdatePassword(string email, string password);
        void UpdateToken(string email, string token);
        public IEnumerable<UserRolePartial> UserListId(int id);
        void UserIsVerified(string email, bool verify);
        IEnumerable<UserDetail> GetAllUsers(UserDetail model);
        int AddRole(Role model);
        int UpdateRole(Role model);
        Role GetRoleById(int Id);
    }
}