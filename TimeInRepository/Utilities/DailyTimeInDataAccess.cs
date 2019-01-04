using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public List<DailyTimeIn> GetDailyTimeIn(DateTime date)
        {
            List<DailyTimeIn> timeInList = new List<DailyTimeIn>();

            using(TimeInEntities context = new TimeInEntities())
            {
                timeInList = context.DailyTimeIns.Where(x => x.UpdateDttm.Date == date.Date).ToList();
            }

            return timeInList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DailyTimeIn> GetMonthTimeIn(DateTime date)
        {
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
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
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<DailyTimeIn> GetEmployeeMonthTimeIn(int userKey, DateTime month)
        {
            DateTime firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            using (TimeInEntities context = new TimeInEntities())
            {
                var query = context.DailyTimeIns.Include("Activity").Where(x => x.EmployeeId == userKey
                && (x.TimeInDttm > firstDayOfMonth && x.TimeInDttm < lastDayOfMonth)
                && x.IsActive == true)
                    .ToList();

                return query;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DailyTimeIn GetEmployeeRecentTimeIn(string userName)
        {
            using (TimeInEntities context = new TimeInEntities())
            {
                var query = context.DailyTimeIns.Include("Activity").Where(x => x.User.UserName.Equals(userName) 
                && x.IsActive).OrderByDescending(x => x.TimeInDttm).FirstOrDefault();
                return query;
            }
        }
    }
}
