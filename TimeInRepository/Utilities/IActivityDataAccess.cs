using System.Collections.Generic;
using TimeInRepository.Models;

namespace TimeInRepository.Utilities
{
    public interface IActivityDataAccess
    {
        string DeleteActivity(int activityId);
        Activity GetActivitySingle(int activityId);
        List<ActivityModel> GetAllActivities();
        string InsertActivity(Activity newActivity);
        string UpdateActivity(int activityId, Activity newActivity);
    }
}