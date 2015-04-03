// <copyright file="PlumsailFormsBackupTimers.EventReceiver.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-04-03 16:08:24Z</date>
namespace SP.PlumsailFormsBackup
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class PlumsailFormsBackupTimersEventReceiver : SPFeatureReceiver
    {
        private bool CreateJob<T>(SPWebApplication site, SPSchedule schedule, string name) where T: Timers.BackupJob
        {
            bool jobCreated = false;
            try
            {
                T job = Activator.CreateInstance(typeof(T), name, site) as T;
                job.Schedule = schedule;
                job.Update();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Couldn't create timer job definition for {0}. Details: {1}", name, ex.Message));
            }
            return jobCreated;
        }

        private bool DeleteExistingJob(string name, SPWebApplication site)
        {
            bool jobDeleted = false;
            try
            {
                foreach (SPJobDefinition job in site.JobDefinitions)
                {
                    if (job.Name == name)
                    {
                        job.Delete();
                        jobDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Couldn't delete timer job definition for {0}. Details: {1}", name, ex.Message));
            }
            return jobDeleted;
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                var webApp = (SPWebApplication)properties.Feature.Parent;
                if (webApp != null)
                {
                    DeleteExistingJob(Timers.BackupJob.SystemName, webApp);
                    SPDailySchedule sc = new SPDailySchedule 
                    { 
                        BeginHour   = 0,
                        BeginMinute = 0,
                        EndHour     = 3,
                        EndMinute   = 0
                    };

                    CreateJob<Timers.BackupJob>(webApp, sc, Timers.BackupJob.SystemName);
                }
            });
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            lock (this)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var webApp = (SPWebApplication)properties.Feature.Parent;
                    if (webApp != null)
                        DeleteExistingJob(Timers.BackupJob.SystemName, webApp);
                });
            }
        }
    }
}

