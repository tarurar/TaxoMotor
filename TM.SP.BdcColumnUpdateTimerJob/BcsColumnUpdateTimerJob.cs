using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;

using TM.Utils;

namespace TM.SP.BdcColumnUpdateTimerJob
{
    public class BcsColumnUpdateTimerJob : SPJobDefinition
    {
        #region [resource strings]

        private static readonly string FeatureId = "{C781E748-F851-454E-B740-EED5839A7979}";
        public static readonly string TaxiListsFeatureId = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        #endregion

        #region [methods]

        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }

        public BcsColumnUpdateTimerJob() : base() {}

        public BcsColumnUpdateTimerJob(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public BcsColumnUpdateTimerJob(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                SPWebApplication webApp = this.Parent as SPWebApplication;
                foreach (SPSite siteCollection in webApp.Sites)
                {
                    SPWeb web = siteCollection.RootWeb;
                    var context = SPServiceContext.GetContext(siteCollection);
                    using (var scope = new SPServiceContextScope(context))
                    {
                        UpdateBcsColumns(web);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(GetFeatureLocalizedResource("BcsColumnUpdateGeneralErrorFmt"), ex.Message));
            }
        }

        private void UpdateBcsColumns(SPWeb web)
        {
            if (web.Features[new Guid(TaxiListsFeatureId)] != null && web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
            {
                var licenseList = web.GetListOrBreak("Lists/LicenseList");
                var refresher = new BusinessDataColumnUpdater(licenseList, "Tm_LicenseAllViewBcsLookup");
                refresher.UpdateColumnUsingBatch(0);
            }
        }

        #endregion
    }
}
