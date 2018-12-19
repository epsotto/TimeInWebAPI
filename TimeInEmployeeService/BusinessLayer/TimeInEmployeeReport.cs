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
        IActivityDataAccess _activityDataAccess;

        public TimeInEmployeeReport(IDailyTimeInDataAccess timeInDataAccess,
            IDailyTimeOutDataAccess timeOutDataAccess,
            IUserDataAccess userDataAccess,
            IActivityDataAccess activityDataAccess)
        {
            _timeInDataAccess = timeInDataAccess;
            _timeOutDataAccess = timeOutDataAccess;
            _userDataAccess = userDataAccess;
            _activityDataAccess = activityDataAccess;
        }

        public GeneratedReportModel GenerateMonthlyReport (ReportQueryModel report)
        {
            DateTime reportDate;
            GeneratedReportModel result = new GeneratedReportModel();

            if(report == null)
            {
                result.QueryStatus = "No data being processed.";
                return result;
            }

            if(report.UserName == "")
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

            if(reportDate < DateTime.Parse("1900-01-01") || reportDate > DateTime.Now)
            {
                result.QueryStatus = "Invalid report date selected.";
                return result;
            }

            var userQuery = _userDataAccess.GetUser(report.UserName);
            if(userQuery == null)
            {
                result.QueryStatus = "Employee not found.";
                return result;
            }

            var timeIn = _timeInDataAccess.GetEmployeeMonthTimeIn(userQuery.UserKey, reportDate);
            var timeOut = _timeOutDataAccess.GetEmployeeMonthTimeOut(userQuery.UserKey, reportDate);

            if (timeIn == null && timeOut == null)
            {
                result.QueryStatus = "Error while fetching data.";
                return result;
            }

            result.TimeList = timeIn.Join(timeOut.DefaultIfEmpty(),
                ti => ti.TimeInDttm,
                to => to.TimeOutDttm,
                (ti, to) => new TimeInTimeOut {
                    ActivityName = ti.Activity.ActivityNm,
                    TimeInDateTime = ti.TimeInDttm,
                    TImeOutDateTime = to.TimeOutDttm
                }).ToList();

            result.QueryStatus = "Report generated.";

            return result;
        }
    }
}