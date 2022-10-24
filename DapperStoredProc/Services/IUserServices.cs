using DapperStoredProc.Models;

namespace DapperStoredProc.Services
{
    public interface IUserServices
    {
        int AddUser(User model);
        User GetEmpByEmail(string model);
        void CheckPassword(User model);
        int UpadateUserImage(User model);
    }
}