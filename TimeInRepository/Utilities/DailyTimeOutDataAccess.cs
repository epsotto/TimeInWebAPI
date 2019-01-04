using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInRepository.Models;

namespace TimeInRepository.Utilities
{
    class DailyTimeOutDataAccess : IDailyTimeOutDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DailyTimeOut> GetDailyTimeOut(DateTime date)
        {
            List<DailyTimeOut> timeOutList = new List<DailyTimeOut>();

            using (TimeInEntities context = new TimeInEntities())
            {
                timeOutList = context.DailyTimeOuts.Where(x => x.UpdateDttm.Date == date.Date).ToList();
            }

            return timeOutList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DailyTimeOut> GetMonthTimeOut(DateTime date)
        {
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            List<DailyTimeOut> timeOutList = new List<DailyTimeOut>();

            using (TimeInEntities context = new TimeInEntities())
            {
                timeOutList = context.DailyTimeOuts.Where(x => x.CreateDttm > firstDayOfMonth && x.CreateDttm < lastDayOfMonth).ToList();
            }

            return timeOutList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="activityId"></param>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        public bool InsertTimeOut(ClockOutQueryModel clockOut)
        {
            DailyTimeOut newRecord = new DailyTimeOut();

            try
            {
                using (TimeInEntities context = new TimeInEntities())
                {
                    newRecord.EmployeeId = clockOut.UserId;
                    newRecord.ActivityCd = clockOut.ActivityId;
                    newRecord.TimeOutDttm = clockOut.ClockOutDateTime;
                    newRecord.IsActive = true;
                    newRecord.CreateDttm = DateTime.Now;
                    newRecord.CreateUserId = clockOut.UserName;
                    newRecord.UpdateDttm = DateTime.Now;
                    newRecord.UpdateUserId = clockOut.UserName;

                    context.DailyTimeOuts.Add(newRecord);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
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
        public List<DailyTimeOut> GetEmployeeMonthTimeOut(int userKey, DateTime month)
        {
            DateTime firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            using (TimeInEntities context = new TimeInEntities())
            {
                var query = context.DailyTimeOuts.Where(x => x.EmployeeId == userKey
                && (x.TimeOutDttm > firstDayOfMonth && x.TimeOutDttm < lastDayOfMonth)
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
        public DailyTimeOut GetEmployeeDailyTimeOut(string userName, DateTime timeIn)
        {
            using (TimeInEntities context = new TimeInEntities())
            {
                var query = context.DailyTimeOuts.Include("Activity").Where(x => x.User.UserName.Equals(userName)
                && x.IsActive && DbFunctions.TruncateTime(x.TimeOutDttm) == timeIn.Date).FirstOrDefault();
                return query;
            }
        }
    }
}
