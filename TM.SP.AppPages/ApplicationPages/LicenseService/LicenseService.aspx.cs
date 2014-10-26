// <copyright file="LicenseService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-16 19:53:14Z</date>



// ReSharper disable once CheckNamespace


namespace TM.SP.AppPages
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web.Services;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using Utils;
    using BCSModels.Taxi;
    using TP.SP.DataMigration;
    using Microsoft.BusinessData.MetadataModel;
    using CamlexNET;

    /// <summary>
    /// Service page for serving requests
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class LicenseService : LayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the LicenseService class
        /// </summary>
        protected LicenseService()
        {
            RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        private static void MigrateItem(SPList list, License license)
        {
            var migrationManager = new MigrationManager<License, MigratingLicense>(BCS.LOBTaxiSystemName, BCS.LOBTaxiSystemNamespace);
            var item = migrationManager.Process(license.Id, "License", "ReadLicenseItem", list.ParentWeb, LicenseMigrator.Execute);

            // updating external fields in sp list
            var refresher = new BusinessDataColumnUpdater(list, "Tm_LicenseAllViewBcsLookup");
            if (item != null)
            {
                refresher.UpdateColumnUsingBatch(item.ID);
            }
            else
            {
                // getting sp item
                SPListItemCollection items = list.GetItems(new SPQuery()
                {
                    Query = Camlex.Query().Where(x => (int)x["Tm_LicenseExternalId"] == license.Id).ToString(),
                    ViewAttributes = "Scope='RecursiveAll'"
                });
                // updating items's external fields
                if (items.Count > 0)
                {
                    refresher.UpdateColumnUsingBatch(items[0].ID);
                }
            }
        }

        private static void SaveSigned(int licenseId, Action<License> contextAction)
        {
            SPWeb web         = SPContext.Current.Web;
            SPList spList     = web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);

            var parentLicense = GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

            var newLicenseBefore = parentLicense.Clone();
            contextAction(newLicenseBefore);

            var newLicenseAfter = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                contentType = "License",
                lob         = BCS.LOBTaxiSystemName,
                methodName  = "CreateLicense",
                methodType  = MethodInstanceType.Creator,
                ns          = BCS.LOBTaxiSystemNamespace
            }, newLicenseBefore);

            web.AllowUnsafeUpdates = true;
            try
            {
                MigrateItem(spList, parentLicense); //in case parent data hasn't been migrated yet
                MigrateItem(spList, newLicenseAfter);
            }
            finally
            {
                web.AllowUnsafeUpdates = false;                    
            }
        }

        private static string GetLicenseXml(int licenseId, Action<License> contextAction)
        {
            SPWeb web         = SPContext.Current.Web;
            SPList spList     = web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);

            var license = GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

            var newLicense = license.Clone();
            contextAction(newLicense);

            //serialization
            var intWriter = new StringWriter(new StringBuilder());
            XmlWriter writer = new XmlTextWriter(intWriter);
            var serializer = new XmlSerializer(typeof(License));
            writer.WriteStartElement("Data");
            serializer.Serialize(writer, newLicense);
            writer.WriteEndElement();
            
            return intWriter.ToString();
        }

        private static License GetLicense(int? id)
        {
            if (id == null || id == 0)
                throw new Exception("Item id must be specified");

            var item = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                contentType = "License",
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                methodName  = "ReadLicenseItem",
                methodType  = MethodInstanceType.SpecificFinder
            }, id);

            return item;
        }

        [WebMethod]
        public static string SuspensionGetXml(int licenseId, DateTime dateFrom, DateTime dateTo, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate       = dateFrom;
                l.TillSuspensionDate = dateTo.IsJavascriptNullDate() ? (DateTime?)null : dateTo;
                l.SuspensionReason   = reason;
            });
        }

        [WebMethod]
        public static string CancellationGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate       = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
                l.CancellationReason = reason;
            });
        }

        [WebMethod]
        public static string RenewalGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
                l.ChangeReason = reason;
            });
        }

        [WebMethod]
        public static void SaveSignedSuspension(int licenseId, DateTime dateFrom, DateTime dateTo, string reason, string signature)
        {
            SaveSigned(licenseId, l =>
            {
                l.CreationDate       = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
                l.TillSuspensionDate = dateTo.IsJavascriptNullDate() ? (DateTime?)null : dateTo;
                l.SuspensionReason   = reason;
                l.Signature          = Uri.UnescapeDataString(signature);
                l.Status             = 2;
            });
        }

        [WebMethod]
        public static void SaveSignedCancellation(int licenseId, DateTime dateFrom, string reason, string signature)
        {
            SaveSigned(licenseId, l =>
            {
                l.CreationDate       = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
                l.CancellationReason = reason;
                l.Signature          = Uri.UnescapeDataString(signature);
                l.Status             = 3;
            });
        }

        [WebMethod]
        public static void SaveSignedRenewal(int licenseId, DateTime dateFrom, string reason, string signature)
        {
            SaveSigned(licenseId, l =>
            {
                l.CreationDate = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
                l.ChangeReason = reason;
                l.Signature    = Uri.UnescapeDataString(signature);
                // setting status
                var parent     = GetLicense(l.Parent);
                var grandpa    = GetLicense(parent.Parent);
                l.Status       = grandpa.Status;
            });
        }
    }
}

