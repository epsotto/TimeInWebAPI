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

        public string ClockInEmployee(ClockInQueryModel clockIn)
        {
            if (clockIn == null)
            {
                return "No data is being processed.";
            }

            if (string.IsNullOrEmpty(clockIn.UserName) 
                || clockIn.UserId < 10000)
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

            var recentTimeIn = _dataAccess.GetEmployeeRecentTimeIn(clockIn.UserName);

            if(recentTimeIn.TimeInDttm.Date == clockIn.ClockInDateTime.Date)
            {
                return "Already clocked in for the day.";
            }

            bool isSuccess = _dataAccess.InsertTimeIn(clockIn);

            if (isSuccess)
            {
                return "Employee successfully clocked in.";
            }

            return string.Empty;
        }
    }
}