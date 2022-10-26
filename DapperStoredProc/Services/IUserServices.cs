using DapperStoredProc.Models;

namespace DapperStoredProc.Services
{
    public interface IUserServices
    {
        int AddUser(Users model);
        Users GetEmpByEmail(string model);
        string CreatePasswordHash(string password);
        int UpadateUserImage(Users model);
        bool VerifyPasswordHash(string dbpassword, string password);
    }
}