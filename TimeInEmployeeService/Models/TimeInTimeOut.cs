using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TimeInRepository;

namespace TimeInEmployeeService.Models
{
    [DataContract]
    public class TimeInTimeOut
    {
        [DataMember]
        public string ActivityName { get; set; }

        [DataMember]
        public DateTime TimeInDateTime { get; set; }

        [DataMember]
        public DateTime TImeOutDateTime { get; set; }
    }
}