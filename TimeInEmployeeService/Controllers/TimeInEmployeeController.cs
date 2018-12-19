using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeInEmployeeService.BusinessLayer;
using TimeInEmployeeService.Interfaces;
using TimeInEmployeeService.Models;
using TimeInRepository;
using TimeInRepository.Models;
using TimeInRepository.Utilities;

namespace TimeInEmployeeService.Controllers
{
    public class TimeInEmployeeController : ApiController
    {
        [Route("api/TimeInEmployee/ClockInEmployee")]
        [HttpPost]
        public IHttpActionResult ClockInEmployee([FromBody]ClockInModel clockIn)
        {
            var container = ContainerConfig.Configure();

            User userQuery = new User();
            Activity activityQuery = new Activity();

            if (clockIn == null)
                {
                    return Json(new { Result = "Internal server error.", StatusCode = HttpStatusCode.InternalServerError });
                }

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IUserDataAccess>();

                userQuery = app.GetUser(clockIn.UserName);

                if (userQuery == null)
                {
                    return Json(new { Result = "Employee record not found." });
                }
            }

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IActivityDataAccess>();

                activityQuery = app.GetActivitySingle(clockIn.ActivityId);
                if (activityQuery == null)
                {
                    return Json(new { Result = "Activity selected is unknown." });
                }
            }

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();
                ClockInQueryModel clockQuery = new ClockInQueryModel
                {
                    UserId = userQuery.UserKey,
                    UserName = clockIn.UserName,
                    ActivityId = activityQuery.ActivityId,
                    ClockInDateTime = DateTime.Parse(clockIn.ClockInDateTime)
                };

                string clockInResult = app.ClockInEmployee(clockQuery);

                return Json(clockInResult);
            }
        }

        [Route("api/TimeInEmployee/ClockOutEmployee")]
        [HttpPost]
        public IHttpActionResult ClockOutEmployee([FromBody]ClockOutModel clockOut)
        {
            var container = ContainerConfig.Configure();

            User userQuery = new User();
            Activity activityQuery = new Activity();

            if (clockOut == null)
            {
                return Json(new { Result = "Internal server error.", StatusCode = HttpStatusCode.InternalServerError });
            }

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IUserDataAccess>();

                userQuery = app.GetUser(clockOut.UserName);

                if (userQuery == null)
                {
                    return Json(new { Result = "Employee record not found." });
                }
            }

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IActivityDataAccess>();

                activityQuery = app.GetActivitySingle(clockOut.ActivityId);
                if (activityQuery == null)
                {
                    return Json(new { Result = "Activity selected is unknown." });
                }
            }

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeAttendance>();
                ClockInQueryModel clockQuery = new ClockInQueryModel
                {
                    UserId = userQuery.UserKey,
                    UserName = clockOut.UserName,
                    ActivityId = activityQuery.ActivityId,
                    ClockInDateTime = DateTime.Parse(clockOut.ClockOutDateTime)
                };

                string clockInResult = app.ClockInEmployee(clockQuery);

                return Json(clockInResult);
            }
        }

        [Route("api/TimeInEmployee/GenerateEmployeeMonthyReport")]
        [HttpPost]
        public IHttpActionResult GenerateEmployeeMonthyReport([FromBody]ReportQueryModel employeeReport)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ITimeInEmployeeReport>();

                ReportQueryModel getReport = new ReportQueryModel()
                {
                    ReportDate = employeeReport.ReportDate,
                    UserName = employeeReport.UserName
                };

                GeneratedReportModel result = app.GenerateMonthlyReport(getReport);

                return Json(result);
            }
        }
    }
}
