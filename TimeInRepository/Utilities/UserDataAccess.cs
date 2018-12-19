using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInRepository.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDataAccess : IUserDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            User userQuery = new User();

            using (TimeInEntities context = new TimeInEntities())
            {
                userQuery = context.Users.Where(x => x.UserName == userName && x.IsActive == true).FirstOrDefault();
            }

            return userQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyPassword(string firstName, string lastName, string password)
        {
            using (TimeInEntities context = new TimeInEntities())
            {
                var userQuery = context.Users
                    .Where(x => x.FirstName.Equals(firstName) 
                    && x.LastName.Equals(lastName)
                    && x.UserPassword.Equals(password) && x.IsActive == true)
                    .FirstOrDefault();

                if(userQuery != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string InsertNewUser(string firstName, string lastName, string userName)
        {
            User newUser = new User();

            try
            {
                using (TimeInEntities context = new TimeInEntities())
                {
                    newUser.FirstName = firstName;
                    newUser.LastName = lastName;
                    newUser.IsActive = true;
                    newUser.UserName = userName;
                    newUser.UserPassword = "123456";
                    newUser.CreateDttm = DateTime.Now;
                    newUser.CreateUserId = "Admin";
                    newUser.UpdateDttm = DateTime.Now;
                    newUser.UpdateUserId = "Admin";

                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "New user inserted.";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public string UpdatePassword(int userId, string newPassword)
        {
            User userQuery = new User();

            try
            {
                using (TimeInEntities context = new TimeInEntities())
                {
                    userQuery = context.Users.Where(x => x.UserKey == userId && x.IsActive == true).FirstOrDefault();

                    userQuery.UserPassword = newPassword;
                    userQuery.UpdateDttm = DateTime.Now;
                    userQuery.UpdateUserId = userQuery.FirstName + userQuery.LastName;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return "User record updated.";
        }
    }
}