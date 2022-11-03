namespace DapperStoredProc.Services
{
    public interface IEmailServices
    {
        bool sendEmail(string email, string token);
    }
}