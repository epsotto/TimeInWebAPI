using Autofac;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInEmployeeService;
using TimeInEmployeeService.BusinessLayer;
using TimeInEmployeeService.Interfaces;
using TimeInRepository.Models;
using TimeInRepository.Utilities;

namespace TimeInEmployee.Service.Test
{
    [TestFixture]
    public class TimeOutEmployeeAttendanceTest
    {
        [Test]
        public void ClockOutEmployeeTest_WhenEmployeeUserNameIsBlank_ReturnsUserIsBlankError()
        { 
            ClockOutQueryModel input = new ClockOutQueryModel
            {
                UserId = 10001,
                UserName = string.Empty,
                ActivityId = 1,
                ClockOutDateTime = DateTime.Now
            };

            Mock<IDailyTimeOutDataAccess> mock = new Mock<IDailyTimeOutDataAccess>();
            mock.Setup(x => x.InsertTimeOut(input)).Returns(true);

            ITimeOutEmployeeAttendance app = new TimeOutEmployeeAttendance(mock.Object);

            string output = app.ClockOutEmployee(input);

            Assert.AreEqual("User not found.", output);
        }

        [Test]
        public void ClockOutEmployeeTest_WhenEmployeeUserIdIsZero_ReturnsUserNotFoundError()
        {
            ClockOutQueryModel input = new ClockOutQueryModel
            {
                UserId = 0,
                UserName = "john.doe",
                ActivityId = 1,
                ClockOutDateTime = DateTime.Now
            };

            Mock<IDailyTimeOutDataAccess> mock = new Mock<IDailyTimeOutDataAccess>();
            mock.Setup(x => x.InsertTimeOut(input)).Returns(true);

            ITimeOutEmployeeAttendance app = new TimeOutEmployeeAttendance(mock.Object);

            string output = app.ClockOutEmployee(input);

            Assert.AreEqual("User not found.", output);
        }

        [Test]
        public void ClockOutEmployeeTest_WhenReportMonthIsOutOfMinimumBounds_ReturnsReportMonthIsOutOfBoundsError()
        {
            ClockOutQueryModel input = new ClockOutQueryModel
            {
                UserId = 10001,
                UserName = "john.doe",
                ActivityId = 1,
                ClockOutDateTime = DateTime.Parse("1800-01-01")
            };

            Mock<IDailyTimeOutDataAccess> mock = new Mock<IDailyTimeOutDataAccess>();
            mock.Setup(x => x.InsertTimeOut(input)).Returns(true);

            ITimeOutEmployeeAttendance app = new TimeOutEmployeeAttendance(mock.Object);

            string output = app.ClockOutEmployee(input);

            Assert.AreEqual("Date is out of bounds.", output);
        }

        [Test]
        public void ClockOutEmployeeTest_WhenReportMonthIsOutOfMaximumBounds_ReturnsReportMonthIsOutOfBoundsError()
        {
            ClockOutQueryModel input = new ClockOutQueryModel
            {
                UserId = 10001,
                UserName = "john.doe",
                ActivityId = 1,
                ClockOutDateTime = DateTime.Now.AddMonths(1)
            };

            Mock<IDailyTimeOutDataAccess> mock = new Mock<IDailyTimeOutDataAccess>();
            mock.Setup(x => x.InsertTimeOut(input)).Returns(true);

            ITimeOutEmployeeAttendance app = new TimeOutEmployeeAttendance(mock.Object);

            string output = app.ClockOutEmployee(input);

            Assert.AreEqual("Date is out of bounds.", output);
        }

        [Test]
        public void ClockOutEmployeeTest_WhenActivityIdIsZero_ReturnsActivityNotFoundError()
        {
            ClockOutQueryModel input = new ClockOutQueryModel
            {
                UserId = 10001,
                UserName = "john.doe",
                ActivityId = 0,
                ClockOutDateTime = DateTime.Now
            };

            Mock<IDailyTimeOutDataAccess> mock = new Mock<IDailyTimeOutDataAccess>();
            mock.Setup(x => x.InsertTimeOut(input)).Returns(true);

            ITimeOutEmployeeAttendance app = new TimeOutEmployeeAttendance(mock.Object);


            string output = app.ClockOutEmployee(input);

            Assert.AreEqual("Activity ID not found.", output);
        }

        [Test]
        public void ClockOutEmployeeTest_WhenUserNameAndReportDateIsCorrect_ReturnsSuccessMessage()
        { 
            ClockOutQueryModel input = new ClockOutQueryModel
            {
                UserId = 10001,
                UserName = "john.doe",
                ActivityId = 1,
                ClockOutDateTime = DateTime.Now
            };

            Mock<IDailyTimeOutDataAccess> mock = new Mock<IDailyTimeOutDataAccess>();
            mock.Setup(x => x.InsertTimeOut(input)).Returns(true);

            ITimeOutEmployeeAttendance app = new TimeOutEmployeeAttendance(mock.Object);

            string output = app.ClockOutEmployee(input);

            Assert.AreEqual("Employee successfully clocked out.", output);
        }
    }
}
