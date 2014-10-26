using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using TM.Utils;
using TM.SP.BCSModels.CoordinateV5;
using TM.SP.BCSModels.Taxi;
using TP.SP.DataMigration;
using CoordinateV5File = TM.SP.BCSModels.CoordinateV5.File;

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

        private void MigrateIncomingRequest(SPWeb web)
        {
            var manager = new MigrationManager<Request, MigratingRequest>(BCS.LOBRequestSystemName,
                BCS.LOBRequestSystemNamespace);
            manager.Process(0, RequestCt, ReadRequestItem, web, IncomeRequestMigrator.Execute);
        }
        private void MigrateLicense(SPWeb web)
        {
            var manager = new MigrationManager<License, MigratingLicense>(BCS.LOBTaxiSystemName,
                BCS.LOBTaxiSystemNamespace);
            manager.Process(0, LicenseCt, ReadLicenseItem, web, LicenseMigrator.Execute);
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
