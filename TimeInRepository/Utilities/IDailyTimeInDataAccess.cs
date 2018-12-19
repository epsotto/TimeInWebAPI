using System;
using System.Collections.Generic;
using TimeInRepository.Models;

namespace TimeInRepository.Utilities
{
    public interface IDailyTimeInDataAccess
    {
        List<DailyTimeIn> GetDailyTimeIn(DateTime date);
        List<DailyTimeIn> GetMonthTimeIn(DateTime date);
        bool InsertTimeIn(ClockInQueryModel clockIn);
        List<DailyTimeIn> GetEmployeeMonthTimeIn(int userKey, DateTime month);
        DailyTimeIn GetEmployeeDailyTimeIn(string userName);
    }
}