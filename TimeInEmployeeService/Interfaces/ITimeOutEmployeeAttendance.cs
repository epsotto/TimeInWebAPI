using TimeInRepository.Models;

namespace TimeInEmployeeService.Interfaces
{
    public interface ITimeOutEmployeeAttendance
    {
        string ClockOutEmployee(ClockOutQueryModel clockOut);
    }
}