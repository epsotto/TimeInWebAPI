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
        public IHttpActionResult ClockInEmployee([FromBody]ClockInModel clockIn)
        {
            var container = ContainerConfig.Configure();

            User userQuery = new User();

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

                var activityQuery = app.GetActivitySingle(clockIn.ActivityId);
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
                    ActivityId = 0,//activityQuery.ActivityId,
                    ClockInDateTime = DateTime.Parse(clockIn.ClockInDateTime)
                };

                String clockInResult = app.ClockInEmployee(clockQuery);

                return Json(clockIn);
            }
        }

        public IHttpActionResult ClockOutEmployee([FromBody]ClockOutModel clockIn)
        {

            return Json(clockIn);
        }

        public IHttpActionResult GenerateEmployeeMonthyReport([FromBody]ReportModel employeeReport)
        {
            return Json(employeeReport);
        }
    }
}
