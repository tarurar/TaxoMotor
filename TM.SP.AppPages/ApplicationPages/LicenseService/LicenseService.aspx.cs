// <copyright file="LicenseService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-16 19:53:14Z</date>

using System.Web;
using System.Linq;
using Microsoft.XmlDiffPatch;
using TM.SP.AppPages.Validators;
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
    using System.Xml.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Collections.Generic;

    public struct LicenseValidationData
    {
        public bool Valid;
        public string DeveloperInfo;
        public string FailMessage;
        public string SuccessMessage;
    }

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
            SPListItem itemToUpdate = item;

            if (itemToUpdate == null)
            {
                SPListItemCollection items = list.GetItems(new SPQuery
                {
                    Query = Camlex.Query().Where(x => (int)x["Tm_LicenseExternalId"] == license.Id).ToString(),
                    ViewAttributes = "Scope='Recursive'"
                });

                itemToUpdate = items.Cast<SPListItem>().FirstOrDefault();
            }

            if (itemToUpdate != null)
            {
                refresher.UpdateColumnUsingBatch(null, itemToUpdate.ID);
                var parentLookup = itemToUpdate["Tm_LicenseParentLicenseLookup"];
                if (parentLookup != null)
                {
                    var parentId = new SPFieldLookupValue(parentLookup.ToString()).LookupId;
                    refresher.UpdateColumnUsingBatch(null, parentId);
                }
            }
        }
        private static void SaveSigned(int licenseId, Action<License> contextAction)
        {
            SPWeb web         = SPContext.Current.Web;
            SPList spList     = web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);

            var parentLicense = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));
            if (parentLicense.HasAnyChilds.HasValue && parentLicense.HasAnyChilds.Value)
                throw new Exception("Выполнение операции неавозможно. Данное разрешение находится в работе в одном из обращений или не является последней редацией.");

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

            var license = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

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
        #endregion

        #region [getting xml methods]
        [WebMethod]
        public static string SuspensionGetXml(int licenseId, DateTime dateFrom, DateTime dateTo, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.OutputDate         = GetUnspecifiedDate(dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom);
                l.TillSuspensionDate = GetUnspecifiedDate(dateTo.IsJavascriptNullDate() ? DateTime.Now: dateTo);
                l.SuspensionReason   = Uri.UnescapeDataString(reason);
                l.Status             = 2;
            });
        }
        [WebMethod]
        public static string CancellationGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.OutputDate         = GetUnspecifiedDate(dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom);
                l.CancellationReason = Uri.UnescapeDataString(reason);
                l.Status             = 3;
            });
        }
        [WebMethod]
        public static string RenewalGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.OutputDate   = GetUnspecifiedDate(dateFrom.IsJavascriptNullDate() ? DateTime.Now : dateFrom);
                l.ChangeReason = Uri.UnescapeDataString(reason);
                // setting status
                var parent     = LicenseHelper.GetLicense(l.Parent);
                var grandpa    = LicenseHelper.GetLicense(parent.Parent);
                l.Status       = grandpa.Status;
            });
        }
        [WebMethod]
        public static string MakeObsoleteGetXml(int licenseId, bool obsolete)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.OutputDate = GetUnspecifiedDate(DateTime.Now);
                l.Obsolete = obsolete;
            });
        }
        [WebMethod]
        public static string DisableGibddGetXml(int licenseId, bool disabled)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.OutputDate = GetUnspecifiedDate(DateTime.Now);
                l.DisableGibddSend = disabled;
            });
        }
        #endregion


        private static DateTime GetDateTimeFromXml(string sourceXml, string tagName)
        {
            var xmlDoc = XDocument.Parse(sourceXml);
            var dateXml = xmlDoc.Descendants().Where(n => n.Name.LocalName == tagName).FirstOrDefault();
            if (dateXml == null)
                throw new Exception(String.Format("В предоставленном xml нет тэга {0}", tagName));

            DateTime parsedDate;
            if (!DateTime.TryParse(dateXml.Value, out parsedDate))
                throw new Exception(String.Format("Невозможно получить значение типа 'Дата' из тэга {0}", tagName));

            return parsedDate;
        }

        private static DateTime GetUnspecifiedDate(DateTime date)
        {
            return new DateTime(
                date.Year, 
                date.Month, 
                date.Day, 
                date.Hour, 
                date.Minute, 
                date.Second, 
                0, 
                DateTimeKind.Unspecified);
        }

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
        public static dynamic SaveSignedSuspension(int licenseId, DateTime dateFrom, DateTime dateTo, string reason, string signature)
        {
            return Utility.WithCatchExceptionOnWebMethod("Ошибка при сохранении подписи", () =>
                {
                    var signedXml = Uri.UnescapeDataString(signature);
                    var outputDate = GetDateTimeFromXml(signedXml, "outputdate");
                    var changeDate = GetDateTimeFromXml(signedXml, "changedate");
                    var tillsuspDate = GetDateTimeFromXml(signedXml, "tillsuspensiondate");

                    SaveSigned(licenseId, l =>
                    {
                        l.OutputDate = outputDate;
                        l.ChangeDate = changeDate;
                        l.TillSuspensionDate = tillsuspDate;
                        l.SuspensionReason = Uri.UnescapeDataString(reason);
                        l.Signature = signedXml;
                        l.Status = 2;
                    });
                });
        }
        [WebMethod]
        public static dynamic SaveSignedCancellation(int licenseId, DateTime dateFrom, string reason, string signature)
        {
            return Utility.WithCatchExceptionOnWebMethod("Ошибка при сохранении подписи", () =>
                {
                    var signedXml = Uri.UnescapeDataString(signature);
                    var outputDate = GetDateTimeFromXml(signedXml, "outputdate");
                    var changeDate = GetDateTimeFromXml(signedXml, "changedate");

                    SaveSigned(licenseId, l =>
                    {
                        l.OutputDate = outputDate;
                        l.ChangeDate = changeDate;
                        l.CancellationReason = Uri.UnescapeDataString(reason);
                        l.Signature = signedXml;
                        l.Status = 3;
                    });
                });
        }
        [WebMethod]
        public static dynamic SaveSignedRenewal(int licenseId, DateTime dateFrom, string reason, string signature)
        {
            return Utility.WithCatchExceptionOnWebMethod("Ошибка при сохранении подписи", () =>
                {
                    var signedXml = Uri.UnescapeDataString(signature);
                    var outputDate = GetDateTimeFromXml(signedXml, "outputdate");
                    var changeDate = GetDateTimeFromXml(signedXml, "changedate");

                    SaveSigned(licenseId, l =>
                    {
                        l.OutputDate = outputDate;
                        l.ChangeDate = changeDate;
                        l.ChangeReason = Uri.UnescapeDataString(reason);
                        l.Signature = signedXml;
                        // setting status
                        var parent = LicenseHelper.GetLicense(l.Parent);
                        var grandpa = LicenseHelper.GetLicense(parent.Parent);
                        l.Status = grandpa.Status;
                    });
                });
        }
        [WebMethod]
        public static dynamic SaveSignedMakeObsolete(int licenseId, bool obsolete, string signature)
        {
            return Utility.WithCatchExceptionOnWebMethod("Ошибка при сохранении подписи", () =>
                {
                    var signedXml = Uri.UnescapeDataString(signature);
                    var outputDate = GetDateTimeFromXml(signedXml, "outputdate");
                    var changeDate = GetDateTimeFromXml(signedXml, "changedate");

                    SaveSigned(licenseId, l =>
                    {
                        l.OutputDate = outputDate;
                        l.ChangeDate = changeDate;
                        l.Obsolete = obsolete;
                        l.Signature = signedXml;
                    });
                });
        }
        [WebMethod]
        public static dynamic SaveSignedDisableGibdd(int licenseId, bool disabled, string signature)
        {
            return Utility.WithCatchExceptionOnWebMethod("Ошибка при сохранении подписи", () =>
                {
                    var signedXml = Uri.UnescapeDataString(signature);
                    var outputDate = GetDateTimeFromXml(signedXml, "outputdate");
                    var changeDate = GetDateTimeFromXml(signedXml, "changedate");

                    SaveSigned(licenseId, l =>
                    {
                        l.OutputDate = outputDate;
                        l.ChangeDate = changeDate;
                        l.DisableGibddSend = disabled;
                        l.Signature = signedXml;
                    });
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
                    var license = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

                    var licCreationDate   = spItem.TryGetValue<DateTime>("Tm_LicenseFromDate");
                    var licTillDate       = spItem.TryGetValue<DateTime>("Tm_LicenseTillDate");
                    var dateFromCondition = (dateFrom >= licCreationDate && dateFrom <= licTillDate);
                    var dateToCondition   = (dateTo >= licCreationDate && dateTo <= licTillDate);
                    var reasonCondition   = !String.IsNullOrEmpty(Uri.UnescapeDataString(reason));

                    if (license.HasAnyChilds.HasValue && license.HasAnyChilds.Value)
                        throw new Exception("Выполнение операции неавозможно. Данное разрешение находится в работе в одном из обращений или не является последней редацией.");
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
                    var license = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

                    var licCreationDate = spItem.TryGetValue<DateTime>("Tm_LicenseFromDate");
                    var licTillDate = spItem.TryGetValue<DateTime>("Tm_LicenseTillDate");
                    var dateFromCondition = (dateFrom >= licCreationDate && dateFrom <= licTillDate);
                    var reasonCondition = !String.IsNullOrEmpty(Uri.UnescapeDataString(reason));

                    if (license.HasAnyChilds.HasValue && license.HasAnyChilds.Value)
                        throw new Exception("Выполнение операции неавозможно. Данное разрешение находится в работе в одном из обращений или не является последней редацией.");
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
                    var license = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

                    var licCreationDate = spItem.TryGetValue<DateTime>("Tm_LicenseFromDate");
                    var licTillDate = spItem.TryGetValue<DateTime>("Tm_LicenseTillDate");
                    var dateFromCondition = (dateFrom >= licCreationDate && dateFrom <= licTillDate);
                    var reasonCondition = !String.IsNullOrEmpty(Uri.UnescapeDataString(reason));

                    if (license.HasAnyChilds.HasValue && license.HasAnyChilds.Value)
                        throw new Exception("Выполнение операции неавозможно. Данное разрешение находится в работе в одном из обращений или не является последней редацией.");
                    if (!dateFromCondition)
                        throw new Exception("Указанные даты не попадают в диапазон дат разрешения");
                    if (!reasonCondition)
                        throw new Exception("Необходимо указать причину");
                });
        }

        /// <summary>
        /// Проверка целостности данных, соответствия текущих данных разрешения его состоянию на момент подписания
        /// </summary>
        /// <param name="licenseId">Идентификатор разрешения</param>
        /// <returns></returns>
        [WebMethod]
        public static dynamic ValidateLicense(int licenseId)
        {
            var results = new List<LicenseValidationData>();

            var catchData = 
                Utility.WithCatchExceptionOnWebMethod("Ошибка при проверке разрешения", () =>
                Utility.WithSPServiceContext(SPContext.Current, (serviceContextWeb) =>
                {
                    IValidator v1 = new LicenseSPDataValidator(serviceContextWeb, licenseId);
                    bool v1Valid = v1.Execute(null);
                    results.Add(new LicenseValidationData 
                    { 
                        Valid         = v1Valid,
                        DeveloperInfo = StringsRes.SPCompareDeveloperError
                    });

                    IValidator v2 = new LicenseDataValidator(serviceContextWeb, licenseId);
                    var v2Valid = v2.Execute(null);
                    results.Add(new LicenseValidationData 
                    { 
                        Valid          = v2Valid && v1Valid,
                        DeveloperInfo  = StringsRes.SQLCompareDeveloperError,
                        FailMessage    = StringsRes.SQLCompareUserError,
                        SuccessMessage = StringsRes.SQLCompareUserSuccess
                    });

                    IValidator v3 = new LicenseSignatureValidator(serviceContextWeb, licenseId);
                    var data3 = new LicenseValidationData();
                    data3.Valid = v3.Execute(null);
                    var cert = v3.GetResult() as X509Certificate2;
                    var author = LicenseHelper.GetCNFromSubjectName(cert.SubjectName.Name);
                    data3.FailMessage = StringsRes.SignatureCheckUserError;
                    data3.SuccessMessage = String.Format(StringsRes.SignatureCheckUserSuccessFmt, author);

                    results.Add(data3);
                    
                }));

            var catchDataObj = catchData as object;
            var errorDataProp = catchDataObj.GetType().GetProperty("Error");
            var errorData = errorDataProp != null ? errorDataProp.GetValue(catchDataObj, null) : null;

            return new
            {
                Error = errorData,
                Data = results
            };
        }
        
        #endregion
    }
}

