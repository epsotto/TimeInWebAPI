using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInRepository.Utilities
{
    /// <summary>
    /// Data controller for Activity table.
    /// </summary>
    public class ActivityDataAccess : IActivityDataAccess
    {
        /// <summary>
        ///  Gets all active Activities from database.
        /// </summary>
        /// <returns></returns>
        public List<Activity> GetAllActivities()
        {
            List<Activity> activities = new List<Activity>();

            using (TimeInEntities context = new TimeInEntities())
            {
                activities = context.Activities.Where(x => x.IsActive == true).ToList();
            }

            return activities;
        }

        /// <summary>
        /// Gets specific Activity from Activity table.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns>Activity object</returns>
        public Activity GetActivitySingle(int activityId)
        {
            Activity activityItem = new Activity();

            using(TimeInEntities context = new TimeInEntities())
            {
                activityItem = context.Activities.Where(x => x.ActivityId == activityId && x.IsActive == true).FirstOrDefault();
            }

            return activityItem;
        }

        /// <summary>
        /// Inserts new Acitivity to the database.
        /// </summary>
        /// <param name="newActivity"></param>
        /// <returns></returns>
        public String InsertActivity(Activity newActivity)
        {
            try
            {
                using (TimeInEntities context = new TimeInEntities())
                {
                    context.Activities.Add(newActivity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Activity saved.";
        }

        /// <summary>
        /// Updates existing Activity.
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="newActivity"></param>
        /// <returns></returns>
        public String UpdateActivity(int activityId, Activity newActivity)
        {
            Activity activityQuery = new Activity();

            try
            {
                using (TimeInEntities context = new TimeInEntities())
                {
                    activityQuery = context.Activities
                        .Where(x => x.ActivityId == activityId && x.IsActive == true).FirstOrDefault();

                    activityQuery.ActivityNm = newActivity.ActivityNm;
                    activityQuery.UpdateDttm = DateTime.Now;

                    context.Activities.Attach(activityQuery);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Activity updated.";
        }

        /// <summary>
        /// Deactivates specified query. Application will not do hard delete.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public String DeleteActivity(int activityId)
        {
            try
            {
                using (TimeInEntities context = new TimeInEntities())
                {
                    var activityQuery = context.Activities
                        .Where(x => x.ActivityId == activityId).FirstOrDefault();

                    activityQuery.IsActive = false;
                    context.Activities.Attach(activityQuery);
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return "Activity deleted.";
        }
    }
}
