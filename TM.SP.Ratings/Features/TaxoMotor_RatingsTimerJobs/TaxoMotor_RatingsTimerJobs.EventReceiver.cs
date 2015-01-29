using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using TM.SP.Ratings.Timers;
using TM.SP.Ratings.Schedule;
using TM.SP.Ratings.Reports;

namespace TM.SP.Ratings.Features.TaxoMotor_RatingsTimerJobs
{
    [Guid("bc09565b-fc36-46a3-bab8-fa4151e7c84f")]
    public class TaxoMotor_RatingsTimerJobsEventReceiver : RatingBaseFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                var webApp = (SPWebApplication)properties.Feature.Parent;
                if (webApp != null)
                {
                    var ratingIntf = typeof(IRatingReport);
                    var ratingBaseType = typeof(RatingBaseJobDefinition);
                    var reportTypeList = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                        .Where(p => ratingIntf.IsAssignableFrom(p) && p.IsClass && p.IsSubclassOf(ratingBaseType));

                    foreach (Type reportType in reportTypeList)
                    {
                        IRatingReport report = (IRatingReport)Activator.CreateInstance(reportType);
                        DeleteExistingJob(report.GetName(), webApp);
                        CreateJob(webApp, report.GetName(), report.GetTitle(), ScheduleFactory.GetMinute(), typeof(RatingCarrierActingLicences));
                    }
                }
            });
        }


        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            lock (this)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var webApp = (SPWebApplication)properties.Feature.Parent;
                    if (webApp != null)
                    {
                        var ratingIntf = typeof(IRatingReport);
                        var ratingBaseType = typeof(RatingBaseJobDefinition);
                        var reportTypeList = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                            .Where(p => ratingIntf.IsAssignableFrom(p) && p.IsClass && p.IsSubclassOf(ratingBaseType));

                        foreach (Type reportType in reportTypeList)
                        {
                            IRatingReport report = (IRatingReport)Activator.CreateInstance(reportType);
                            DeleteExistingJob(report.GetName(), webApp);
                        }
                    }
                });
            }
        }
    }
}
