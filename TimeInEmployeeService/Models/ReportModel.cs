using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TimeInEmployeeService.Models
{
    [DataContract]
    public class ReportModel
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string ReportDate { get; set; }
    }
}