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
    public class VerifyEmployeeClockInTest
    {
        [Test]
        public void VerifyEmployeeClockIn_WhenUserNameIsBlank_ReturnsUserNameError()
        {
            VerifyClockInModel input = new VerifyClockInModel
            {
                UserName = ""
            };

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            timeIn.Setup(x => x.GetEmployeeRecentTimeIn(input.UserName)).Returns(new DailyTimeIn());

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.VerifyEmployeeClockIn(input);

            Assert.AreEqual("No data being processed.", output.QueryStatus);
        }

        [Test]
        public void VerifyEmployeeClockIn_WhenUserNameIsNull_ReturnsUserNameError()
        {
            VerifyClockInModel input = new VerifyClockInModel
            {
                UserName = null
            };

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            timeIn.Setup(x => x.GetEmployeeRecentTimeIn(input.UserName)).Returns(new DailyTimeIn());

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.VerifyEmployeeClockIn(input);

            Assert.AreEqual("No data being processed.", output.QueryStatus);
        }

        [Test]
        public void VerifyEmployeeClockIn_WhenUserNameIsCorrect_ReturnsSuccessMessage()
        {
            VerifyClockInModel input = new VerifyClockInModel
            {
                UserName = "john.doe"
            };
            DateTime now = DateTime.Now;

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            timeIn.Setup(x => x.GetEmployeeRecentTimeIn(input.UserName)).Returns(new DailyTimeIn() {
                TimeInId = 1,
                ActivityCd = 1,
                EmployeeId = 10001,
                IsActive = true,
                TimeInDttm = now,
                CreateUserId = input.UserName,
                CreateDttm = now,
                UpdateUserId = input.UserName,
                UpdateDttm = now
            });
            timeOut.Setup(x => x.GetEmployeeDailyTimeOut(input.UserName, now)).Returns(new DailyTimeOut()
            {
                TimeInId = 1,
                ActivityCd = 1,
                EmployeeId = 10001,
                IsActive = true,
                TimeOutDttm = now,
                CreateUserId = input.UserName,
                CreateDttm = now,
                UpdateUserId = input.UserName,
                UpdateDttm = now
            });

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object, timeOut.Object);

            var output = app.VerifyEmployeeClockIn(input);

            Assert.AreEqual("Query success.", output.QueryStatus);
        }
    }
}
