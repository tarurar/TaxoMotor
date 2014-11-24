﻿// <copyright file="IncomeRequestService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-10-30 14:36:53Z</date>

using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using CamlexNET.Impl.Helpers;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint.Utilities;
using TM.Services.CoordinateV5;
using TM.SP.BCSModels;
using TM.SP.BCSModels.CoordinateV5;
using Aspose.Words;
using Address = TM.SP.BCSModels.CoordinateV5.Address;
using License = TM.SP.BCSModels.Taxi.License;
using AsposeLicense = Aspose.Words.License;
using RequestContact = TM.SP.BCSModels.CoordinateV5.RequestContact;

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

    public struct LicenseXml
    {
        public int ExternalId;
        public string Xml;
    }

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
        /// Получение списка всех транспортных средств обращения в указанном статусе
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Статус ТС</returns>
        [WebMethod]
        public static string GetAllTaxiInRequestByStatus(int incomeRequestId, string status)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)incomeRequestId.ToString(CultureInfo.InvariantCulture),
                x => x["Tm_TaxiStatus"] == (DataTypes.Choice)status
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
        /// Получение списка всех транспортных средств обращения принятых в работу
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Строка с перечислением идентификаторов транпортных средств через точку с запятой</returns>
        [WebMethod]
        public static string GetAllWorkingTaxiInRequest(int incomeRequestId)
        {
            return GetAllTaxiInRequestByStatus(incomeRequestId, "В работе");
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
                    FromOrgCode     = Consts.TaxoMotorDepCode,
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
        /// Генерация документов по обращению при его закрытии
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Массив структур DocumentMetaData</returns>
        [WebMethod]
        public static DocumentMetaData[] CreateDocumentsWhileClosing(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;
            var retValList = new List<DocumentMetaData>();

            Utility.WithSafeUpdate(web, (safeWeb) =>
            {
                // getting data
                var spList = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                var spItem = spList.GetItemOrBreak(incomeRequestId);
                var ctId   = new SPContentTypeId(spItem["ContentTypeId"].ToString());

                var docBuilder = new TemplatedDocumentBuilder(safeWeb, incomeRequestId);

                if (IsAllTaxiInStatus(incomeRequestId, "Решено положительно"))
                {
                    retValList.Add(ctId == spList.ContentTypes["Аннулирование"].Id
                        ? docBuilder.RenderDocument(4)
                        : docBuilder.RenderDocument(5));
                }
                else if (IsAnyTaxiInStatus(incomeRequestId, "Отказано"))
                {
                    retValList.Add(ctId == spList.ContentTypes["Аннулирование"].Id
                        ? docBuilder.RenderDocument(4)
                        : docBuilder.RenderDocument(5));
                    retValList.Add(docBuilder.RenderDocument(6));
                }
            });

            return retValList.ToArray();
        }

        /// <summary>
        /// Генерация документов по обращению при отказе
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Массив структур DocumentMetaData</returns>
        [WebMethod]
        public static DocumentMetaData[] CreateDocumentsWhileRefusing(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;
            var retValList = new List<DocumentMetaData>();

            Utility.WithSafeUpdate(web, (safeWeb) =>
            {
                // getting data
                var spList = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                var spItem = spList.GetItemOrBreak(incomeRequestId);
                var ctId = new SPContentTypeId(spItem["ContentTypeId"].ToString());

                var docBuilder = new TemplatedDocumentBuilder(safeWeb, incomeRequestId);

                var status = docBuilder.RequestStatus;
                if (status != null)
                {
                    var statusCode = status["Tm_ServiceCode"] != null
                        ? status["Tm_ServiceCode"].ToString()
                        : String.Empty;

                    switch (statusCode)
                    {
                        case "1020":
                            retValList.Add(docBuilder.NeedPersonVisit
                                ? docBuilder.RenderDocument(1)
                                : docBuilder.RenderDocument(2));
                            break;
                        case "1110":
                        case "6420":
                            retValList.Add(docBuilder.RenderDocument(3));
                            break;
                    }
                }
            });

            return retValList.ToArray();
        }


        /// <summary>
        /// Сохранение открепленной цифровой подписи для документа
        /// </summary>
        /// <param name="documentId">Идентификатор документа в библиотеке AttachLib</param>
        /// <param name="signature">Значение подписи</param>
        /// <returns></returns>
        [WebMethod]
        public static int SaveDocumentDetachedSignature(int documentId, string signature)
        {
            SPWeb web = SPContext.Current.Web;
            int retVal;

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

        /// <summary>
        /// Удаление черновиков разрешений для ТС указанного обращения в указанном статусе
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// /// <param name="status">Статус ТС</param>
        [WebMethod]
        public static dynamic DeleteLicenseDraftsByTaxiStatus(int incomeRequestId, string status)
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при удалении черновиков разрешений", () =>
                    Utility.WithSPServiceContext(SPContext.Current, (serviceContextWeb) =>
                        Utility.WithSafeUpdate(serviceContextWeb, (safeWeb) =>
                        {
                            var allTaxiStr = GetAllTaxiInRequestByStatus(incomeRequestId, status);
                            if (!String.IsNullOrEmpty(allTaxiStr))
                            {
                                string[] allTaxiArr = allTaxiStr.Split(';');
                                foreach (string taxiId in allTaxiArr)
                                {
                                    BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                                    {
                                        lob = BCS.LOBTaxiSystemName,
                                        ns = BCS.LOBTaxiSystemNamespace,
                                        contentType = "License",
                                        methodName = "DeleteLicenseDraftForSPTaxiIdInstance",
                                        methodType = MethodInstanceType.Updater
                                    }, Convert.ToInt32(taxiId));
                                }    
                            }
                        })));
        }


        /// <summary>
        /// Установка статуса обращения при его закрытии
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns></returns>
        [WebMethod]
        public static dynamic SetStatusOnClosing(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;

            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка обновления статуса при закрытии обращения", () =>
                    Utility.WithSafeUpdate(web, (safeWeb) =>
                    {
                        var list = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                        var item = list.GetItemById(incomeRequestId);
                        var statusList = safeWeb.GetListOrBreak("Lists/IncomeRequestStateBookList");

                        SPListItem newStatus = statusList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                            IsAnyTaxiInStatus(incomeRequestId, "Отказано") ? "1085" : "1075");

                        if (newStatus != null)
                            item["Tm_IncomeRequestStateLookup"] = new SPFieldLookupValue(newStatus.ID, newStatus.Title);
                        item["Tm_PrepareFactDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date);
                        item.Update();
                    }));
        }

        /// <summary>
        /// Принятие ТС в работу
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="taxiIdList">Строка с перечислением идентификаторов транпортных средств через точку с запятой</param>
        /// <returns>Структура с описанием ошибки и стеком для разработчика</returns>
        [WebMethod]
        public static dynamic AcceptTaxi(int incomeRequestId, string taxiIdList)
        {
            return Utility.WithCatchExceptionOnWebMethod("Ошибка при принятии транспортного средства", () => 
                   Utility.WithSPServiceContext(SPContext.Current, (serviceContextWeb) => 
                   Utility.WithSafeUpdate(serviceContextWeb, (safeWeb) =>
            {
                var rList = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                var rItem = rList.GetItemOrBreak(incomeRequestId);
                var taxiList = safeWeb.GetListOrBreak("Lists/TaxiList");
                var taxiIdArr = taxiIdList.Split(';');

                SPListItem rStatus;
                Utility.TryGetListItemFromLookupValue(rItem["Tm_IncomeRequestStateLookup"],
                    rList.Fields.GetFieldByInternalName("Tm_IncomeRequestStateLookup") as SPFieldLookup, out rStatus);

                if (rStatus == null)
                    throw new Exception("У обращения должно быть установлено значение статуса");
                var rStatusCode = rStatus["Tm_ServiceCode"] != null
                    ? rStatus["Tm_ServiceCode"].ToString()
                    : String.Empty;

                foreach (var taxiItem in taxiIdArr.Select(taxiId => taxiList.GetItemById(Convert.ToInt32(taxiId))))
                {
                    switch (rStatusCode)
                    {
                        case "1020":
                            taxiItem["Tm_TaxiStatus"] = "В работе";
                            taxiItem.Update();
                            break;
                        case "6420":
                            var ctId = new SPContentTypeId(rItem["ContentTypeId"].ToString());
                            var licenseDraft = new License
                            {
                                Status = (int)LicenseStatus.Draft,
                                TaxiId = taxiItem.ID
                            };

                            if (ctId == rList.ContentTypes["Новое"].Id)
                            {
                                var storedLicenseDraft = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                                {
                                    lob         = BCS.LOBTaxiSystemName,
                                    ns          = BCS.LOBTaxiSystemNamespace,
                                    contentType = "License",
                                    methodName  = "CreateLicense",
                                    methodType  = MethodInstanceType.Creator
                                }, licenseDraft);
                                if (storedLicenseDraft != null)
                                    taxiItem["Tm_TaxiPrevLicenseNumber"] = storedLicenseDraft.RegNumber;
                            }
                            else
                            {
                                var regNumber = taxiItem["Tm_TaxiPrevLicenseNumber"];
                                if (regNumber != null && regNumber.ToString() != String.Empty)
                                {
                                    licenseDraft.RegNumber = regNumber.ToString();
                                    BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                                    {
                                        lob         = BCS.LOBTaxiSystemName,
                                        ns          = BCS.LOBTaxiSystemNamespace,
                                        contentType = "License",
                                        methodName  = "CreateLicense",
                                        methodType  = MethodInstanceType.Creator
                                    }, licenseDraft);
                                }
                                else
                                    throw new Exception("В транспортном средстве не указан номер ранее выданного разрешения");
                            }

                            taxiItem["Tm_TaxiStatus"] = "Решено положительно";
                            taxiItem.Update();
                            break;
                    }
                }
            })));
        }

        /// <summary>
        /// Отказ по ТС
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="taxiIdList">Строка с перечислением идентификаторов транпортных средств через точку с запятой</param>
        /// <param name="refuseReasonCode">Код причины отказа</param>
        /// <param name="refuseComment">Комментарий к отказу</param>
        /// <param name="needPersonVisit">Требуется ли очный визит</param>
        /// <returns>Структура с описанием ошибки и стеком для разработчика</returns>
        [WebMethod]
        public static dynamic RefuseTaxi(int incomeRequestId, string taxiIdList, int refuseReasonCode, string refuseComment,
            bool needPersonVisit)
        {
            return Utility.WithCatchExceptionOnWebMethod("Ошибка при отказе по транспортному средству", () => 
                   Utility.WithSafeUpdate(SPContext.Current.Web, (safeWeb) =>
            {
                var taxiList = safeWeb.GetListOrBreak("Lists/TaxiList");
                var refuseList = safeWeb.GetListOrBreak("Lists/DenyReasonBookList");
                var refuseItem = refuseList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                    refuseReasonCode.ToString(CultureInfo.InvariantCulture));

                var taxiIdArr = taxiIdList.Split(';');

                foreach (var taxiItem in taxiIdArr.Select(taxiId => taxiList.GetItemById(Convert.ToInt32(taxiId))))
                {
                    taxiItem["Tm_DenyReasonLookup"] = new SPFieldLookupValue(refuseItem.ID, refuseItem.Title);
                    taxiItem["Tm_TaxiDenyComment"] = refuseComment;
                    taxiItem["Tm_NeedPersonVisit"] = needPersonVisit;
                    taxiItem["Tm_TaxiStatus"] = "Отказано";
                    taxiItem.Update();
                }
            }));
        }

        /// <summary>
        /// Все ли транспортные средства обращения, находящиеся в указанном статусе, имеют указанный номер и серию бланка
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="status">Статус ТС</param>
        /// <returns></returns>
        [WebMethod]
        public static bool IsAllTaxiInStatusHasBlankNo(int incomeRequestId, string status)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");

            var choicesCondition = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_TaxiStatus"] == (DataTypes.Choice)status
            };
            var choicesExpr = ExpressionsHelper.CombineOr(choicesCondition);

            var parentCondition = new List<Expression<Func<SPListItem, bool>>>
            {
                x =>
                    x["Tm_IncomeRequestLookup"] ==
                    (DataTypes.LookupId) incomeRequestId.ToString(CultureInfo.InvariantCulture)
            };
            var parentExpr = ExpressionsHelper.CombineOr(parentCondition);

            var blankNoCondition = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_BlankNo"] == null,
                x => x["Tm_BlankSeries"] == null
            };
            var blankExpr = ExpressionsHelper.CombineOr(blankNoCondition);
            var expressions = new List<Expression<Func<SPListItem, bool>>> { choicesExpr, parentExpr, blankExpr };

            SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return taxiItems.Count == 0;
        }

        /// <summary>
        /// Перевод разрешения из черновика в один из "действующих" статусов в зависимости от ситуации
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Информация об ошибке</returns>
        [WebMethod]
        public static dynamic PromoteLicenseDrafts(int incomeRequestId)
        {
            var externalIdList = new List<int>();

            var catchData = 
                Utility.WithCatchExceptionOnWebMethod("Ошибка при обновлении черновиков разрешений", () =>
                    Utility.WithSPServiceContext(SPContext.Current, serviceContextWeb =>
                        Utility.WithSafeUpdate(serviceContextWeb, (safeWeb) =>
                        {
                            var taxiList = safeWeb.GetListOrBreak("Lists/TaxiList");
                            var licenseList = safeWeb.GetListOrBreak("Lists/LicenseList");

                            var taxiIdList = GetAllTaxiInRequestByStatus(incomeRequestId, "Решено положительно");
                            var taxiIdArr = taxiIdList.Split(';');

                            foreach (
                                var taxiItem in
                                    taxiIdArr.Select(taxiId => taxiList.GetItemById(Convert.ToInt32(taxiId))))
                            {
                                var externalId = LicenseHelper.PromoteDraftFor(safeWeb, incomeRequestId, taxiItem.ID);
                                // immediately execute migration on item
                                var license = LicenseService.GetLicense(externalId);
                                LicenseService.MigrateItem(licenseList, license);
                                externalIdList.Add(externalId);
                            }
                        })));

            var payLoad = String.Join(";", externalIdList.Select(el => el.ToString(CultureInfo.InvariantCulture)));

            var catchDataObj  = catchData as object;
            var errorDataProp = catchDataObj.GetType().GetProperty("Error");
            var errorData     = errorDataProp != null ? errorDataProp.GetValue(catchDataObj, null) : null;

            return new
            {
                Error = errorData,
                Data = payLoad
            };
        }

        /// <summary>
        /// Формирование xml представления ВНЕШНИХ разрешений по списку ВНЕШНИХ идентификаторов
        /// </summary>
        /// <param name="licenseIdList">Список ВНЕШНИХ идентификаторов разрешений через точку с запятой</param>
        /// <returns>Информация об ошибке</returns>
        [WebMethod]
        public static dynamic GetLicenseXmlById(string licenseIdList)
        {
            var licenseXmlList = new List<LicenseXml>();

            var catchData =
                Utility.WithCatchExceptionOnWebMethod("Ошибка при сериализации разрешений", () =>
                    Utility.WithSPServiceContext(SPContext.Current, web =>
                    {
                        var licenseIdArr = licenseIdList.Split(';');

                        foreach (string licenseId in licenseIdArr)
                        {
                            var license = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                            {
                                lob         = BCS.LOBTaxiSystemName,
                                ns          = BCS.LOBTaxiSystemNamespace,
                                contentType = "License",
                                methodName  = "ReadLicenseItem",
                                methodType  = MethodInstanceType.SpecificFinder
                            }, Convert.ToInt32(licenseId));

                            if (license == null) continue;

                            //serialization
                            var intWriter    = new StringWriter(new StringBuilder());
                            XmlWriter writer = new XmlTextWriter(intWriter);
                            var serializer   = new XmlSerializer(typeof(License));
                            writer.WriteStartElement("Data");
                            serializer.Serialize(writer, license);
                            writer.WriteEndElement();

                            licenseXmlList.Add(new LicenseXml
                            {
                                ExternalId = Convert.ToInt32(licenseId),
                                Xml = intWriter.ToString()
                            });
                        }
                    }));

            var payLoad = licenseXmlList.ToArray();

            var catchDataObj = catchData as object;
            var errorDataProp = catchDataObj.GetType().GetProperty("Error");
            var errorData = errorDataProp != null ? errorDataProp.GetValue(catchDataObj, null) : null;

            return new
            {
                Error = errorData,
                Data = payLoad
            };
        }

        /// <summary>
        /// Обновление подписи для указанного разрешения во ВНЕШНЕМ источнике данных
        /// </summary>
        /// <param name="licenseId">ВНЕШНИЙ идентификатор разрешения</param>
        /// <param name="signature">Подпись</param>
        /// <returns>Информация об ошибке</returns>
        [WebMethod]
        public static dynamic UpdateSignatureForLicense(int licenseId, string signature)
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при обновлении подписи в разрешении", () =>
                    Utility.WithSPServiceContext(SPContext.Current, serviceContextWeb =>
                        Utility.WithSafeUpdate(serviceContextWeb, safeWeb =>
                        {
                            var license = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                            {
                                lob         = BCS.LOBTaxiSystemName,
                                ns          = BCS.LOBTaxiSystemNamespace,
                                contentType = "License",
                                methodName  = "ReadLicenseItem",
                                methodType  = MethodInstanceType.SpecificFinder
                            }, licenseId);

                            if (license != null)
                            {
                                license.Signature = Uri.UnescapeDataString(signature);

                                BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                                {
                                    lob         = BCS.LOBTaxiSystemName,
                                    ns          = BCS.LOBTaxiSystemNamespace,
                                    contentType = "License",
                                    methodName  = "UpdateLicense",
                                    methodType  = MethodInstanceType.Updater
                                }, license);
                            }
                            else throw new Exception("Разрешение не найдено");

                        })));
        }

        /// <summary>
        /// Обновление исходящих межведомственных запросов при закрытии обращения
        /// </summary>
        /// <param name="closingIncomeRequestId">Идентификатор обращения</param>
        /// <returns>Информация об ошибке</returns>
        [WebMethod]
        public static dynamic UpdateOutcomeRequestsOnClosing(int closingIncomeRequestId)
        {
            var web = SPContext.Current.Web;

            return
                Utility.WithCatchExceptionOnWebMethod("Обновление межвед. запросов при закрытии обращения", () =>
                    Utility.WithSafeUpdate(web, safeWeb =>
                    {
                        var licList          = safeWeb.GetListOrBreak("Lists/LicenseList");
                        var outRequestStList = safeWeb.GetListOrBreak("Lists/OutcomeRequestStateList");
                        var taxiIdList       = GetAllTaxiInRequestByStatus(closingIncomeRequestId, "Решено положительно");
                        var taxiIdArr        = taxiIdList.Split(';');
                        // iterating throw all the taxi items in the closing income request that were given a license
                        foreach (var taxiId in taxiIdArr)
                        {
                            var id = taxiId;
                            // at that moment we have to be sure that our new licenses have already been migrated to sp list
                            var licItems = licList.GetItems(new SPQuery
                            {
                                Query =
                                    Camlex.Query()
                                        .Where(x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) id)
                                        .OrderBy(x => x["Created"] as Camlex.Desc)
                                        .ToString(),
                                ViewAttributes = "Scope='RecursiveAll'"
                            });
                            // in case we have more than one license (by the way, this is error) we will take the latest one by date
                            var lic = licItems.Count > 0 ? licItems[0] : null;

                            if (lic != null && lic["Tm_LicenseRtParentLicenseLookup"] != null)
                            {
                                // we need the root parent's link
                                var licRootParent = lic["Tm_LicenseRtParentLicenseLookup"];
                                var incomeRequestIdStr = closingIncomeRequestId.ToString(CultureInfo.InvariantCulture);
                                // searching for outcome requests by current taxi and income request
                                var expressions = new List<Expression<Func<SPListItem, bool>>>
                                {
                                    x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) id,
                                    x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId) incomeRequestIdStr,
                                };

                                SPListItemCollection outRequests = outRequestStList.GetItems(new SPQuery
                                {
                                    Query = Camlex.Query().WhereAll(expressions).ToString(),
                                    ViewAttributes = "Scope='RecursiveAll'"
                                });
                                // setting links for all outcome requests that we have found
                                foreach (SPListItem outRequestItem in outRequests)
                                {
                                    outRequestItem["Tm_LicenseLookup"] = licRootParent;
                                    outRequestItem.SystemUpdate();
                                }
                            }
                        }
                    }));
        }
    }
}

