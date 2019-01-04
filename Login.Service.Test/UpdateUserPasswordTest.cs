using LoginService.BusinessLayer;
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
    [TestFixture]
    public class UpdateUserPasswordTest
    {
        [Test]
        public void UpdateUserPassword_WhenInputModelIsBlank_ReturnsNoDataToProcessErrorMessage()
        {
            UpdatePasswordModel input = null;

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.UpdateUserPassword(input);

            Assert.AreEqual("No data to process.", output);
        }

        [Test]
        public void UpdateUserPassword_WhenUserNameIsBlankOrNull_ReturnsUserNameIsEmptyPassword([Values ("", null)] string userName)
        {
            UpdatePasswordModel input = new UpdatePasswordModel()
            {
                UserName = userName,
                NewPassword = "LTg9BIob8upybfUeFvpKXA==", //password1
                PreviousPassword = "LTg9BIob8urwz643K5+pBA==" //password
            };

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.UpdateUserPassword(input);

            Assert.AreEqual("Username is empty.", output);
        }

        [Test]
        public void UpdateUserPassword_WhenNewPasswordIsBlankOrNull_ReturnsUserNameIsEmptyPassword([Values("", null)] string newPassword)
        {
            UpdatePasswordModel input = new UpdatePasswordModel()
            {
                UserName = "john.doe",
                NewPassword = newPassword,
                PreviousPassword = "LTg9BIob8urwz643K5+pBA==" //password
            };

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.UpdateUserPassword(input);

            Assert.AreEqual("New Password is empty.", output);
        }

        [Test]
        public void UpdateUserPassword_WhenPreviousPasswordIsBlankOrNull_ReturnsUserNameIsEmptyPassword([Values("", null)] string previousPassword)
        {
            UpdatePasswordModel input = new UpdatePasswordModel()
            {
                UserName = "john.doe",
                NewPassword = "LTg9BIob8upybfUeFvpKXA==", //password1
                PreviousPassword = previousPassword
            };

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.UpdateUserPassword(input);

            Assert.AreEqual("Previous Password is empty.", output);
        }

        [Test]
        public void UpdateUserPassword_WhenPreviousPasswordDoesNotMatchInRecords_ReturnIncorrectPasswordError()
        {
            UpdatePasswordModel input = new UpdatePasswordModel()
            {
                UserName = "john.doe",
                NewPassword = "LTg9BIob8upybfUeFvpKXA==", //password1
                PreviousPassword = "LTg9BIob8urwz643K5+pBA==" //password
            };

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();

            user.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserName = "john.doe",
                UserPassword = "LTg9BIob8upybfUeFvpKXA=="
            });

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.UpdateUserPassword(input);

            Assert.AreEqual("Previous password does not match.", output);
        }

        [Test]
        public void UpdateUserPassword_WhenInputsAreCorrect_ReturnsQuerySuccessMessage()
        {
            UpdatePasswordModel input = new UpdatePasswordModel()
            {
                UserName = "john.doe",
                NewPassword = "password1", //password1
                PreviousPassword = "password" //password
            };

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();

            user.Setup(x => x.UpdatePassword(10000, It.IsAny<string>())).Returns("User record updated.");
            user.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 10000,
                UserName = "john.doe",
                UserPassword = "LTg9BIob8urwz643K5+pBA=="
            });

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.UpdateUserPassword(input);

            Assert.AreEqual("User record updated.", output);
        }
    }
}
