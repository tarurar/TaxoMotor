using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace TM.SP.Ratings.Timers
{
    public class RatingBaseFeatureReceiver : SPFeatureReceiver
    {
        protected bool CreateJob(SPWebApplication site, string name, string title, SPSchedule schedule, Type type)
        {
            bool jobCreated = false;
            try
            {
                var baseType = typeof(RatingBaseJobDefinition);

                if (!type.IsSubclassOf(baseType))
                    throw new Exception(String.Format("Job of class {0} must be a subclass of {1}", type.FullName, baseType.FullName));

                var job = (RatingBaseJobDefinition)Activator.CreateInstance(type, name, title, site);
                job.Schedule = schedule;

                job.Update();
                job.Register();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Couldn't create timer job definition for {0}. Details: {1}", title, ex.Message));
            }

            return jobCreated;
        }
        protected bool DeleteExistingJob(string jobName, SPWebApplication site)
        {
            bool jobDeleted = false;
            try
            {
                foreach (SPJobDefinition job in site.JobDefinitions)
                {
                    if (job.Name == jobName)
                    {
                        var ratingJob = job as RatingBaseJobDefinition;
                        if (ratingJob != null)
                            ratingJob.UnRegister();
                        job.Delete();
                        jobDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Couldn't delete timer job definition for {0}. Details: {1}", jobName, ex.Message));
            }
            return jobDeleted;
        }
    }
}
