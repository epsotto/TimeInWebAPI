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
        IDailyTimeOutDataAccess _timeOutDataAccess;

        public LoginBusinessRules(IUserDataAccess userDataDataAccess, IDailyTimeInDataAccess timeInDataAccess, 
            IDailyTimeOutDataAccess timeOutDataAccess)
        {
            _userDataAccess = userDataDataAccess;
            _timeInDataAccess = timeInDataAccess;
            _timeOutDataAccess = timeOutDataAccess;
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
        public VerifyClockInResultModel VerifyEmployeeClockIn(VerifyClockInModel model)
        {
            VerifyClockInResultModel result = new VerifyClockInResultModel()
            {
                ClockedIn = false
            };

            if (string.IsNullOrEmpty(model.UserName))
            {
                result.QueryStatus = "No data being processed.";
                return result;
            }

            var query = _timeInDataAccess.GetEmployeeRecentTimeIn(model.UserName);
            if(query == null)
            {
                result.QueryStatus = "Query success.";
                return result;
            }

            var queryTimeOut = _timeOutDataAccess.GetEmployeeDailyTimeOut(model.UserName, query.TimeInDttm);
            if(queryTimeOut == null)
            {
                result.QueryStatus = "Query success.";
                result.ActivityNm = query.Activity.ActivityNm;
                result.ActivityId = query.ActivityCd;
                result.ClockedIn = true;

                return result;
            }
            else
            {
                result.QueryStatus = "Query success.";

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateUserPassword(UpdatePasswordModel model)
        {
            if (model == null)
            {
                return "No data to process.";
            }
            if (string.IsNullOrEmpty(model.UserName))
            {
                return "Username is empty.";
            }
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                return "New Password is empty.";
            }
            if (string.IsNullOrEmpty(model.PreviousPassword))
            {
                return "Previous Password is empty.";
            }

            var user = _userDataAccess.GetUser(model.UserName);
            if (user == null)
            {
                return "User not found.";
            }

            if (user.UserPassword != EncryptPassword(model.PreviousPassword))
            {
                return "Previous password does not match.";
            }
            else
            {
                user.UserPassword = EncryptPassword(model.NewPassword);
                user.UpdateDttm = DateTime.Now;
                user.UpdateUserId = model.UserName;

                var result = _userDataAccess.UpdatePassword(user.UserKey, user.UserPassword);

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