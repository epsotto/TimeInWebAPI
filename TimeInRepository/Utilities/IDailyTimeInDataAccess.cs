using System.Collections.Generic;
using TimeInRepository.Models;

namespace TimeInRepository.Utilities
{
    public interface IDailyTimeInDataAccess
    {
        List<DailyTimeIn> GetDailyTimeIn(string date);
        List<DailyTimeIn> GetMonthTimeIn(string date);
        bool InsertTimeIn(ClockInQueryModel clockIn);
    }
}