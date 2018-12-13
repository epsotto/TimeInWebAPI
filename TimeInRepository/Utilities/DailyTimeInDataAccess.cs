using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInRepository.Models;

namespace TimeInRepository.Utilities
{
    /// <summary>
    /// Data controller for Daily TimeIn table.
    /// </summary>
    public class DailyTimeInDataAccess : IDailyTimeInDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DailyTimeIn> GetDailyTimeIn(string date)
        {
            List<DailyTimeIn> timeInList = new List<DailyTimeIn>();

            using(TimeInEntities context = new TimeInEntities())
            {
                timeInList = context.DailyTimeIns.Where(x => x.UpdateDttm.Date == DateTime.Parse(date).Date).ToList();
            }

            return timeInList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DailyTimeIn> GetMonthTimeIn(string date)
        {
            DateTime currentDate = DateTime.Parse(date);
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            List<DailyTimeIn> timeInList = new List<DailyTimeIn>();

            using (TimeInEntities context = new TimeInEntities())
            {
                timeInList = context.DailyTimeIns.Where(x => x.CreateDttm > firstDayOfMonth && x.CreateDttm < lastDayOfMonth).ToList();
            }

            return timeInList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="activityId"></param>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        public bool InsertTimeIn(ClockInQueryModel clockIn)
        {
            DailyTimeIn newRecord = new DailyTimeIn();

            try
            {
                using(TimeInEntities context = new TimeInEntities())
                {
                    newRecord.EmployeeId = clockIn.UserId;
                    newRecord.ActivityCd = clockIn.ActivityId;
                    newRecord.TimeInDttm = clockIn.ClockInDateTime;
                    newRecord.IsActive = true;
                    newRecord.CreateDttm = DateTime.Now;
                    newRecord.CreateUserId = clockIn.UserName;
                    newRecord.UpdateDttm = DateTime.Now;
                    newRecord.UpdateUserId = clockIn.UserName;

                    context.DailyTimeIns.Add(newRecord);
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
