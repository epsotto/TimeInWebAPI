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

        public LoginBusinessRules(IUserDataAccess userDataDataAccess)
        {
            _userDataAccess = userDataDataAccess;
        }

        private readonly static string hash = "t!m3!N";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public String VerifyValidUser(AuthenticateModel auth)
        {
            var container = ContainerConfig.Configure();

            var userData = _userDataAccess.GetUser(auth.UserName);

            if (auth.Password == String.Empty) { return "Password is empty."; }

            String decryptedPassword = DecryptPassword(userData.UserPassword);

            if (auth.Password != decryptedPassword) { return "Password does not match."; }

            return "Password matched.";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public String DecryptPassword(String password)
        {
            String decryptedPassword = String.Empty;
            if (String.IsNullOrEmpty(password)) return String.Empty;

            byte[] data = Convert.FromBase64String(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using(TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    decryptedPassword = UTF8Encoding.UTF8.GetString(results);
                }
            }

            return decryptedPassword;
        }
    }
}