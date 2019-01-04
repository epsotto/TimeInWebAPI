using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LoginService.Models
{
    [DataContract]
    public class UpdatePasswordModel
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string PreviousPassword { get; set; }

        [DataMember]
        public string NewPassword { get; set; }
    }
}