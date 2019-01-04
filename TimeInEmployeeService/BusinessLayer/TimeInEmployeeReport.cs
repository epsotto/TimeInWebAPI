using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeInEmployeeService.Interfaces;
using TimeInEmployeeService.Models;
using TimeInRepository.Utilities;

namespace TimeInEmployeeService.BusinessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeInEmployeeReport : ITimeInEmployeeReport
    {
        IDailyTimeInDataAccess _timeInDataAccess;
        IDailyTimeOutDataAccess _timeOutDataAccess;
        IUserDataAccess _userDataAccess;

        public TimeInEmployeeReport(IDailyTimeInDataAccess timeInDataAccess,
            IDailyTimeOutDataAccess timeOutDataAccess,
            IUserDataAccess userDataAccess)
        {
            _timeInDataAccess = timeInDataAccess;
            _timeOutDataAccess = timeOutDataAccess;
            _userDataAccess = userDataAccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public GeneratedReportModel GenerateMonthlyReport(ReportQueryModel report)
        {
            DateTime reportDate;
            GeneratedReportModel result = new GeneratedReportModel();

            if (report == null)
            {
                result.QueryStatus = "No data being processed.";
                return result;
            }

            if (report.UserName == "")
            {
                result.QueryStatus = "Employee username is blank.";
                return result;
            }

            bool dateStringCheck = DateTime.TryParse(report.ReportDate, out reportDate);
            if (!dateStringCheck)
            {
                result.QueryStatus = "Invalid report date selected.";
                return result;
            }

            if (reportDate < DateTime.Parse("1900-01-01") || reportDate > DateTime.Now)
            {
                result.QueryStatus = "Invalid report date selected.";
                return result;
            }

            var userQuery = _userDataAccess.GetUser(report.UserName);
            if (userQuery == null)
            {
                result.QueryStatus = "Employee not found.";
                return result;
            }

            var timeIn = _timeInDataAccess.GetEmployeeMonthTimeIn(userQuery.UserKey, reportDate);
            var timeOut = _timeOutDataAccess.GetEmployeeMonthTimeOut(userQuery.UserKey, reportDate);

            if (timeIn.Count == 0 && timeOut.Count == 0)
            {
                result.QueryStatus = "No data available.";
                return result;
            }

            if (timeIn.Count == 0)
            {
                result.TimeList = timeOut.Select(x => new TimeInTimeOut
                {
                    ActivityName = x.Activity.ActivityNm,
                    TImeOutDateTime = x.TimeOutDttm.ToString(),
                    TimeInDateTime = null
                }).ToList();

                result.QueryStatus = "Report generated.";

                return result;
            }
            else if (timeOut.Count == 0)
            {
                result.TimeList = timeIn.Select(x => new TimeInTimeOut
                {
                    ActivityName = x.Activity.ActivityNm,
                    TImeOutDateTime = null,
                    TimeInDateTime = x.TimeInDttm.ToString()
                }).ToList();

                result.QueryStatus = "Report generated.";

                return result;
            }
            else
            {
                //result.TimeList = timeIn.Join(timeOut.DefaultIfEmpty(),
                //    ti => ti.TimeInDttm.Date,
                //    to => to.TimeOutDttm.Date,
                //    (ti, to) => new TimeInTimeOut
                //    {
                //        ActivityName = ti.Activity.ActivityNm,
                //        TimeInDateTime = ti.TimeInDttm,
                //        TImeOutDateTime = to.TimeOutDttm
                //    }).ToList();

                result.TimeList = (from ti in timeIn
                                   join to in timeOut.DefaultIfEmpty()
                                   on ti.TimeInDttm.Date equals to.TimeOutDttm.Date into tito
                                   from to in tito.DefaultIfEmpty()
                                   select new TimeInTimeOut
                                   {
                                       ActivityName = ti.Activity.ActivityNm,
                                       TimeInDateTime = ti.TimeInDttm.ToString(),
                                       TImeOutDateTime = to?.TimeOutDttm.ToString()
                                   }
                                   ).ToList();

                result.QueryStatus = "Report generated.";

                return result;
            }
        }
    }
}