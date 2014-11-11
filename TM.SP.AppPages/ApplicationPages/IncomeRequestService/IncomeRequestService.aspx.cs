// <copyright file="IncomeRequestService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-10-30 14:36:53Z</date>

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Services;
using CamlexNET.Impl.Helpers;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint.Utilities;
using TM.Services.CoordinateV5;
using Aspose.Words;

// ReSharper disable CheckNamespace
namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using CamlexNET;
    using Utils;

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class IncomeRequestService : LayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the IncomeRequestService class
        /// </summary>
        public IncomeRequestService()
        {
            RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Проверка утверждения что все транспортные средства указанного обращения находятся в статусах, перечисленных в списке
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="statuses">Список названий статусов через точку с запятой без пробелов</param>
        /// <returns></returns>
        [WebMethod]
        public static bool IsAllTaxiInStatus(int incomeRequestId, string statuses)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");
            var arrStatuses = statuses.Split(';');

            var expressions = new List<Expression<Func<SPListItem, bool>>>();
            foreach (string t in arrStatuses)
            {
                string token = t;
                expressions.Add(x => x["Tm_TaxiStatus"] != (DataTypes.Choice) token);
            }
            expressions.Add(
                x =>
                    x["Tm_IncomeRequestLookup"] ==
                    (DataTypes.LookupId) incomeRequestId.ToString(CultureInfo.InvariantCulture));

            SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return taxiItems.Count == 0;
        }

        /// <summary>
        /// Проверка наличия в обращении хотя бы одного транспортного средства со статусом, указанным в списке
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="statuses">Список названий статусов через точку с запятой без пробелов</param>
        /// <returns></returns>
        [WebMethod]
        public static bool IsAnyTaxiInStatus(int incomeRequestId, string statuses)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");
            var arrStatuses = statuses.Split(';');

            var choicesCondition = new List<Expression<Func<SPListItem, bool>>>();
            foreach (string t in arrStatuses)
            {
                string token = t;
                choicesCondition.Add(x => x["Tm_TaxiStatus"] == (DataTypes.Choice)token);
            }
            var choicesExpr = ExpressionsHelper.CombineOr(choicesCondition);

            var parentConditions = new List<Expression<Func<SPListItem, bool>>>
            {
                x =>
                    x["Tm_IncomeRequestLookup"] ==
                    (DataTypes.LookupId) incomeRequestId.ToString(CultureInfo.InvariantCulture)
            };
            var parentExpr = ExpressionsHelper.CombineOr(parentConditions);
            var expressions = new List<Expression<Func<SPListItem, bool>>> { choicesExpr, parentExpr };

            SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return taxiItems.Count > 0;
        }

        /// <summary>
        /// Проверка наличия действующего разрешения по указанному транспортному средству
        /// </summary>
        /// <param name="taxiId">Идентификатор транспортного средства</param>
        /// <returns></returns>
        [WebMethod]
        public static bool HasTaxiActingLicense(int taxiId)
        {
            SPWeb web = SPContext.Current.Web;
            var licenseList = web.GetListOrBreak("Lists/LicenseList");

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Integer) "1",
                // checking for exactly this taxi license
                x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) taxiId.ToString(CultureInfo.InvariantCulture),
                // license status is not Аннулировано
                x => x["Tm_LicenseStatus"] != (DataTypes.Choice) "Аннулировано"
            };
            SPListItemCollection licenseItems = licenseList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return licenseItems.Count > 0;
        }

        /// <summary>
        ///  Проверка возможности выдачи разрешения по указанному транспортному средству
        /// </summary>
        /// <param name="taxiId">Идентификатор транспортного средства</param>
        /// <returns></returns>
        [WebMethod]
        public static bool CanReleaseNewLicenseForTaxi(int taxiId)
        {
            SPWeb web = SPContext.Current.Web;
            var licenseList = web.GetListOrBreak("Lists/LicenseList");

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Integer) "1",
                // checking for exactly this taxi license
                x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) taxiId.ToString(CultureInfo.InvariantCulture),
                // license status is not Аннулировано
                x => x["Tm_LicenseStatus"] != (DataTypes.Choice) "Аннулировано",
                // we can release new license only if old license expires in < then 45 days
                x => (DateTime)x["Tm_LicenseTillDate"] > DateTime.Now.AddDays(45)
            };
            SPListItemCollection licenseItems = licenseList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return licenseItems.Count == 0;
        }

        /// <summary>
        ///  Получение списка всех транпортных средства в указанном обращении
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Строка с перечислением идентификаторов транпортных средств через точку с запятой</returns>
        [WebMethod]
        public static string GetAllTaxiInRequest(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");

            SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
            {
                Query =
                    Camlex.Query()
                        .Where(
                            x =>
                                x["Tm_IncomeRequestLookup"] ==
                                (DataTypes.LookupId) incomeRequestId.ToString(CultureInfo.InvariantCulture))
                        .ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return taxiItems.Cast<SPListItem>().Aggregate(String.Empty,
                (current, item) => current + (String.IsNullOrEmpty(current) ? String.Empty : ";") + item.ID.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Получение списка всех транспортных средств обращения принятых в работу
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Строка с перечислением идентификаторов транпортных средств через точку с запятой</returns>
        [WebMethod]
        public static string GetAllWorkingTaxiInRequest(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)incomeRequestId.ToString(CultureInfo.InvariantCulture),
                x => x["Tm_TaxiStatus"] == (DataTypes.Choice)"В работе"
            };
            SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return taxiItems.Cast<SPListItem>().Aggregate(String.Empty,
                    (current, item) => current + (String.IsNullOrEmpty(current) ? String.Empty : ";") + item.ID.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Проверка возможности выдать разрешение по каждому транспортному средству, принятому в работу в указанном обращении. Ошибкой считается наличие еще действующего разрешения (+ интервал).
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Структура данных  с полями CanRelease и TaxiNumber. В TaxiNumber записывается номер первого ТС, которое не прошло проверку</returns>
        [WebMethod]
        public static dynamic CanReleaseNewLicensesForRequest(int incomeRequestId)
        {
            SPWeb web      = SPContext.Current.Web;
            var taxiList   = web.GetListOrBreak("Lists/TaxiList");
            var taxiIdList = GetAllWorkingTaxiInRequest(incomeRequestId);
            var arrTaxi    = taxiIdList.Split(';');

            string failedTaxiNumber = String.Empty;
            foreach (string taxiId in arrTaxi)
            {
                if (!CanReleaseNewLicenseForTaxi(Convert.ToInt32(taxiId)))
                {
                    var taxiItem = taxiList.GetItemById(Convert.ToInt32(taxiId));
                    failedTaxiNumber = taxiItem["Tm_TaxiStateNumber"] != null &&
                                       (string) taxiItem["Tm_TaxiStateNumber"] != String.Empty
                        ? taxiItem["Tm_TaxiStateNumber"].ToString()
                        : "Гос номер ТС не указан";
                    break;
                }
            }

            return new
            {
                CanRelease = failedTaxiNumber == String.Empty,
                TaxiNumber = failedTaxiNumber
            };
        }

        /// <summary>
        /// Проверка наличия действующего разрешения у всех транспортных средства указанного обращения, принятых в работу. Ошибкой считается отсутствие действующего разрешения.
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Структура данных  с полями CanRelease и TaxiNumber. В TaxiNumber записывается номер первого ТС, которое не прошло проверку</returns>
        [WebMethod]
        public static dynamic HasRequestActingLicenses(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");
            var taxiIdList = GetAllWorkingTaxiInRequest(incomeRequestId);
            var arrTaxi = taxiIdList.Split(';');

            string failedTaxiNumber = String.Empty;
            foreach (string taxiId in arrTaxi)
            {
                if (!HasTaxiActingLicense(Convert.ToInt32(taxiId)))
                {
                    var taxiItem = taxiList.GetItemById(Convert.ToInt32(taxiId));
                    failedTaxiNumber = taxiItem["Tm_TaxiStateNumber"] != null &&
                                       (string)taxiItem["Tm_TaxiStateNumber"] != String.Empty
                        ? taxiItem["Tm_TaxiStateNumber"].ToString()
                        : "Гос номер ТС не указан";
                    break;
                }
            }

            return new
            {
                CanRelease = failedTaxiNumber == String.Empty,
                TaxiNumber = failedTaxiNumber
            };
        }

        /// <summary>
        /// Проверка заявителя в указанном обращении на предмет факта регистрации в качестве индивидуального предпринимателя
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns></returns>
        [WebMethod]
        public static bool IsRequestDeclarantPrivateEntrepreneur(int incomeRequestId)
        {
            var retVal = false;

            SPSite curSite = SPContext.Current.Site;
            SPWeb curWeb = SPContext.Current.Web;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (var site = new SPSite(curSite.ID))
                using (var web = site.OpenWeb(curWeb.ID))
                {
                    var context = SPServiceContext.GetContext(web.Site);
                    using (new SPServiceContextScope(context))
                    {
                        var list = web.GetListOrBreak("Lists/IncomeRequestList");
                        var item = list.GetItemById(incomeRequestId);

                        var declarantId = item["Tm_RequestAccountBCSLookup"] != null ? BCS.GetBCSFieldLookupId(item, "Tm_RequestAccountBCSLookup") : null;
                        if (declarantId != null)
                        {
                            var declarant = SendRequestEGRULPage.GetRequestAccount((int) declarantId);
                            retVal = declarant.OrgFormCode == SendRequestEGRULPage.PrivateEntrepreneurCode;
                        }
                        else throw new Exception("Declarant is not specified");
                    }
                }
            });

            return retVal;
        }

        /// <summary>
        ///  Вычисление и установка сроков предоставления гос услуги, а также установка нового статуса
        ///  Сроки вычисляются для всех ситуаций кроме Отказа
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="statusCode">Новый статус</param>
        [WebMethod]
        public static void CalculateDatesAndSetStatus(int incomeRequestId, int statusCode)
        {
            SPWeb web = SPContext.Current.Web;

            web.AllowUnsafeUpdates = true;
            try
            {
                var list       = web.GetListOrBreak("Lists/IncomeRequestList");
                var statusList = web.GetListOrBreak("Lists/IncomeRequestStateBookList");
                var item       = list.GetItemById(incomeRequestId);
                var statusItem = statusList.GetSingleListItemByFieldValue("Tm_ServiceCode", statusCode.ToString(CultureInfo.InvariantCulture));

                // В случае отказа сроки проедоставления услуги не рассчитываем
                if (statusCode != 1080)
                    item["Tm_PrepareTargetDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddDays(14).Date);
                if (statusItem != null)
                    item["Tm_IncomeRequestStateLookup"] = new SPFieldLookupValue(statusItem.ID, statusItem.Title);

                item.Update();
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
        }

        /// <summary>
        ///  Получение xml сообщения статуса обращения по V5
        /// </summary>
        /// <param name="incomeRequestId">Идентифифкатор обращения</param>
        /// <returns>Сообщение в виде xml</returns>
        [WebMethod]
        public static string GetIncomeRequestCoordinateV5StatusMessage(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;
            var rList = web.GetListOrBreak("Lists/IncomeRequestList");
            // request item
            SPListItem rItem = rList.GetItemOrBreak(incomeRequestId);
            var sNumber = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // status lookup item
            var stList = web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var stItemId = rItem["Tm_IncomeRequestStateLookup"] != null ? new SPFieldLookupValue(rItem["Tm_IncomeRequestStateLookup"].ToString()).LookupId : 0;
            var stItem = stList.GetItemOrNull(stItemId);
            var stCode = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            var message = new CoordinateStatusMessage
            {
                ServiceHeader = new Headers
                {
                    FromOrgCode     = Consts.TaxoMotorSysCode,
                    ToOrgCode       = Consts.AsgufSysCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = sNumber
                },
                StatusMessage = new CoordinateStatusData
                {
                    ServiceNumber = sNumber,
                    StatusCode    = Convert.ToInt32(stCode),
                }
            };

            return message.ToXElement<CoordinateStatusMessage>().ToString();
        }

        /// <summary>
        ///  Сохранение подписанного состояния обращения в список IncomeRequestStatusLogList
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="signature">Xml подписи</param>
        [WebMethod]
        public static void SaveIncomeRequestStatusLog(int incomeRequestId, string signature)
        {
            SPWeb web         = SPContext.Current.Web;
            SPList spList     = web.GetListOrBreak("Lists/IncomeRequestList");
            SPList logList    = web.GetListOrBreak("Lists/IncomeRequestStatusLogList");
            SPListItem spItem = spList.GetItemOrBreak(incomeRequestId);

            if (spItem["Tm_IncomeRequestStateLookup"] == null)
                throw new Exception("SaveIncomeRequestStatusLog: Income request state not defined");

            web.AllowUnsafeUpdates = true;
            try
            {
                string yearStr  = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                string monthstr = DateTime.Now.ToString("MMM", CultureInfo.CurrentCulture);
                string num      = spItem.Title.ToString(CultureInfo.InvariantCulture);

                SPFolder parentFolder = logList.RootFolder.CreateSubFolders(new[] { yearStr, monthstr, num });
                SPListItem newLogItem = logList.AddItem(parentFolder.ServerRelativeUrl, SPFileSystemObjectType.File);

                newLogItem["Title"] = new SPFieldLookupValue(spItem["Tm_IncomeRequestStateLookup"].ToString()).LookupValue;
                newLogItem["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(spItem.ID, spItem.Title);
                newLogItem["Tm_IncomeRequestStateLookup"] = spItem["Tm_IncomeRequestStateLookup"];
                newLogItem["Tm_XmlValue"] = Uri.UnescapeDataString(signature);
                newLogItem.Update();
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }

        }

        /// <summary>
        /// Установка причины отказа по обращению и комментария к отказу
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="refuseReasonCode">Код причины отказа</param>
        /// <param name="refuseComment">Текст комментария к отказу</param>
        [WebMethod]
        public static void SetRefuseReasonAndComment(int incomeRequestId, int refuseReasonCode, string refuseComment)
        {
            SPWeb web = SPContext.Current.Web;

            web.AllowUnsafeUpdates = true;
            try
            {
                var list = web.GetListOrBreak("Lists/IncomeRequestList");
                var refuseList = web.GetListOrBreak("Lists/DenyReasonBookList");
                var item = list.GetItemById(incomeRequestId);
                var refuseItem = refuseList.GetSingleListItemByFieldValue("Tm_ServiceCode", refuseReasonCode.ToString(CultureInfo.InvariantCulture));

                if (refuseItem != null)
                    item["Tm_DenyReasonLookup"] = new SPFieldLookupValue(refuseItem.ID, refuseItem.Title);
                item["Tm_Comment"] = refuseComment;
                item["Tm_RefuseDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date);

                item.Update();
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
        }

        /// <summary>
        /// Генерация документа уведомления об отказе по обращению
        /// </summary>
        /// <param name="refusedIncomeRequestId">Идентификатор обращения по которому отказано</param>
        /// <returns>Структура данных содержащщая поля: Идентификатор созданного документа в библиотеке AttachLib, Адрес документа в библиотеке AttachLib</returns>
        [WebMethod]
        public static dynamic CreateIncomeRequestRefuseNotifyDocument(int refusedIncomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;

            web.AllowUnsafeUpdates = true;
            try
            {
                // getting data
                var spList    = web.GetListOrBreak("Lists/IncomeRequestList");
                var spItem    = spList.GetItemOrBreak(refusedIncomeRequestId);
                var tmplLib   = web.GetListOrBreak("DocumentTemplateLib");
                var tmplItem  = tmplLib.GetSingleListItemByFieldValue("Tm_ServiceCode", "1");
                var attachLib = web.GetListOrBreak("AttachLib");

                SPListItem requestedDocument;
                Utility.TryGetListItemFromLookupValue(spItem["Tm_RequestedDocument"],
                    spItem.Fields.GetFieldByInternalName("Tm_RequestedDocument") as SPFieldLookup, out requestedDocument);
                SPListItem denyReason;
                Utility.TryGetListItemFromLookupValue(spItem["Tm_DenyReasonLookup"],
                    spItem.Fields.GetFieldByInternalName("Tm_DenyReasonLookup") as SPFieldLookup, out denyReason);

                var refuseDate = spItem["Tm_RefuseDate"] != null
                    ? DateTime.Parse(spItem["Tm_RefuseDate"].ToString()).ToString("dd.MM.yyyy")
                    : "Дата отказа не указана";
                var regDate = spItem["Tm_RegistrationDate"] != null
                    ? DateTime.Parse(spItem["Tm_RegistrationDate"].ToString()).ToString("dd.MM.yyyy")
                    : "Дата регистрации не указана";

                // getting Aspose license for generating PDF
                var asposeLicense = new License();
                asposeLicense.SetLicense("Aspose.Total.lic");

                var doc = new Document(tmplItem.File.OpenBinaryStream());
                doc.MailMerge.Execute(
                    new[]
                    {
                        "RefuseDate", "DeclarantName", "CreationDate", "SingleNumber", "SubServiceName",
                        "RefuseReasonTitle", "RefuseReasonText", "OperatorDepartment", "OperatorName"
                    },
                    new[]
                    {
                        refuseDate, 
                        spItem["Tm_RequestAccountBCSLookup"] ?? "", 
                        regDate, spItem["Tm_SingleNumber"] ?? "",
                        requestedDocument != null ? requestedDocument.Title : "", 
                        denyReason != null ? denyReason.Title : "", 
                        spItem["Tm_Comment"] ?? "", 
                        "", 
                        web.CurrentUser.Name
                    });

                using (var ms = new MemoryStream())
                {
                    doc.Save(ms, SaveFormat.Pdf);

                    var parentFolder = attachLib.RootFolder.CreateSubFolders(new[] { 
                        DateTime.Now.Year.ToString(CultureInfo.InvariantCulture), 
                        DateTime.Now.Month.ToString(CultureInfo.InvariantCulture), 
                        spItem.Title });

                    var uplFolder = parentFolder.CreateSubFolders(new[] { (parentFolder.ItemCount + 1).ToString(CultureInfo.InvariantCulture) });
                    var fn = Path.HasExtension(tmplItem.File.Name) ? Path.ChangeExtension(tmplItem.File.Name, "pdf") : tmplItem.File.Name + ".pdf";
                    var templatedDoc = uplFolder.Files.Add(fn, ms);
                    uplFolder.Update();

                    templatedDoc.Item["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(spItem.ID, spItem.Title);
                    templatedDoc.Item.Update();

                    return new
                    {
                       templatedDoc.Item.ID, 
                       templatedDoc.ServerRelativeUrl
                    };
                }
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
        }

        [WebMethod]
        public static int SaveDocumentDetachedSignature(int documentId, string signature)
        {
            SPWeb web = SPContext.Current.Web;
            var retVal = 0;

            web.AllowUnsafeUpdates = true;
            try
            {
                // getting data
                var attachLib   = web.GetListOrBreak("AttachLib");
                var attachItem  = attachLib.GetItemById(documentId);
                var sigFileName = attachItem.File.Name + ".sig";

                var uplFolder = attachItem.File.ParentFolder;

                SPFile sigFile = uplFolder.Files.Add(sigFileName, Convert.FromBase64String(signature));
                uplFolder.Update();

                sigFile.Item["Tm_IncomeRequestLookup"] = attachItem["Tm_IncomeRequestLookup"];
                sigFile.Item.Update();
                retVal = sigFile.Item.ID;
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }

            return retVal;
        }

        /// <summary>
        /// Удаление черновиков разрешений для всех ТС указанного обращения
        /// </summary>
        /// <param name="refusedIncomeRequestId">Идентификатор обращения, по которому отказано</param>
        [WebMethod]
        public static void DeleteLicenseDraftsOnRefusing(int refusedIncomeRequestId)
        {
            SPSite curSite = SPContext.Current.Site;
            SPWeb curWeb = SPContext.Current.Web;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (var site = new SPSite(curSite.ID))
                using (var web = site.OpenWeb(curWeb.ID))
                {
                    var context = SPServiceContext.GetContext(web.Site);
                    using (new SPServiceContextScope(context))
                    {
                        var allTaxiStr = GetAllTaxiInRequest(refusedIncomeRequestId);
                        string[] allTaxiArr = allTaxiStr.Split(';');
                        foreach (string taxiId in allTaxiArr)
                        {
                            BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                            {
                                lob         = BCS.LOBTaxiSystemName,
                                ns          = BCS.LOBTaxiSystemNamespace,
                                contentType = "License",
                                methodName  = "DeleteLicenseDraftForSPTaxiIdInstance",
                                methodType  = MethodInstanceType.Updater
                            }, Convert.ToInt32(taxiId));
                        }

                    }
                }
            });
        }
    }
}

