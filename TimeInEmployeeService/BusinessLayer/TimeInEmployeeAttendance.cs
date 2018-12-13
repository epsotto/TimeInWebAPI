using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeInEmployeeService.Interfaces;
using TimeInRepository.Models;
using TimeInRepository.Utilities;

namespace TimeInEmployeeService.BusinessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeInEmployeeAttendance : ITimeInEmployeeAttendance
    {
        IDailyTimeInDataAccess _dataAccess;

        public TimeInEmployeeAttendance(IDailyTimeInDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public String ClockInEmployee(ClockInQueryModel clockIn)
        {
            if (clockIn == null)
            {
                return "No data is being processed.";
            }

            if (String.IsNullOrEmpty(clockIn.UserName) || clockIn.UserId < 10000)
            {
                return "User not found.";
            }
            
            if (clockIn.ClockInDateTime < DateTime.Parse("1900-01-01") 
                || clockIn.ClockInDateTime > DateTime.Now)
            {
                return "Date is out of bounds.";
            }

            if(clockIn.ActivityId == 0)
            {
                return "Activity ID not found.";
            }

            bool isSuccess = _dataAccess.InsertTimeIn(clockIn);
            return isSuccess.ToString();
            if (isSuccess)
            {
                return "Employee successfully clocked in.";
            }

            return String.Empty;
        }
    }
}