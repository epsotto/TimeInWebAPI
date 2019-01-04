using Autofac;
using LoginService.BusinessLayer;
using LoginService.Interfaces;
using LoginService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using TimeInRepository;

namespace LoginService.Controllers
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        [Route("api/Login/AuthenticateUser")]
        [HttpPost]
        public IHttpActionResult AuthenticateUser([FromBody]AuthenticateModel auth)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ILoginBusinessRules>();
                string verifyResult = app.VerifyValidUser(auth);
                return Json(new { Result = verifyResult });
            }
        }

        [Route("api/Login/VerifyEmployeeClockIn")]
        [HttpPost]
        public IHttpActionResult VerifyEmployeeClockIn([FromBody]VerifyClockInModel model)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ILoginBusinessRules>();
                VerifyClockInResultModel verifyResult = app.VerifyEmployeeClockIn(model);

                return Json(verifyResult);
            }
        }

        [Route("api/Login/UpdateUserPassword")]
        [HttpPost]
        public IHttpActionResult UpdateUserPassword([FromBody]UpdatePasswordModel model)
        {
            var container = ContainerConfig.Configure();

            using(var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ILoginBusinessRules>();
                string updateResult = app.UpdateUserPassword(model);

                return Json(new { Result = updateResult });
            }
        }
    }
}
