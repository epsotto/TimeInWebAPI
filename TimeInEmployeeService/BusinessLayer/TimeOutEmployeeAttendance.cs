using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeInEmployeeService.Interfaces;
using TimeInRepository.Models;
using TimeInRepository.Utilities;

namespace TimeInEmployeeService.BusinessLayer
{
    public class TimeOutEmployeeAttendance : ITimeOutEmployeeAttendance
    {
        IDailyTimeOutDataAccess _dataAccess;

        public TimeOutEmployeeAttendance(IDailyTimeOutDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public string ClockOutEmployee(ClockOutQueryModel clockOut)
        {
            if (clockOut == null)
            {
                return "No data is being processed.";
            }

            if (string.IsNullOrEmpty(clockOut.UserName) 
                || clockOut.UserId < 10000)
            {
                return "User not found.";
            }

            if (clockOut.ClockOutDateTime < DateTime.Parse("1900-01-01") 
                || clockOut.ClockOutDateTime > DateTime.Now)
            {
                return "Date is out of bounds.";
            }

            if (clockOut.ActivityId == 0)
            {
                return "Activity ID not found.";
            }

            bool isSuccess = _dataAccess.InsertTimeOut(clockOut);

            if (isSuccess)
            {
                return "Employee successfully clocked out.";
            }

            return String.Empty;
        }
    }
}