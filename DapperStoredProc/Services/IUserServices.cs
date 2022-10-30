using DapperStoredProc.Models;

namespace DapperStoredProc.Services
{
    public interface IUserServices
    {
        int AddUser(Users model);
        Users GetUserByEmail(string model);
        string CreatePasswordHash(string password);
        int UpadateUserImage(Users model);
        bool VerifyPasswordHash(string dbpassword, string password);
        void UpdatePassword(string email, string password);
        void UpdateToken(string email, string token);
    }
}