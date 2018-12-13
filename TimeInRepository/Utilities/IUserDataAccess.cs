namespace TimeInRepository.Utilities
{
    public interface IUserDataAccess
    {
        User GetUser(string userName);
        string InsertNewUser(string firstName, string lastName, string userName);
        string UpdatePassword(int userId, string newPassword);
        bool VerifyPassword(string firstName, string lastName, string password);
    }
}