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
                String verifyResult = app.VerifyValidUser(auth);
                return Json(new { Result = verifyResult });
            }
        }
    }
}
