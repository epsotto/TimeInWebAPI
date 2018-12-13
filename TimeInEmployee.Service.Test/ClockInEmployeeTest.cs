using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInRepository.Models;
using Moq;
using TimeInRepository;
using TimeInEmployeeService;
using Autofac;
using TimeInEmployeeService.Interfaces;
using TimeInRepository.Utilities;

namespace TimeInEmployee.Service.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ClockInEmployeeTest
    {
        [Test]
        public void ClockInEmployeeTest_WhenEmployeeUserNameIsBlank_ReturnsUserIsBlankError()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();

                ClockInQueryModel clockIn = new ClockInQueryModel
                {
                    UserId = 10001,
                    UserName = String.Empty,
                    ActivityId = 1,
                    ClockInDateTime = DateTime.Now
                };

                String output = app.ClockInEmployee(clockIn);

                Assert.AreEqual("User not found.", output);
            }
        }

        [Test]
        public void ClockInEmployeeTest_WhenEmployeeUserIdIsZero_ReturnsUserNotFoundError()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();

                ClockInQueryModel clockIn = new ClockInQueryModel
                {
                    UserId = 0,
                    UserName = "john.doe",
                    ActivityId = 1,
                    ClockInDateTime = DateTime.Now
                };

                String output = app.ClockInEmployee(clockIn);

                Assert.AreEqual("User not found.", output);
            }
        }

        [Test]
        public void ClockInEmployeeTest_WhenReportMonthIsOutOfMinimumBounds_ReturnsReportMonthIsOutOfBoundsError()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();

                ClockInQueryModel clockIn = new ClockInQueryModel
                {
                    UserId = 10001,
                    UserName = "john.doe",
                    ActivityId = 1,
                    ClockInDateTime = DateTime.Parse("1800-01-01")
                };

                String output = app.ClockInEmployee(clockIn);

                Assert.AreEqual("Date is out of bounds.", output);
            }
        }

        [Test]
        public void ClockInEmployeeTest_WhenReportMonthIsOutOfMaximumBounds_ReturnsReportMonthIsOutOfBoundsError()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();

                ClockInQueryModel clockIn = new ClockInQueryModel
                {
                    UserId = 10001,
                    UserName = "john.doe",
                    ActivityId = 1,
                    ClockInDateTime = DateTime.Now.AddMonths(1)
                };

                String output = app.ClockInEmployee(clockIn);

                Assert.AreEqual("Date is out of bounds.", output);
            }
        }

        [Test]
        public void ClockInEmployeeTest_WhenActivityIdIsZero_ReturnsActivityNotFoundError()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();

                ClockInQueryModel clockIn = new ClockInQueryModel
                {
                    UserId = 10001,
                    UserName = "john.doe",
                    ActivityId = 0,
                    ClockInDateTime = DateTime.Now
                };

                String output = app.ClockInEmployee(clockIn);

                Assert.AreEqual("Activity ID not found.", output);
            }
        }

        [Test]
        public void ClockInEmployeeTest_WhenUserNameAndReportDateIsCorrect_ReturnsSuccessMessage()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();

                ClockInQueryModel clockIn = new ClockInQueryModel
                {
                    UserId = 10001,
                    UserName = "john.doe",
                    ActivityId = 1,
                    ClockInDateTime = DateTime.Now
                };

                Mock<IDailyTimeInDataAccess> mock = new Mock<IDailyTimeInDataAccess>();
                mock.Setup(x => x.InsertTimeIn(clockIn)).Returns(true);

                String output = app.ClockInEmployee(clockIn);

                Assert.AreEqual("Employee successfully clocked in.", output);
            }
        }
    }
}
