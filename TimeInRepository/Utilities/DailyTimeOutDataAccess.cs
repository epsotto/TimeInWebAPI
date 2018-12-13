using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInRepository.Utilities
{
    class DailyTimeOutDataAccess : IDailyTimeOutDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DailyTimeOut> GetDailyTimeOut(string date)
        {
            List<DailyTimeOut> timeOutList = new List<DailyTimeOut>();

            using (TimeInEntities context = new TimeInEntities())
            {
                timeOutList = context.DailyTimeOuts.Where(x => x.UpdateDttm.Date == DateTime.Parse(date).Date).ToList();
            }

            return timeOutList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DailyTimeOut> GetMonthTimeIn(string date)
        {
            DateTime currentDate = DateTime.Parse(date);
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
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
        public String InsertTimeIn(int userId, int activityId, String employeeName)
        {
            DailyTimeOut newRecord = new DailyTimeOut();

            try
            {
                using (TimeInEntities context = new TimeInEntities())
                {
                    newRecord.EmployeeId = userId;
                    newRecord.ActivityCd = activityId;
                    newRecord.TimeOutDttm = DateTime.Now;
                    newRecord.IsActive = true;
                    newRecord.CreateDttm = DateTime.Now;
                    newRecord.CreateUserId = employeeName;
                    newRecord.UpdateDttm = DateTime.Now;
                    newRecord.UpdateUserId = employeeName;

                    context.DailyTimeOuts.Add(newRecord);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Daily Time In record created.";
        }
    }
}
