using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;

namespace LoginService.Models
{
    [DataContract]
    public class AuthenticateModel
    {
        [DataMember]
        public String UserName { get; set; }

        [DataMember]
        public String Password { get; set; }
    }
}