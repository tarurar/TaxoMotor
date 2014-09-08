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
        public static readonly string webUrlPropertyKeyName = "WebUrl";

        private static readonly string jobName = "TaxoMotorCoordinateV5AnswerProcessing";

        private bool CreateJob(SPWebApplication site, string webUrl)
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

                // set web url
                if (job.Properties.ContainsKey(webUrlPropertyKeyName))
                {
                    job.Properties[webUrlPropertyKeyName] = webUrl;
                }
                else
                {
                    job.Properties.Add(webUrlPropertyKeyName, webUrl);
                }

                job.Update();
            }
            catch (Exception)
            {
                return jobCreated;
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
            catch (Exception)
            {
                return jobDeleted;
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
                    SPWeb parentWeb = (SPWeb)properties.Feature.Parent;
                    if (parentWeb != null)
                    {
                        SPWebApplication parentWebApp = parentWeb.Site.WebApplication;
                        DeleteExistingJob(jobName, parentWebApp);
                        CreateJob(parentWebApp, parentWeb.Url);
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
                        SPWeb parentWeb = (SPWeb)properties.Feature.Parent;
                        if (parentWeb != null)
                        {
                            SPWebApplication parentWebApp = parentWeb.Site.WebApplication;
                            DeleteExistingJob(jobName, parentWebApp);
                        }
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

