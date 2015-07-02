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
    using TM.Utils.TimerJobs;

    /// <summary>
    /// TODO: Add comment to TaxoMotor_CommonTimerJobsEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class TaxoMotor_CommonTimerJobsEventReceiver : SPFeatureReceiver
    {
        private static readonly List<ICreationInfo> jobList = new List<ICreationInfo> 
        { 
            #region ОДОПМ
            new CreationInfo 
            { 
                Name     = "TaxoMotorOdopmJob", 
                Title    = "ТаксоМотор: Отправка в ОДОПМ", 
                Type     = typeof(OdopmTimer), 
                Schedule = new SPDailySchedule 
                {
                    BeginHour   = 1, 
                    BeginMinute = 0, 
                    EndHour     = 1, 
                    EndMinute   = 15 
                }  
            },
            #endregion
            #region МО
            new CreationInfo 
            { 
                Name     = "TaxoMotorMoJob", 
                Title    = "ТаксоМотор: Отправка в МО", 
                Type     = typeof(MoTimer), 
                Schedule = new SPDailySchedule 
                {
                    BeginHour   = 4,
                    BeginMinute = 0,
                    EndHour     = 4,
                    EndMinute   = 15
                }  
            },
            #endregion
            #region Обновление представлений SQL
            new CreationInfo
            {
                Name     = "TaxoMotorUpdateSqlViewsJob", 
                Title    = "ТаксоМотор: Обновление представлений SQL", 
                Type     = typeof(UpdateViewsTimer), 
                Schedule = new SPDailySchedule 
                {
                    BeginHour   = 2,
                    BeginMinute = 15,
                    EndHour     = 4,
                    EndMinute   = 15
                }
            },
            #endregion
            #region Отправка в РСТ
            new CreationInfo
            {
                Name     = "TaxoMotorSendSpecialVehicleRegister", 
                Title    = "ТаксоМотор: Отправка сведений в РСТ", 
                Type     = typeof(SpecialTransportRegisterTimer), 
                Schedule = new SPDailySchedule 
                {
                    BeginHour   = 3,
                    BeginMinute = 15,
                    EndHour     = 5,
                    EndMinute   = 15
                }
            }
            #endregion
            /*
            #region Виртуальное подписание
            new CreationInfo
            {
                Name     = "TaxoMotorVirtualSignLicenses", 
                Title    = "ТаксоМотор: Виртуальный подписант разрешений", 
                Type     = typeof(VirualSignerTimer), 
                Schedule = null
            }
            #endregion
            */
        };
        
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
                        Creator.RunDelete(jobList, webApp);
                        Creator.RunCreate(jobList, webApp);
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
                            Creator.RunDelete(jobList, webApp);
                        }
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

