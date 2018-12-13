using System.Collections.Generic;

namespace TimeInRepository.Utilities
{
    interface IDailyTimeOutDataAccess
    {
        List<DailyTimeOut> GetDailyTimeOut(string date);
        List<DailyTimeOut> GetMonthTimeIn(string date);
        string InsertTimeIn(int userId, int activityId, string employeeName);
    }
}