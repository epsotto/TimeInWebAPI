using TimeInRepository.Models;

namespace TimeInEmployeeService.Interfaces
{
    public interface ITimeInEmployeeAttendance
    {
        string ClockInEmployee(ClockInQueryModel clockIn);
    }
}