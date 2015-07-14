using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Utils.TimerJobs
{
    public class JobDefinition: SPJobDefinition
    {
        #region [resource strings]
        public static readonly string TaxiListsFeatureId = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";
        #endregion

        public JobDefinition() { }

        public JobDefinition(string jobName, string jobTitle, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            Title = jobTitle;
        }

        public JobDefinition(string jobName, string jobTitle, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            Title = jobTitle;
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                var webApp = Parent as SPWebApplication;
                if (webApp != null)
                {
                    foreach (SPSite siteCollection in webApp.Sites)
                    {
                        try
                        {
                            SPWeb web = siteCollection.RootWeb;

                            if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                                web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                            {
                                var context = SPServiceContext.GetContext(siteCollection);
                                using (new SPServiceContextScope(context))
                                {
                                    Work(web);
                                }
                            }
                        }
                        finally
                        {
                            siteCollection.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Детали: {0}", ex.Message));
            }
        }

        protected virtual void Work(SPWeb web)
        {
            throw new NotImplementedException();
        }
    }
}
