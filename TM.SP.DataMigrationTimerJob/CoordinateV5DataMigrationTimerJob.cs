using System;
using System.Net.Http;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using TM.Utils;
using TM.SP.BCSModels.CoordinateV5;
using TM.SP.BCSModels.Taxi;
using TP.SP.DataMigration;
using TM.SP.AppPages;
using CoordinateV5File = TM.SP.BCSModels.CoordinateV5.File;
using System.Net;

namespace TM.SP.DataMigrationTimerJob
{

    public class CoordinateV5DataMigrationTimerJob : SPJobDefinition
    {
        #region resource strings

        private static readonly string FeatureId           = "{785b2032-a102-44b8-a747-08121f2a9d0b}";
        public static readonly string Cv5ListsFeatureId    = "{88749623-db7e-4ffc-b1e4-b6c4cf9332b6}";
        public static readonly string TaxiListsFeatureId   = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        private static readonly string RequestCt                = GetFeatureLocalizedResource("RequestEntityName");
        private static readonly string LicenseCt                = GetFeatureLocalizedResource("LicenseEntityName");

        private static readonly string ReadRequestItem          = GetFeatureLocalizedResource("ReadRequestItemMethodName");
        private static readonly string ReadLicenseItem          = GetFeatureLocalizedResource("ReadLicenseItemMethodName");

        #endregion

        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }
        public CoordinateV5DataMigrationTimerJob()
        { }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.None)
        {
            Title = GetFeatureLocalizedResource("JobTitle");
        }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            Title = GetFeatureLocalizedResource("JobTitle");
        }

        private void NotifyAboutItemStatus(SPListItem spItem)
        {
            var web = spItem.ParentList.ParentWeb;

            var url = SPUtility.ConcatUrls(SPUtility.GetWebLayoutsFolder(web), "TaxoMotor/SendStatus.aspx");
            var uriBuilder = new UriBuilder(url) { Port = -1 };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["ListId"] = spItem.ParentList.ID.ToString("B");
            query["Items"] = spItem.ID.ToString();
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();

            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = 0;
            request.UseDefaultCredentials = true;
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(String.Format(GetFeatureLocalizedResource("SendWebRequestErrorFmt"), url));
        }

        private void MigrateIncomingRequest(SPWeb web)
        {
            int repeat = Config.GetConfigValueOrDefault<Int32>(web, "MigrateIncomeRequestCountPerJob");

            var manager = new MigrationManager<Request, MigratingRequest>(BCS.LOBRequestSystemName,
                BCS.LOBRequestSystemNamespace);

            for (int i = 0; i < repeat; i++)
            {
                var request = manager.Process(0, RequestCt, ReadRequestItem, web, IncomeRequestMigrator.Execute);
                if (request != null)
                {
                    // saving income request status change history
                    var statusXml = IncomeRequestService.GetIncomeRequestCoordinateV5StatusMessage(request.ID);
                    IncomeRequestService.SaveIncomeRequestStatusLog(request.ID, statusXml);
                    // sending income request status
                    NotifyAboutItemStatus(request);
                }
                else break;
            }
        }
        private void MigrateLicense(SPWeb web)
        {
            int repeat = Config.GetConfigValueOrDefault<Int32>(web, "MigrateLicenseCountPerJob");
            SPList licList = web.GetListOrBreak("Lists/LicenseList");

            var manager = new MigrationManager<License, MigratingLicense>(BCS.LOBTaxiSystemName,
                BCS.LOBTaxiSystemNamespace);
            var refresher = new BusinessDataColumnUpdater(licList, "Tm_LicenseAllViewBcsLookup");
            for (int i = 0; i < repeat; i++)
            {
                var item = manager.Process(0, LicenseCt, ReadLicenseItem, web, LicenseMigrator.Execute);
                if (item != null)
                {
                    refresher.UpdateColumnUsingBatch(null, item.ID);
                    var parentLookup = item["Tm_LicenseParentLicenseLookup"];
                    if (parentLookup != null)
                    {
                        var parentId = new SPFieldLookupValue(parentLookup.ToString()).LookupId;
                        refresher.UpdateColumnUsingBatch(null, parentId);
                    }
                }
                else break;
            }
        }

        private void ProcessMigration(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            try
            {
                if (web.Features[new Guid(Cv5ListsFeatureId)] != null)
                {
                    MigrateIncomingRequest(web);
                }

                if (web.Features[new Guid(TaxiListsFeatureId)] != null && web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                {
                    MigrateLicense(web);
                }
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                var webApp = Parent as SPWebApplication;
                foreach (SPSite siteCollection in webApp.Sites)
                {
                    SPWeb web = siteCollection.RootWeb;
                    var context = SPServiceContext.GetContext(siteCollection);
                    using (var scope = new SPServiceContextScope(context))
                    {
                        ProcessMigration(web);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(GetFeatureLocalizedResource("MigrationGeneralErrorFmt"), ex.Message));
            }
        }


    }
}
