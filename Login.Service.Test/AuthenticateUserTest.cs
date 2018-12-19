using Autofac;
using LoginService;
using LoginService.BusinessLayer;
using LoginService.Controllers;
using LoginService.Interfaces;
using LoginService.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInRepository;
using TimeInRepository.Utilities;

namespace Login.Service.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class AuthenticateUserTest
    {
        [Test]
        public void AuthenticateUser_WhenPasswordIsBlank_ReturnsPasswordIsEmptyErrorMessage()
        {
            AuthenticateModel input = new AuthenticateModel
            {
                UserName = "john.doe",
                Password = "" //encrypted string for 'password'
            };

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IUserDataAccess> mock = new Mock<IUserDataAccess>();
            mock.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 1,
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                UserName = "john.doe",
                UserPassword = "LTg9BIob8urwz643K5+pBA==", //encrypted string for password
                CreateDttm = DateTime.Now,
                CreateUserId = "john.doe",
                UpdateDttm = DateTime.Now,
                UpdateUserId = "john.doe"
            });

            ILoginBusinessRules app = new LoginBusinessRules(mock.Object, timeIn.Object);

            var output = app.VerifyValidUser(input);

            Assert.AreEqual("Password is empty.", output);
        }

        [Test]
        public void AuthenticateUser_WhenPasswordNotMatch_ReturnsPasswordNotMatchErrorMessage()
        {
            AuthenticateModel input = new AuthenticateModel
            {
                UserName = "john.doe",
                Password = "otherPassword"
            };

            string password = "Al1eq36aoBq6K0+pAA==";

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<ILoginBusinessRules> mockPassword = new Mock<ILoginBusinessRules>();
            mockPassword.Setup(x => x.EncryptPassword(input.Password)).Returns(password);

            Mock<IUserDataAccess> mock = new Mock<IUserDataAccess>();
            mock.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 1,
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                UserName = "john.doe",
                UserPassword = "LTg9BIob8urwz643K5+pBA==", //encrypted string for password
                CreateDttm = DateTime.Now,
                CreateUserId = "john.doe",
                UpdateDttm = DateTime.Now,
                UpdateUserId = "john.doe"
            });

            ILoginBusinessRules app = new LoginBusinessRules(mock.Object, timeIn.Object);

            string encryption = app.EncryptPassword(input.Password);
            var output = app.VerifyValidUser(input);

            Assert.AreEqual("Password does not match.", output);
        }

        [Test]
        public void AuthenticateUser_WhenPasswordMatch_ReturnPasswordMatchSuccess()
        {
            AuthenticateModel input = new AuthenticateModel
            {
                UserName = "john.doe",
                Password = "password" 
            };

            string password = "LTg9BIob8urwz643K5+pBA=="; //encrypted string for 'password'

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<ILoginBusinessRules> mockPassword = new Mock<ILoginBusinessRules>();
            mockPassword.Setup(x => x.EncryptPassword(input.Password)).Returns(password);

            Mock<IUserDataAccess> mock = new Mock<IUserDataAccess>();
            mock.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 1,
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                UserName = "john.doe",
                UserPassword = "LTg9BIob8urwz643K5+pBA==", //encrypted string for password
                CreateDttm = DateTime.Now,
                CreateUserId = "john.doe",
                UpdateDttm = DateTime.Now,
                UpdateUserId = "john.doe"
            });

            ILoginBusinessRules app = new LoginBusinessRules(mock.Object, timeIn.Object);

            var output = app.VerifyValidUser(input);

            Assert.AreEqual("Password matched.", output);
        }
    }
}
