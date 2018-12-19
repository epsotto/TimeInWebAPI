using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LoginService.Models
{
    [DataContract]
    public class VerifyClockInModel
    {
        [DataMember]
        public bool ClockedIn { get; set; }

        [DataMember]
        public string QueryStatus { get; set; }
    }
}