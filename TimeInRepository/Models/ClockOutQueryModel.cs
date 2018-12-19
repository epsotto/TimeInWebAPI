using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInRepository.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ClockOutQueryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ClockOutDateTime { get; set; }
    }
}
