// <copyright file="TaxoMotor_CommonTimerJobs.EventReceiver.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-04-24 15:40:56Z</date>
namespace TM.SP.AppPages
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using Timers;

    /// <summary>
    /// TODO: Add comment to TaxoMotor_CommonTimerJobsEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class TaxoMotor_CommonTimerJobsEventReceiver : SPFeatureReceiver
    {
        private static readonly string OdopmJobName = "TaxoMotorOdopmJob";
        private static readonly string MoJobName = "TaxoMotorMoJob";
        private static readonly string UpdateSqlViewsJobName = "TaxoMotorUpdateSqlViewsJob";
        private static readonly string VirtualSignerJobName = "TaxoMotorVirtualSignLicenses";

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
                        DeleteExistingJob(OdopmJobName, webApp);
                        DeleteExistingJob(MoJobName, webApp);
                        DeleteExistingJob(UpdateSqlViewsJobName, webApp);
                        DeleteExistingJob(VirtualSignerJobName, webApp);

                        try
                        {
                            OdopmTimer job = new OdopmTimer(OdopmJobName, webApp);
                            SPDailySchedule schedule = new SPDailySchedule
                            {
                                BeginHour = 1,
                                BeginMinute = 0,
                                EndHour = 1,
                                EndMinute = 15
                            };
                            job.Schedule = schedule;
                            job.Update();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("Couldn't create timer job definition for {0}. Details: {1}", OdopmJobName, ex.Message));
                        }

                        try
                        {
                            MoTimer job = new MoTimer(MoJobName, webApp);
                            SPDailySchedule schedule = new SPDailySchedule
                            {
                                BeginHour = 4,
                                BeginMinute = 0,
                                EndHour = 4,
                                EndMinute = 15
                            };
                            job.Schedule = schedule;
                            job.Update();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("Couldn't create timer job definition for {0}. Details: {1}", MoJobName, ex.Message));
                        }

                        try
                        {
                            UpdateViewsTimer job = new UpdateViewsTimer(UpdateSqlViewsJobName, webApp);
                            SPDailySchedule schedule = new SPDailySchedule
                            {
                                BeginHour = 2,
                                BeginMinute = 15,
                                EndHour = 4,
                                EndMinute = 15
                            };
                            job.Schedule = schedule;
                            job.Update();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("Couldn't create timer job definition for {0}. Details: {1}", MoJobName, ex.Message));
                        }

                        try
                        {
                            VirualSignerTimer job = new VirualSignerTimer(VirtualSignerJobName, webApp);
                            job.Update();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("Couldn't create timer job definition for {0}. Details: {1}", MoJobName, ex.Message));
                        }
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
                        {
                            DeleteExistingJob(OdopmJobName, webApp);
                            DeleteExistingJob(MoJobName, webApp);
                            DeleteExistingJob(UpdateSqlViewsJobName, webApp);
                            DeleteExistingJob(VirtualSignerJobName, webApp);
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

