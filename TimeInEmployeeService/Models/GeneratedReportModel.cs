using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using TimeInRepository;

namespace TimeInEmployeeService.Models
{
    [DataContract]
    public class GeneratedReportModel
    {
        [DataMember]
        public List<TimeInTimeOut> TimeList { get; set; }

        [DataMember]
        public string QueryStatus { get; set; }
    }
}