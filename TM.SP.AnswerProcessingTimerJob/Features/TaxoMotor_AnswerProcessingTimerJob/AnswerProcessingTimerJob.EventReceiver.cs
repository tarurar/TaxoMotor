// <copyright file="AnswerProcessingTimerJob.EventReceiver.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-09-08 15:35:00Z</date>
namespace TM.SP.AnswerProcessingTimerJob
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment to AnswerProcessingTimerJobEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class AnswerProcessingTimerJobEventReceiver : SPFeatureReceiver
    {
        private static readonly string jobName = "TaxoMotorCoordinateV5AnswerProcessing";

        private bool CreateJob(SPWebApplication site)
        {
            bool jobCreated = false;
            try
            {
                CoordinateV5AnswerProcessingTimerJob job = new CoordinateV5AnswerProcessingTimerJob(jobName, site);
                SPMinuteSchedule schedule = new SPMinuteSchedule();
                schedule.BeginSecond = 0;
                schedule.EndSecond = 59;
                schedule.Interval = 1;
                job.Schedule = schedule;

                job.Update();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Couldn't create timer job definition for {0}. Details: {1}", jobName, ex.Message));
            }
            return jobCreated;
        }
        public bool DeleteExistingJob(string jobName, SPWebApplication site)
        {
            bool jobDeleted = false;
            try
            {
                foreach (SPJobDefinition job in site.JobDefinitions)
                {
                    if (job.Name == jobName)
                    {
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
        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                var webApp = (SPWebApplication)properties.Feature.Parent;
                if (webApp != null)
                {
                    DeleteExistingJob(jobName, webApp);
                    CreateJob(webApp);
                }

            });
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            lock (this)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var webApp = (SPWebApplication)properties.Feature.Parent;
                    if (webApp != null)
                        DeleteExistingJob(jobName, webApp);
                });
            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions after feature installation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature uninstalling
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature upgrading
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        /// <param name="upgradeActionName">The name of the custom upgrade action to execute. The value can be null if the override of this method implements only one upgrade action.</param>
        /// <param name="parameters">Parameter names and values for the custom action</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            ////TODO: place receiver code here or remove method
        }
    }
}

