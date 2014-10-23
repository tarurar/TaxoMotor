using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData;
using Microsoft.SharePoint.Utilities;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.Infrastructure;

using TM.Utils;
using TM.SP.BCSModels;
using TM.SP.BCSModels.CoordinateV5;
using TM.SP.BCSModels.Taxi;
using TM.SP.BCSModels.TaxiV2;
using TP.SP.DataMigration;
using CoordinateV5File = TM.SP.BCSModels.CoordinateV5.File;
using CamlexNET;

namespace TM.SP.DataMigrationTimerJob
{

    public class CoordinateV5DataMigrationTimerJob : SPJobDefinition
    {
        #region resource strings

        private static readonly string FeatureId           = "{785b2032-a102-44b8-a747-08121f2a9d0b}";
        public static readonly string CV5ListsFeatureId    = "{88749623-db7e-4ffc-b1e4-b6c4cf9332b6}";
        public static readonly string TaxiListsFeatureId   = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        private static readonly string RequestCT                = GetFeatureLocalizedResource("RequestEntityName");
        private static readonly string ServiceCT                = GetFeatureLocalizedResource("ServiceEntityName");
        private static readonly string ServiceHeaderCT          = GetFeatureLocalizedResource("ServiceHeaderEntityName");
        private static readonly string RequestAccountCT         = GetFeatureLocalizedResource("RequestAccountEntityName");
        private static readonly string RequestContactCT         = GetFeatureLocalizedResource("RequestContactEntityName");
        private static readonly string ServiceDocCT             = GetFeatureLocalizedResource("ServiceDocumentEntityName");
        private static readonly string TaxiInfoCT               = GetFeatureLocalizedResource("TaxiInfoEntityName");
        private static readonly string ServicePropsCT           = GetFeatureLocalizedResource("ServicePropertiesEntityName");
        private static readonly string FileCT                   = GetFeatureLocalizedResource("FileEntityName");
        private static readonly string LicenseCT                = GetFeatureLocalizedResource("LicenseEntityName");
        private static readonly string LicenseV2AllViewCT       = GetFeatureLocalizedResource("LicenseV2AllViewEntityName");

        private static readonly string TakeItemMethod           = GetFeatureLocalizedResource("TakeItemMethodName");
        private static readonly string UpdateMigrationStatus    = GetFeatureLocalizedResource("UpdateMigrationStatusMethodName");
        private static readonly string FinishMigration          = GetFeatureLocalizedResource("FinishMigrationMethodName");

        private static readonly string ReadRequestItem          = GetFeatureLocalizedResource("ReadRequestItemMethodName");
        private static readonly string ReadLicenseAllViewItem   = GetFeatureLocalizedResource("ReadLicenseAllViewItemMethodName");
        private static readonly string ReadLicenseItem          = GetFeatureLocalizedResource("ReadLicenseItemMethodName");
        private static readonly string ReadServicePropsItem     = GetFeatureLocalizedResource("ReadServicePropertiesItemMethodName");
        private static readonly string ListMissingErrorFmt      = GetFeatureLocalizedResource("ListMissingErrorFmt");
        private static readonly string SingleListValueErrorFmt  = GetFeatureLocalizedResource("SingleListValueErrorFmt");
        private static readonly string SingleParentItemErrorFmt = GetFeatureLocalizedResource("SingleParentItemErrorFmt");
        private static readonly string ReadServiceItem          = GetFeatureLocalizedResource("ReadServiceItemMethodName");
        private static readonly string ReadServiceHeaderItem    = GetFeatureLocalizedResource("ReadServiceHeaderItemMethodName");
        private static readonly string ReadRequestAccountItem   = GetFeatureLocalizedResource("ReadRequestAccountItemMethodName");
        private static readonly string ReadRequestContactItem   = GetFeatureLocalizedResource("ReadRequestContactItemMethodName");
        private static readonly string RequestTitleFmt          = GetFeatureLocalizedResource("RequestTitleFmt");
        private static readonly string TaxiTitleFmt             = GetFeatureLocalizedResource("TaxiTitleFmt");
        private static readonly string ServicePropsTaxiList     = GetFeatureLocalizedResource("ServicePropertiesTaxiListMethodName");
        private static readonly string ServiceDocumentList      = GetFeatureLocalizedResource("ServiceDocumentListMethodName");
        private static readonly string ServiceDocumentFileList  = GetFeatureLocalizedResource("ServiceDocumentFileListMethodName");
        private static readonly string ReadFileItemContent      = GetFeatureLocalizedResource("ReadFileItemContentMethodName");

        #endregion

        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }
        public CoordinateV5DataMigrationTimerJob() : base() { }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        private void MigrateIncomingRequest(SPWeb web)
        {
            var manager = new MigrationManager<Request, MigratingRequest>(BCS.LOBRequestSystemName,
                BCS.LOBRequestSystemNamespace);
            manager.Process(0, RequestCT, ReadRequestItem, web, IncomeRequestMigrator.Execute);
        }
        private void MigrateLicense(SPWeb web)
        {
            var manager = new MigrationManager<License, MigratingLicense>(BCS.LOBTaxiSystemName,
                BCS.LOBTaxiSystemNamespace);
            manager.Process(0, LicenseCT, ReadLicenseItem, web, LicenseMigrator.Execute);
        }

        private void ProcessMigration(SPWeb web)
        {
            if (web.Features[new Guid(CV5ListsFeatureId)] != null)
            {
                MigrateIncomingRequest(web);
            }

            if (web.Features[new Guid(TaxiListsFeatureId)] != null && web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
            {
                MigrateLicense(web);
            }
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
