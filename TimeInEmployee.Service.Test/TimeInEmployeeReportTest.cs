using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInEmployeeService.BusinessLayer;
using TimeInEmployeeService.Interfaces;
using TimeInEmployeeService.Models;
using TimeInRepository;
using TimeInRepository.Utilities;

namespace TimeInEmployee.Service.Test
{
    [TestFixture]
    public class TimeInEmployeeReportTest
    {
        [Test]
        public void TimeInReport_WhenUserNameIsBlank_ReturnsUserIsNameBlankError()
        {
            ReportQueryModel input = new ReportQueryModel()
            {
                UserName = "",
                ReportDate = DateTime.Now.ToString()
            };

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IActivityDataAccess> activity = new Mock<IActivityDataAccess>();

            timeIn.Setup(x => x.GetMonthTimeIn(DateTime.Now)).Returns(new List<DailyTimeIn>());
            timeOut.Setup(x => x.GetMonthTimeOut(DateTime.Now)).Returns(new List<DailyTimeOut>());
            user.Setup(x => x.GetUser(input.UserName)).Returns(new User());
            activity.Setup(x => x.GetActivitySingle(1)).Returns(new Activity());

            ITimeInEmployeeReport app = new TimeInEmployeeReport(timeIn.Object, timeOut.Object, user.Object, activity.Object);

            var output = app.GenerateMonthlyReport(input);

            Assert.AreEqual("Employee username is blank.", output.QueryStatus);
        }

        [Test]
        public void TimeInReport_WhenEmployeeIsNotFound_ReturnsEmployeeNotFoundError()
        {
            ReportQueryModel input = new ReportQueryModel()
            {
                UserName = "john.doe",
                ReportDate = DateTime.Now.ToString()
            };

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IActivityDataAccess> activity = new Mock<IActivityDataAccess>();

            timeIn.Setup(x => x.GetMonthTimeIn(DateTime.Now)).Returns(new List<DailyTimeIn>());
            timeOut.Setup(x => x.GetMonthTimeOut(DateTime.Now)).Returns(new List<DailyTimeOut>());
            user.Setup(x => x.GetUser(input.UserName)).Returns((User)null);
            activity.Setup(x => x.GetActivitySingle(1)).Returns(new Activity());

            ITimeInEmployeeReport app = new TimeInEmployeeReport(timeIn.Object, timeOut.Object, user.Object, activity.Object);

            var output = app.GenerateMonthlyReport(input);

            Assert.AreEqual("Employee not found.", output.QueryStatus);
        }

        [Test]
        public void TimeInReport_ReportDateIsNotValidDate_ReturnsReportDateIncorrectError()
        {
            ReportQueryModel input = new ReportQueryModel()
            {
                UserName = "john.doe",
                ReportDate = "incorrect date"
            };

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IActivityDataAccess> activity = new Mock<IActivityDataAccess>();

            timeIn.Setup(x => x.GetMonthTimeIn(DateTime.Now)).Returns(new List<DailyTimeIn>());
            timeOut.Setup(x => x.GetMonthTimeOut(DateTime.Now)).Returns(new List<DailyTimeOut>());
            user.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 1,
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                UserName = "john.doe",
                UserPassword = "!@Aavq113A15+==",
                CreateUserId = "john.doe",
                CreateDttm = DateTime.Now,
                UpdateUserId = "john.doe",
                UpdateDttm = DateTime.Now
            });
            activity.Setup(x => x.GetActivitySingle(1)).Returns(new Activity());

            ITimeInEmployeeReport app = new TimeInEmployeeReport(timeIn.Object, timeOut.Object, user.Object, activity.Object);

            var output = app.GenerateMonthlyReport(input);

