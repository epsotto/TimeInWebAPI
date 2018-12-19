using LoginService.BusinessLayer;
using LoginService.Interfaces;
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
        public void VerifyEmployeeClockIn_WhenUserNameIsBlankOrNull_ReturnsUserNameError()
        {
            string input = "";

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            timeIn.Setup(x => x.GetEmployeeDailyTimeIn(input)).Returns(new DailyTimeIn());

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object);

            var output = app.VerifyEmployeeClockIn(input);

            Assert.AreEqual("No data being processed.", output.QueryStatus);
        }

        [Test]
        public void VerifyEmployeeClockIn_WhenUserNameIsCorrect_ReturnsSuccessMessage()
        {
            string input = "john.doe";

            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            timeIn.Setup(x => x.GetEmployeeDailyTimeIn(input)).Returns(new DailyTimeIn() {
                TimeInId = 1,
                ActivityCd = 1,
                EmployeeId = 10001,
                IsActive = true,
                TimeInDttm = DateTime.Now,
                CreateUserId = input,
                CreateDttm = DateTime.Now,
                UpdateUserId = input,
                UpdateDttm = DateTime.Now
            });

            ILoginBusinessRules app = new LoginBusinessRules(user.Object, timeIn.Object);

            var output = app.VerifyEmployeeClockIn(input);

            Assert.AreEqual("Query success.", output.QueryStatus);
        }
    }
}
