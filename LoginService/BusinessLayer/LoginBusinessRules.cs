using Autofac;
using LoginService.Interfaces;
using LoginService.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TimeInRepository.Utilities;

namespace LoginService.BusinessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginBusinessRules : ILoginBusinessRules
    {
        IUserDataAccess _userDataAccess;
        IDailyTimeInDataAccess _timeInDataAccess;

        public LoginBusinessRules(IUserDataAccess userDataDataAccess, IDailyTimeInDataAccess timeInDataAccess)
        {
            _userDataAccess = userDataDataAccess;
            _timeInDataAccess = timeInDataAccess;
        }

        private readonly static string hash = "t!m3!N";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string VerifyValidUser(AuthenticateModel auth)
        {
            var container = ContainerConfig.Configure();

            var userData = _userDataAccess.GetUser(auth.UserName);

            if(userData == null)
            {
                return "Employee not found.";
            }

            if (auth.Password == string.Empty) { return "Password is empty."; }

            string decryptedPassword = EncryptPassword(auth.Password);

            if (userData.UserPassword != decryptedPassword) { return "Password does not match."; }

            return "Password matched.";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public VerifyClockInModel VerifyEmployeeClockIn(string userName)
        {
            VerifyClockInModel result = new VerifyClockInModel();
            result.ClockedIn = false;

            if (string.IsNullOrEmpty(userName))
            {
                result.QueryStatus = "No data being processed.";
                return result;
            }

            var query = _timeInDataAccess.GetEmployeeDailyTimeIn(userName);
            if(query == null)
            {
                result.QueryStatus = "Query success.";
                return result;
            }
            else
            {
                result.QueryStatus = "Query success.";
                result.ClockedIn = true;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptPassword(string password)
        {
            string encryptedPassword = string.Empty;
            if (string.IsNullOrEmpty(password)) return string.Empty;

            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using(TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    encryptedPassword = Convert.ToBase64String(results,0,results.Length);
                }
            }

            return encryptedPassword;
        }
    }
}