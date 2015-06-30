using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Utils.TimerJobs
{
    public static class Creator
    {
        public static void RunCreate(List<ICreationInfo> jobInfoList, SPWebApplication webApp)
        {
            foreach (ICreationInfo jobInfo in jobInfoList)
            {
                var job = (JobDefinition)Activator.CreateInstance(
                    jobInfo.Type, jobInfo.Name, jobInfo.Title, webApp);
                job.Schedule = jobInfo.Schedule ?? new SPOneTimeSchedule(DateTime.Now);
                job.Update();
            }
        }

        public static void RunDelete(List<ICreationInfo> jobInfoList, SPWebApplication webApp)
        {
            try
            {
                foreach (SPJobDefinition job in webApp.JobDefinitions)
                {
                    if (jobInfoList.Exists(ci => ci.Name == job.Name))
                    {
                        job.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Couldn't delete timer job definition. Details: {0}", ex.Message));
            }
        }
    }
}
