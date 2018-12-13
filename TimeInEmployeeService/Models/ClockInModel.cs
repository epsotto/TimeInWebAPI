using System.Runtime.Serialization;

namespace TimeInEmployeeService.Models
{
    [DataContract]
    public class ClockInModel
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int ActivityId { get; set; }

        [DataMember]
        public string ClockInDateTime { get; set; }
    }
}