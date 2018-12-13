using System.Collections.Generic;

namespace TimeInRepository.Utilities
{
    public interface IActivityDataAccess
    {
        string DeleteActivity(int activityId);
        Activity GetActivitySingle(int activityId);
        List<Activity> GetAllActivities();
        string InsertActivity(Activity newActivity);
        string UpdateActivity(int activityId, Activity newActivity);
    }
}