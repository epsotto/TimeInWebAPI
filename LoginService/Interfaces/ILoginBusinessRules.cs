using LoginService.Models;

namespace LoginService.Interfaces
{
    public interface ILoginBusinessRules
    {
        string VerifyValidUser(AuthenticateModel password);
        string DecryptPassword(string password);
    }
}