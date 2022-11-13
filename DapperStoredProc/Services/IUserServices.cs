using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using DapperStoredProc.Models.DataTable;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<DataTableResponse<UserPartial>> GetAllUserAsync(DataTableRequest request);
        
    }
}