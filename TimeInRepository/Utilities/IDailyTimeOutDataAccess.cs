using System;
using System.Collections.Generic;
using TimeInRepository.Models;

namespace TimeInRepository.Utilities
{
    public interface IDailyTimeOutDataAccess
    {
        List<DailyTimeOut> GetDailyTimeOut(DateTime date);
        List<DailyTimeOut> GetMonthTimeOut(DateTime date);
        bool InsertTimeOut(ClockOutQueryModel clockOut);
        List<DailyTimeOut> GetEmployeeMonthTimeOut(int userKey, DateTime month);
        DailyTimeOut GetEmployeeDailyTimeOut(string userName);
    }
}