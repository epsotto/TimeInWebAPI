using TimeInEmployeeService.Models;

namespace TimeInEmployeeService.Interfaces
{
    public interface ITimeInEmployeeReport
    {
        GeneratedReportModel GenerateMonthlyReport(ReportQueryModel report);
    }
}