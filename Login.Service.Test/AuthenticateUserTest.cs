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
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ILoginBusinessRules>();

                AuthenticateModel input = new AuthenticateModel
                {
                    UserName = "john.doe",
                    Password = "" //encrypted string for 'password'
                };

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

                var output = app.VerifyValidUser(input);

                Assert.AreEqual("Password is empty.", output);
            }
        }

        [Test]
        public void AuthenticateUser_WhenPasswordNotMatch_ReturnsPasswordNotMatchErrorMessage()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ILoginBusinessRules>();

                AuthenticateModel input = new AuthenticateModel
                {
                    UserName = "john.doe",
                    Password = "Al1eq36aoBq6K0+pAA==" //encrypted string for 'password'
                };

                String password = "otherPassword";

                Mock<ILoginBusinessRules> mockPassword = new Mock<ILoginBusinessRules>();
                mockPassword.Setup(x => x.DecryptPassword(input.Password)).Returns(password);

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

                var output = app.VerifyValidUser(input);

                Assert.AreEqual("Password does not match.", output);
            }
        }

        [Test]
        public void AuthenticateUser_WhenPasswordMatch_ReturnPasswordMatchSuccess()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ILoginBusinessRules>();

                AuthenticateModel input = new AuthenticateModel
                {
                    UserName = "john.doe",
                    Password = "LTg9BIob8urwz643K5+pBA==" //encrypted string for 'password'
                };

                String password = "password";

                Mock<ILoginBusinessRules> mockPassword = new Mock<ILoginBusinessRules>();
                mockPassword.Setup(x => x.DecryptPassword(input.Password)).Returns(password);

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

                var output = app.VerifyValidUser(input);

                Assert.AreEqual("Password matched.", output);
            }
        }
    }
}
