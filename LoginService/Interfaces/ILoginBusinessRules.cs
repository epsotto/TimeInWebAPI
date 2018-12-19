using LoginService.Models;

namespace LoginService.Interfaces
{
    public interface ILoginBusinessRules
    {
        string VerifyValidUser(AuthenticateModel password);
        string EncryptPassword(string password);
        VerifyClockInModel VerifyEmployeeClockIn(string userName);
    }
}