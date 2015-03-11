// <copyright file="LicenseService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-16 19:53:14Z</date>

using System.Web;
// ReSharper disable CheckNamespace


namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
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

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class LicenseService : LayoutsPageBase
    {
        protected LicenseService()
        {
            RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        #region [common methods]
        public static void MigrateItem(SPList list, License license)
        {
            var migrationManager = new MigrationManager<License, MigratingLicense>(BCS.LOBTaxiSystemName, BCS.LOBTaxiSystemNamespace);
            var item = migrationManager.Process(license.Id, "License", "ReadLicenseItem", list.ParentWeb, LicenseMigrator.Execute);

            // updating external fields in sp list
            var refresher = new BusinessDataColumnUpdater(list, "Tm_LicenseAllViewBcsLookup");
            if (item != null)
            {
                refresher.UpdateColumnUsingBatch(null, item.ID);
            }
            else
            {
                // getting sp item
                SPListItemCollection items = list.GetItems(new SPQuery
                {
                    Query = Camlex.Query().Where(x => (int)x["Tm_LicenseExternalId"] == license.Id).ToString(),
                    ViewAttributes = "Scope='RecursiveAll'"
                });
                // updating items's external fields
                if (items.Count > 0)
                {
                    refresher.UpdateColumnUsingBatch(null, items[0].ID);
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
        public static License GetLicense(int? id)
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
        #endregion

        #region [getting xml methods]
        [WebMethod]
        public static string SuspensionGetXml(int licenseId, DateTime dateFrom, DateTime dateTo, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.OutputDate         = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom; ;
                l.TillSuspensionDate = dateTo.IsJavascriptNullDate() ? (DateTime?)null : dateTo;
                l.SuspensionReason   = reason;
                l.Status             = 2;
            });
        }
        [WebMethod]
        public static string CancellationGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate       = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
                l.CancellationReason = reason;
                l.Status             = 3;
            });
        }
        [WebMethod]
        public static string RenewalGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
                l.ChangeReason = reason;
                // setting status
                var parent     = GetLicense(l.Parent);
                var grandpa    = GetLicense(parent.Parent);
                l.Status       = grandpa.Status;
            });
        }
        [WebMethod]
        public static string MakeObsoleteGetXml(int licenseId, bool obsolete)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.Obsolete = obsolete;
            });
        }
        [WebMethod]
        public static string DisableGibddGetXml(int licenseId, bool disabled)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.DisableGibddSend = disabled;
            });
        }
        #endregion

        #region [save signed methods]
        /// <summary>
        /// Приостановка разрешения
        /// </summary>
        /// <param name="licenseId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reason"></param>
        /// <param name="signature"></param>
        [WebMethod]
        public static void SaveSignedSuspension(int licenseId, DateTime dateFrom, DateTime dateTo, string reason, string signature)
        {
            SaveSigned(licenseId, l =>
            {
                l.OutputDate         = dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom;
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
        [WebMethod]
        public static void SaveSignedMakeObsolete(int licenseId, bool obsolete, string signature)
        {
            SaveSigned(licenseId, l =>
            {
                l.Obsolete  = obsolete;
                l.Signature = Uri.UnescapeDataString(signature);
            });
        }
        [WebMethod]
        public static void SaveSignedDisableGibdd(int licenseId, bool disabled, string signature)
        {
            SaveSigned(licenseId, l =>
            {
                l.DisableGibddSend = disabled;
                l.Signature = Uri.EscapeDataString(signature);
            });
        }
        #endregion

        #region [validation methods]
        /// <summary>
        /// Проверка на значения полей при приостановке
        /// </summary>
        /// <param name="licenseId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [WebMethod]
        public static dynamic SuspensionValidate(int licenseId, DateTime dateFrom, DateTime dateTo, string reason)
        {
            return 
                Utility.WithCatchExceptionOnWebMethod("Ошибка при проверке данных", () => {
                    SPWeb web         = SPContext.Current.Web;
                    SPList spList     = web.GetListOrBreak("Lists/LicenseList");
                    SPListItem spItem = spList.GetItemOrBreak(licenseId);

                    var licCreationDate   = spItem.TryGetValue<DateTime>("Tm_LicenseFromDate");
                    var licTillDate       = spItem.TryGetValue<DateTime>("Tm_LicenseTillDate");
                    var dateFromCondition = (dateFrom >= licCreationDate && dateFrom <= licTillDate);
                    var dateToCondition   = (dateTo >= licCreationDate && dateTo <= licTillDate);
                    var reasonCondition   = !String.IsNullOrEmpty(reason);

                    if (!dateFromCondition || !dateToCondition)
                        throw new Exception("Указанные даты не попадают в диапазон дат разрешения");
                    if (!reasonCondition)
                        throw new Exception("Необходимо указать причину");
                });
        }
        /// <summary>
        /// Проверка на значения полей при аннулировании
        /// </summary>
        /// <param name="licenseId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [WebMethod]
        public static dynamic CancellationValidate(int licenseId, DateTime dateFrom, string reason)
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при проверке данных", () =>
                {
                    SPWeb web = SPContext.Current.Web;
                    SPList spList = web.GetListOrBreak("Lists/LicenseList");
                    SPListItem spItem = spList.GetItemOrBreak(licenseId);

                    var licCreationDate = spItem.TryGetValue<DateTime>("Tm_LicenseFromDate");
                    var licTillDate = spItem.TryGetValue<DateTime>("Tm_LicenseTillDate");
                    var dateFromCondition = (dateFrom >= licCreationDate && dateFrom <= licTillDate);
                    var reasonCondition = !String.IsNullOrEmpty(reason);

                    if (!dateFromCondition)
                        throw new Exception("Указанные даты не попадают в диапазон дат разрешения");
                    if (!reasonCondition)
                        throw new Exception("Необходимо указать причину");
                });
        }
        /// <summary>
        /// Проверка на значения полей при возобновлении
        /// </summary>
        /// <param name="licenseId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [WebMethod]
        public static dynamic RenewalValidate(int licenseId, DateTime dateFrom, string reason)
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при проверке данных", () =>
                {
                    SPWeb web = SPContext.Current.Web;
                    SPList spList = web.GetListOrBreak("Lists/LicenseList");
                    SPListItem spItem = spList.GetItemOrBreak(licenseId);

                    var licCreationDate = spItem.TryGetValue<DateTime>("Tm_LicenseFromDate");
                    var licTillDate = spItem.TryGetValue<DateTime>("Tm_LicenseTillDate");
                    var dateFromCondition = (dateFrom >= licCreationDate && dateFrom <= licTillDate);
                    var reasonCondition = !String.IsNullOrEmpty(reason);

                    if (!dateFromCondition)
                        throw new Exception("Указанные даты не попадают в диапазон дат разрешения");
                    if (!reasonCondition)
                        throw new Exception("Необходимо указать причину");
                });
        }
        #endregion
    }
}

