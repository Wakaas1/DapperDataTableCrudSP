namespace DapperStoredProc.Services
{
    public interface IEmailServices
    {
        void SendEmail(string email, string token, string name);
    }
}