            Assert.AreEqual("Invalid report date selected.", output.QueryStatus);
        }

        [Test]
        public void TimeInReport_ReportDateIsBeyondMinimumRangeValidDate_ReturnsReportDateIncorrectError()
        {
            ReportQueryModel input = new ReportQueryModel()
            {
                UserName = "john.doe",
                ReportDate = "1800-01-01"
            };

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IActivityDataAccess> activity = new Mock<IActivityDataAccess>();

            timeIn.Setup(x => x.GetMonthTimeIn(DateTime.Now)).Returns(new List<DailyTimeIn>());
            timeOut.Setup(x => x.GetMonthTimeOut(DateTime.Now)).Returns(new List<DailyTimeOut>());
            user.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 1,
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                UserName = "john.doe",
                UserPassword = "!@Aavq113A15+==",
                CreateUserId = "john.doe",
                CreateDttm = DateTime.Now,
                UpdateUserId = "john.doe",
                UpdateDttm = DateTime.Now
            });
            activity.Setup(x => x.GetActivitySingle(1)).Returns(new Activity());

            ITimeInEmployeeReport app = new TimeInEmployeeReport(timeIn.Object, timeOut.Object, user.Object, activity.Object);

            var output = app.GenerateMonthlyReport(input);

            Assert.AreEqual("Invalid report date selected.", output.QueryStatus);
        }

        [Test]
        public void TimeInReport_ReportDateIsBeyondCurrentDate_ReturnsReportDateIncorrectError()
        {
            ReportQueryModel input = new ReportQueryModel()
            {
                UserName = "john.doe",
                ReportDate = "1800-01-01"
            };

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IActivityDataAccess> activity = new Mock<IActivityDataAccess>();

            timeIn.Setup(x => x.GetMonthTimeIn(DateTime.Now)).Returns(new List<DailyTimeIn>());
            timeOut.Setup(x => x.GetMonthTimeOut(DateTime.Now)).Returns(new List<DailyTimeOut>());
            user.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 1,
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                UserName = "john.doe",
                UserPassword = "!@Aavq113A15+==",
                CreateUserId = "john.doe",
                CreateDttm = DateTime.Now,
                UpdateUserId = "john.doe",
                UpdateDttm = DateTime.Now
            });
            activity.Setup(x => x.GetActivitySingle(1)).Returns(new Activity());

            ITimeInEmployeeReport app = new TimeInEmployeeReport(timeIn.Object, timeOut.Object, user.Object, activity.Object);

            var output = app.GenerateMonthlyReport(input);

            Assert.AreEqual("Invalid report date selected.", output.QueryStatus);
        }

        [Test]
        public void TimeInReport_ReportQueryIscorrect_ReturnsReportGeneratedSuccessMessage()
        {
            ReportQueryModel input = new ReportQueryModel()
            {
                UserName = "john.doe",
                ReportDate = DateTime.Now.ToString()
            };

            Mock<IDailyTimeInDataAccess> timeIn = new Mock<IDailyTimeInDataAccess>();
            Mock<IDailyTimeOutDataAccess> timeOut = new Mock<IDailyTimeOutDataAccess>();
            Mock<IUserDataAccess> user = new Mock<IUserDataAccess>();
            Mock<IActivityDataAccess> activity = new Mock<IActivityDataAccess>();

            timeIn.Setup(x => x.GetEmployeeMonthTimeIn(1, DateTime.Parse(input.ReportDate))).Returns(new List<DailyTimeIn>() {
                new DailyTimeIn
                {
                    TimeInId = 1,
                    ActivityCd = 1,
                    EmployeeId = 10001,
                    IsActive = true,
                    TimeInDttm = DateTime.Parse(input.ReportDate),
                    CreateUserId = "john.doe",
                    CreateDttm = DateTime.Parse(input.ReportDate),
                    UpdateUserId = "john.doe",
                    UpdateDttm = DateTime.Parse(input.ReportDate)
                }
            });
            timeOut.Setup(x => x.GetEmployeeMonthTimeOut(1, DateTime.Parse(input.ReportDate))).Returns(new List<DailyTimeOut>() {
                new DailyTimeOut
                {
                    TimeInId = 1,
                    ActivityCd = 1,
                    EmployeeId = 10001,
                    IsActive = true,
                    TimeOutDttm = DateTime.Parse(input.ReportDate),
                    CreateUserId = "john.doe",
                    CreateDttm = DateTime.Parse(input.ReportDate),
                    UpdateUserId = "john.doe",
                    UpdateDttm = DateTime.Parse(input.ReportDate)
                }
            });
            user.Setup(x => x.GetUser(input.UserName)).Returns(new User()
            {
                UserKey = 1,
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                UserName = "john.doe",
                UserPassword = "!@Aavq113A15+==",
                CreateUserId = "john.doe",
                CreateDttm = DateTime.Now,
                UpdateUserId = "john.doe",
                UpdateDttm = DateTime.Now
            });
            activity.Setup(x => x.GetActivitySingle(1)).Returns(new Activity() {
                ActivityId = 1,
                ActivityNm = "Activity",
                IsActive = true,
                CreateUserId = "Admin",
                CreateDttm = DateTime.Now,
                UpdateUserId = "Admin",
                UpdateDttm = DateTime.Now
            });

            ITimeInEmployeeReport app = new TimeInEmployeeReport(timeIn.Object, timeOut.Object, user.Object, activity.Object);

            var output = app.GenerateMonthlyReport(input);

            Assert.AreEqual("Report generated.", output.QueryStatus);
        }
    }
}
