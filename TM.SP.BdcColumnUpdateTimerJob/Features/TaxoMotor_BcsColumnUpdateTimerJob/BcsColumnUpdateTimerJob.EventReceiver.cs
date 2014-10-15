// <copyright file="BcsColumnUpdateTimerJob.EventReceiver.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-15 16:59:24Z</date>
namespace TM.SP.BdcColumnUpdateTimerJob
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Security;

    /// <summary>
    /// TODO: Add comment to BcsColumnUpdateTimerJobEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class BcsColumnUpdateTimerJobEventReceiver : SPFeatureReceiver
    {

        private static readonly string jobName = "TaxoMotorBcsColumnUpdate";

        private bool CreateJob(SPWebApplication site)
        {
            bool jobCreated = false;
            try
            {
                BcsColumnUpdateTimerJob job = new BcsColumnUpdateTimerJob(jobName, site);
                SPHourlySchedule schedule = new SPHourlySchedule()
                {
                    BeginMinute = 0,
                    EndMinute   = 59
                };
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
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;
                    if (webApp != null)
                    {
                        DeleteExistingJob(jobName, webApp);
                        CreateJob(webApp);
                    }

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;
                        if (webApp != null)
                            DeleteExistingJob(jobName, webApp);
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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

