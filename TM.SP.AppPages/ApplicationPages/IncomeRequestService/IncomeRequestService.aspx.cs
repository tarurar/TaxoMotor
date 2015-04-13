// <copyright file="IncomeRequestService.aspx.cs" company="Armd">
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
using MessageQueueService = TM.ServiceClients.MessageQueue;
using TM.SP.AppPages.Validators;

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

    // interface class for client scripts
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
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Number) "1",
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
        /// Выборка действующих разрещений (на практике должно быть одно) для указанного транспортного средства (проверяется номер ТС)
        /// </summary>
        /// <param name="taxiId">Идентификатор транспортного средства</param>
        /// <returns></returns>
        [WebMethod]
        public static SPListItemCollection GetTaxiNumberActingLicenses(int taxiId)
        {
            SPWeb web = SPContext.Current.Web;
            var licenseList = web.GetListOrBreak("Lists/LicenseList");
            var taxiList = web.GetListOrBreak("Lists/TaxiList");
            var taxiItem = taxiList.GetItemById(taxiId);
            var stateNumber = taxiItem["Tm_TaxiStateNumber"].ToString();

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Number) "1",
                // checking for exactly this taxi
                x => (string)x["Tm_TaxiStateNumber"] == stateNumber
            };
            SPListItemCollection licenseItems = licenseList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return licenseItems;
        }

        /// <summary>
        /// Получение списка всех обращений в указанных статусах
        /// </summary>
        /// <param name="statuses">Список кодов статусов через точку с запятой</param>
        /// <returns></returns>
        [WebMethod]
        public static SPListItemCollection GetAllIncomeRequestInStatus(string statuses)
        {
            SPWeb web = SPContext.Current.Web;
            var requestList = web.GetListOrBreak("Lists/IncomeRequestList");
            var arrStatuses = statuses.Split(';');

            var statusConditions = new List<Expression<Func<SPListItem, bool>>>();
            foreach (string t in arrStatuses)
            {
                string token = t;
                statusConditions.Add(x => (string)x["_x0421__x043e__x0441__x0442__x04"] == t);
            }

            var statusExpr = ExpressionsHelper.CombineOr(statusConditions);
            var expressions = new List<Expression<Func<SPListItem, bool>>> { statusExpr };

            SPListItemCollection requestItems = requestList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return requestItems;
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
            var taxiList    = web.GetListOrBreak("Lists/TaxiList");
            var taxiItem    = taxiList.GetItemById(taxiId);
            var taxiStNum  = taxiItem["Tm_TaxiStateNumber"];
            if (taxiStNum == null) throw new Exception("Необходимо указать гос. рег. знак ТС");

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Number) "1",

                // checking for exactly this taxi license
                // x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) taxiId.ToString(CultureInfo.InvariantCulture),

                x => (string)x["Tm_TaxiStateNumber"] == taxiStNum.ToString(),

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
        /// <param name="status">Статус ТС</param>
        /// <returns>Строка с перечислением идентификаторов транпортных средств через точку с запятой</returns>
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
                ViewAttributes = "Scope='Recursive'"
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
        /// Проверка наличия ТС с указанным номером в обращении
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// /// <param name="taxiStateNumber">Гос номер ТС</param>
        /// <returns></returns>
        [WebMethod]
        public static bool HasRequestTaxiStateNumber(int incomeRequestId, string taxiStateNumber)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)incomeRequestId.ToString(),
                x => (string)x["Tm_TaxiStateNumber"] == taxiStateNumber
            };

            SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return taxiItems.Count > 0;

        }

        /// <summary>
        /// Проверка наличия ТС с указанным номером ранее выданного разрешения в обращении
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// /// <param name="taxiLicNumber">Номер ранее выданного разрешения в формате [00000]</param>
        /// <returns></returns>
        [WebMethod]
        public static bool HasRequestTaxiLicenseNumber(int incomeRequestId, string taxiLicNumber)
        {
            SPWeb web = SPContext.Current.Web;
            var taxiList = web.GetListOrBreak("Lists/TaxiList");

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)incomeRequestId.ToString(),
                x => (string)x["Tm_TaxiPrevLicenseNumber"] == taxiLicNumber
            };

            SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            return taxiItems.Count > 0;

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
        ///  Вычисление и установка сроков предоставления гос услуги, а также установка нового статуса, генерация внутреннего регистрационного номера
        ///  Сроки вычисляются для всех ситуаций кроме Отказа
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="statusCode">Новый статус</param>
        [WebMethod]
        public static void CalculateDatesAndSetStatus(int incomeRequestId, int statusCode)
        {
            Utility.WithSPServiceContext(SPContext.Current, (serviceContextWeb) =>
            Utility.WithSafeUpdate(serviceContextWeb, (safeWeb) =>
            {
                var list = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                var statusList = safeWeb.GetListOrBreak("Lists/IncomeRequestStateBookList");
                var item = list.GetItemById(incomeRequestId);
                var statusItem = statusList.GetSingleListItemByFieldValue("Tm_ServiceCode", statusCode.ToString(CultureInfo.InvariantCulture));
                var ctId = new SPContentTypeId(item["ContentTypeId"].ToString());

                DateTime applyDate = DateTime.Now.Date;
                // В случае отказа сроки предоставления услуги не рассчитываем
                if (statusCode != 1080)
                {
                    DateTime prepDate, outpDate;
                    DateTime beginWorkDate = applyDate.AddDays(1);

                    if (ctId == list.ContentTypes["Аннулирование"].Id)
                    {
                        prepDate = Calendar.CalcFinishDate(safeWeb, beginWorkDate, 1);
                        outpDate = Calendar.CalcFinishDate(safeWeb, beginWorkDate, 2);
                    }
                    else if (ctId == list.ContentTypes["Выдача дубликата"].Id)
                    {
                        prepDate = Calendar.CalcFinishDate(safeWeb, beginWorkDate, 5);
                        outpDate = Calendar.CalcFinishDate(safeWeb, beginWorkDate, 6);
                    }
                    else
                    {
                        prepDate = Calendar.CalcFinishDate(safeWeb, beginWorkDate, 10);
                        outpDate = Calendar.CalcFinishDate(safeWeb, beginWorkDate, 11);
                    }

                    item["Tm_PrepareTargetDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(prepDate);
                    item["Tm_OutputTargetDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(outpDate);
                }
                if (statusItem != null)
                    item["Tm_IncomeRequestStateLookup"] = new SPFieldLookupValue(statusItem.ID, statusItem.Title);
                // Присвоение внутреннего регистрационного номера только если он еще не задан
                var number = item.TryGetValue<string>("Tm_InternalRegNumber");
                if (String.IsNullOrEmpty(number))
                {
                    item["Tm_InternalRegNumber"] = Utility.GetIncomeRequestInternalRegNumber("InternalRegNumber");
                }
                // дату регистрации указываем только если она еще не задана
                var regDate = item.TryGetValueOrNull<DateTime>("Tm_ApplyDate");
                if (regDate == null)
                {
                    item["Tm_ApplyDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(applyDate);
                }

                item.Update();                            
            }));
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
            return IncomeRequestHelper.GetIncomeRequestCoordinateV5StatusMessage(incomeRequestId, web);
        }

        /// <summary>
        ///  Сохранение подписанного состояния обращения в список IncomeRequestStatusLogList
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="signature">Xml подписи</param>
        [WebMethod]
        public static void SaveIncomeRequestStatusLog(int incomeRequestId, string signature)
        {
            SPWeb web = SPContext.Current.Web;
            IncomeRequestHelper.SaveIncomeRequestStatusLog(incomeRequestId, signature, web);
        }

        /// <summary>
        /// Установка причины отказа по обращению и комментария к отказу
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="refuseReasonCode">Код причины отказа</param>
        /// <param name="refuseComment">Текст комментария к отказу</param>
        [WebMethod]
        public static void SetRefuseReasonAndComment(int incomeRequestId, int refuseReasonCode, string refuseComment, bool needPersonVisit, bool refuseDocuments)
        {
            SPWeb web = SPContext.Current.Web;

            web.AllowUnsafeUpdates = true;
            try
            {
                var list = web.GetListOrBreak("Lists/IncomeRequestList");
                var refuseList = web.GetListOrBreak("Lists/DenyReasonBookList");
                var item = list.GetItemById(incomeRequestId);
                SPListItem refuseItem = null;
                if (refuseReasonCode != 0)
                {
                    refuseItem = refuseList.GetSingleListItemByFieldValue("Tm_ServiceCode", 
                        refuseReasonCode.ToString(CultureInfo.InvariantCulture));
                }

                item["Tm_DenyReasonLookup"] = refuseItem != null ? new SPFieldLookupValue(refuseItem.ID, refuseItem.Title) : null;
                item["Tm_Comment"] = refuseComment;
                item["Tm_RefuseDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date);
                item["Tm_PrepareFactDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date);
                item["Tm_NeedPersonVisit"] = needPersonVisit;
                item["Tm_RefuseDocuments"] = refuseDocuments;

                // Присвоение внутреннего регистрационного номера только если он еще не задан
                var number = item.TryGetValue<string>("Tm_InternalRegNumber");
                if (String.IsNullOrEmpty(number))
                {
                    item["Tm_InternalRegNumber"] = Utility.GetIncomeRequestInternalRegNumber("InternalRegNumber");
                }
                // дату регистрации указываем только если она еще не задана
                var regDate = item.TryGetValueOrNull<DateTime>("Tm_ApplyDate");
                if (regDate == null)
                {
                    item["Tm_ApplyDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date);
                }

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
                var spList    = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                var attachLib = safeWeb.GetListOrBreak("AttachLib");
                var spItem    = spList.GetItemOrBreak(incomeRequestId);
                var ctId      = new SPContentTypeId(spItem["ContentTypeId"].ToString());

                // если у документа уже есть вложения в итоговых документах - используем их и ничего не создаем
                // файлы подписей, которые могут быть, исключаем
                var attachItems = attachLib.GetItems(new SPQuery
                {
                    Query =
                        Camlex.Query()
                            .Where(x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)incomeRequestId.ToString() 
                                     && x["Tm_IncomeRequestAttachLookup"] == null)
                            .ToString(),
                    ViewAttributes = "Scope='Recursive'"
                }).Cast<SPListItem>().Where(x => !x.File.Name.EndsWith(".sig"));

                if (attachItems.Any())
                {
                    var existantItems = attachItems.Select(x =>
                        new DocumentMetaData
                        {
                            DocumentId = x.ID,
                            DocumentUrl = x.File.ServerRelativeUrl
                        });
                    retValList.AddRange(existantItems);
                }
                else
                {
                    var docBuilder = new TemplatedDocumentBuilder(safeWeb, incomeRequestId);

                    if (IsAllTaxiInStatus(incomeRequestId, "Решено положительно"))
                    {
                        retValList.Add(ctId == spList.ContentTypes["Аннулирование"].Id
                            ? docBuilder.RenderDocument(4)
                            : docBuilder.RenderDocument(5));
                    }
                    else if (IsAnyTaxiInStatus(incomeRequestId, "Отказано;Решено отрицательно"))
                    {
                        retValList.Add(ctId == spList.ContentTypes["Аннулирование"].Id
                            ? docBuilder.RenderDocument(4)
                            : docBuilder.RenderDocument(5));
                        retValList.Add(docBuilder.RenderDocument(6));
                    }
                }
            });

            return retValList.ToArray();
        }

        /// <summary>
        /// Получение перечня документов, которые необходимо отправить при оповещении о статусе обращения
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Массив структур DocumentMetaData</returns>
        [WebMethod]
        public static DocumentMetaData[] GetDocumentsForSendStatus(int incomeRequestId)
        {
            SPWeb web = SPContext.Current.Web;
            var retValList = new List<DocumentMetaData>();

            var attachLib = web.GetListOrBreak("AttachLib");
            var attachItems = attachLib.GetItems(new SPQuery
            {
                Query =
                    Camlex.Query()
                        .Where(x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)incomeRequestId.ToString()
                                    && x["Tm_IncomeRequestAttachLookup"] == null)
                        .ToString(),
                ViewAttributes = "Scope='Recursive'"
            }).Cast<SPListItem>();

            if (attachItems.Any())
            {
                var existantItems = attachItems.Select(x =>
                    new DocumentMetaData
                    {
                        DocumentId = x.ID,
                        DocumentUrl = x.File.ServerRelativeUrl
                    });
                retValList.AddRange(existantItems);
            }

            return retValList.ToArray();
        }

        /// <summary>
        /// Генерация документов по обращению при отказе
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Массив структур DocumentMetaData</returns>
        [WebMethod]
        public static dynamic CreateDocumentsWhileRefusing(int incomeRequestId)
        {
            DocumentMetaData[] payLoad;
            SPWeb web = SPContext.Current.Web;
            var retValList = new List<DocumentMetaData>();

            var catchData = 
                Utility.WithCatchExceptionOnWebMethod("Ошибка генерации документов при отказе", () =>
                Utility.WithSafeUpdate(web, (safeWeb) =>
                {
                    // getting data
                    var spList    = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                    var attachLib = safeWeb.GetListOrBreak("AttachLib");
                    var spItem    = spList.GetItemOrBreak(incomeRequestId);
                    var ctId      = new SPContentTypeId(spItem["ContentTypeId"].ToString());

                    // если у документа уже есть вложения в итоговых документах - используем их и ничего не создаем
                    // файлы подписей, которые могут быть, исключаем
                    var attachItems = attachLib.GetItems(new SPQuery
                    {
                        Query =
                            Camlex.Query()
                                .Where(x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)incomeRequestId.ToString()
                                         && x["Tm_IncomeRequestAttachLookup"] == null)
                                .ToString(),
                        ViewAttributes = "Scope='Recursive'"
                    }).Cast<SPListItem>().Where(x => !x.File.Name.EndsWith(".sig"));

                    if (attachItems.Any())
                    {
                        var existantItems = attachItems.Select(x =>
                            new DocumentMetaData
                            {
                                DocumentId = x.ID,
                                DocumentUrl = x.File.ServerRelativeUrl
                            });
                        retValList.AddRange(existantItems);
                    }
                    else
                    {

                        var docBuilder = new TemplatedDocumentBuilder(safeWeb, incomeRequestId);

                        if (docBuilder.RefuseDocuments)
                            retValList.Add(docBuilder.NeedPersonVisit ? docBuilder.RenderDocument(1) : docBuilder.RenderDocument(2));
                        else
                            retValList.Add(docBuilder.RenderDocument(3));
                    }
                }));

            payLoad = retValList.ToArray();

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
                var attachLib      = web.GetListOrBreak("AttachLib");
                var attachItem     = attachLib.GetItemById(documentId);
                var sigFileId      = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
                var sourceFileName = Path.GetFileNameWithoutExtension(attachItem.File.Name);
                var sigFileName    = String.Format("Подпись для {0} {1}.sig", sourceFileName, sigFileId);
                sigFileName        = Utility.MakeFileNameSharePointCompatible(sigFileName);
                var uplFolder      = attachItem.File.ParentFolder;

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
                            IsAnyTaxiInStatus(incomeRequestId, "Отказано;Решено отрицательно") ? "1085" : "1075");

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
                var rList       = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                var rItem       = rList.GetItemOrBreak(incomeRequestId);
                var taxiList    = safeWeb.GetListOrBreak("Lists/TaxiList");
                var licenseList = safeWeb.GetListOrBreak("Lists/LicenseList");
                var taxiIdArr   = taxiIdList.Split(';');

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
                    if (((rStatusCode == "6420") || (rStatusCode == "1050")) && (taxiItem["Tm_TaxiStatus"].ToString() == "Отказано")) continue;

                    switch (rStatusCode)
                    {
                        #region [1040]
                        case "1040":
                            var validator = new TaxiDuplicateValidator(safeWeb, incomeRequestId, taxiItem.ID);
                            if (validator.Execute(null))
                            {
                                taxiItem["Tm_TaxiStatus"] = "В работе";
                                taxiItem.Update();
                            }
                            break;
                        #endregion
                        #region [1050 and 6420]
                        case "6420":
                        case "1050":
                            var ctId = new SPContentTypeId(rItem["ContentTypeId"].ToString());
                            var licenseDraft = new License
                            {
                                Status = (int)LicenseStatus.Draft,
                                TaxiId = taxiItem.ID,
                                MO     = false
                            };
                            #region [Черновик для нового разрешения]
                            if (ctId == rList.ContentTypes["Новое"].Id)
                            {
                                licenseDraft.CreationDate = DateTime.Now.Date;
                                licenseDraft.ChangeDate   = DateTime.Now.Date;
                                licenseDraft.OutputDate   = DateTime.Now.Date;
                                licenseDraft.TillDate     = DateTime.Now.AddYears(5).AddDays(-1).Date;
                                var storedLicenseDraft = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                                {
                                    lob         = BCS.LOBTaxiSystemName,
                                    ns          = BCS.LOBTaxiSystemNamespace,
                                    contentType = "License",
                                    methodName  = "CreateLicense",
                                    methodType  = MethodInstanceType.Creator
                                }, licenseDraft);
                                if (storedLicenseDraft != null)
                                {
                                    var num = String.Format("{0:00000}", Convert.ToInt32(storedLicenseDraft.RegNumber));
                                    taxiItem["Tm_TaxiPrevLicenseNumber"] = num;
                                }
                            }
                            #endregion
                            #region [Черновик для всех остальных]
                            else
                            {
                                var regNumber = taxiItem["Tm_TaxiPrevLicenseNumber"];
                                if (regNumber == null || regNumber.ToString() == String.Empty)
                                    throw new Exception(
                                        "В транспортном средстве не указан номер ранее выданного разрешения");

                                var numInt = Convert.ToInt32(regNumber.ToString());
                                // trying to find existing acting license
                                var expressions = new List<Expression<Func<SPListItem, bool>>>
                                {
                                    // IsLast field - checking if license is acting
                                    x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Number) "1",
                                    // checking for regNumber
                                    x => (string) x["Tm_RegNumber"] == numInt.ToString(CultureInfo.InvariantCulture)
                                };
                                SPListItemCollection licenseItems = licenseList.GetItems(new SPQuery
                                {
                                    Query = Camlex.Query().WhereAll(expressions).ToString(),
                                    ViewAttributes = "Scope='RecursiveAll'"
                                });
                                if (licenseItems.Count == 0)
                                    throw new Exception(String.Format("Не найдено предыдущее действующее разрешение с номером {0}", numInt));
                                if (licenseItems.Count > 1)
                                    throw new Exception(String.Format("Найдено {0} действующих разрешения с номером {1}. Ожидается наличие одного.", licenseItems.Count, numInt));

                                var parentLicense = licenseItems[0];
                                var parentLicenseExt =
                                    LicenseService.GetLicense(Convert.ToInt32(parentLicense["Tm_LicenseExternalId"]));

                                licenseDraft.RegNumber    = parentLicenseExt.RegNumber;
                                licenseDraft.CreationDate = parentLicenseExt.CreationDate;
                                licenseDraft.ChangeDate   = DateTime.Now.Date;
                                licenseDraft.OutputDate   = DateTime.Now.Date;
                                licenseDraft.Parent       = parentLicenseExt.Id;
                                licenseDraft.RootParent   = parentLicenseExt.RootParent;

                                #region [TillDate]
                                if (ctId == rList.ContentTypes["Аннулирование"].Id)
                                {
                                    licenseDraft.TillDate = parentLicenseExt != null ? parentLicenseExt.TillDate : DateTime.Now.Date;
                                }
                                else if (ctId == rList.ContentTypes["Выдача дубликата"].Id ||
                                          ctId == rList.ContentTypes["Переоформление"].Id)
                                {
                                    licenseDraft.TillDate = parentLicenseExt != null ? parentLicenseExt.TillDate : DateTime.Now.AddYears(5).AddDays(-1).Date;
                                }
                                #endregion

                                BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                                {
                                    lob         = BCS.LOBTaxiSystemName,
                                    ns          = BCS.LOBTaxiSystemNamespace,
                                    contentType = "License",
                                    methodName  = "CreateLicense",
                                    methodType  = MethodInstanceType.Creator
                                }, licenseDraft);
                            }
                            #endregion

                            taxiItem["Tm_TaxiStatus"] = "Решено положительно";
                            taxiItem.Update();
                            break;
                        #endregion
                        default:
                            throw new Exception(
                                String.Format(
                                    "Не предусмотрено принятие ТС в данном статусе обращения. Код статуса: {0}",
                                    rStatusCode));
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
                var rList = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                var rItem = rList.GetItemOrBreak(incomeRequestId);
                var refuseList = safeWeb.GetListOrBreak("Lists/DenyReasonBookList");
                SPListItem refuseItem = null;
                if (refuseReasonCode != 0)
                {
                    refuseItem = refuseList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                        refuseReasonCode.ToString(CultureInfo.InvariantCulture));
                }

                SPListItem rStatus;
                Utility.TryGetListItemFromLookupValue(rItem["Tm_IncomeRequestStateLookup"],
                    rList.Fields.GetFieldByInternalName("Tm_IncomeRequestStateLookup") as SPFieldLookup, out rStatus);
                if (rStatus == null)
                    throw new Exception("У обращения должно быть установлено значение статуса");
                var rStatusCode = rStatus["Tm_ServiceCode"] != null
                    ? rStatus["Tm_ServiceCode"].ToString()
                    : String.Empty;

                var taxiIdArr = taxiIdList.Split(';');
                foreach (var taxiItem in taxiIdArr.Select(taxiId => taxiList.GetItemById(Convert.ToInt32(taxiId))))
                {
                    if (((rStatusCode == "6420") || (rStatusCode == "1050")) && (taxiItem["Tm_TaxiStatus"].ToString() == "Отказано")) continue;

                    taxiItem["Tm_DenyReasonLookup"] = refuseItem != null ? new SPFieldLookupValue(refuseItem.ID, refuseItem.Title) : null;
                    taxiItem["Tm_TaxiDenyComment"] = refuseComment;
                    taxiItem["Tm_NeedPersonVisit"] = needPersonVisit;
                    if ((rStatusCode == "6420") || (rStatusCode == "1050"))
                        taxiItem["Tm_TaxiStatus"] = "Решено отрицательно";
                    else taxiItem["Tm_TaxiStatus"] = "Отказано";

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
        /// Все ли транспортные средства обращения, находящиеся в указанном статусе, имеют указанный номер разрешения
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="status">Статус ТС</param>
        /// <returns></returns>
        [WebMethod]
        public static bool IsAllTaxiInStatusHasLicenseNumber(int incomeRequestId, string status)
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

            var licNoCondition = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_TaxiPrevLicenseNumber"] == null
            };
            var licExpr = ExpressionsHelper.CombineOr(licNoCondition);
            var expressions = new List<Expression<Func<SPListItem, bool>>> { choicesExpr, parentExpr, licExpr };

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
                                    outRequestItem["Tm_LicenseLookup"] = new SPFieldLookupValue(lic.ID, lic.Title);
                                    outRequestItem["Tm_LicenseRtParentLicenseLookup"] = licRootParent;
                                    outRequestItem.SystemUpdate();
                                }
                            }
                        }
                    }));
        }

        /// <summary>
        /// Получение кода текущего статуса обращения
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns></returns>
        [WebMethod]
        public static dynamic GetCurrentStatusCode(int incomeRequestId)
        {
            int payLoad = 0;

            var catchData = Utility.WithCatchExceptionOnWebMethod("Получение текущего статуса обращения", () =>
            {
                var web = SPContext.Current.Web;
                var list = web.GetListOrBreak("Lists/IncomeRequestList");
                var item = list.GetItemById(incomeRequestId);

                SPListItem statusItem;
                Utility.TryGetListItemFromLookupValue(item["Tm_IncomeRequestStateLookup"],
                    item.Fields.GetFieldByInternalName("Tm_IncomeRequestStateLookup") as SPFieldLookup, out statusItem);

                if (statusItem != null)
                    payLoad = statusItem["Tm_ServiceCode"] != null ? Convert.ToInt32(statusItem["Tm_ServiceCode"]) : 0;
            });

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
        /// Выдача
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns></returns>
        [WebMethod]
        public static dynamic MakeOutput(int incomeRequestId)
        {
            dynamic innerCatch = null;

            var catchData = Utility.WithCatchExceptionOnWebMethod("Ошибка при выдаче", () =>
                Utility.WithSPServiceContext(SPContext.Current, serviceContextWeb =>
                    Utility.WithSafeUpdate(serviceContextWeb, safeWeb =>
                    {
                        var rList = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                        var rItem = rList.GetItemById(incomeRequestId);
                        var ctId = new SPContentTypeId(rItem["ContentTypeId"].ToString());

                        rItem["Tm_OutputFactDate"] = DateTime.Now.Date;
                        rItem.Update();

                        if (ctId == rList.ContentTypes["Аннулирование"].Id)
                            innerCatch = PromoteLicenseDrafts(incomeRequestId);
                    })));

            object innerError = null;
            object innerData = null;
            var innerDataObj = innerCatch as object;
            if (innerDataObj != null)
            {
                var errorProp = innerDataObj.GetType().GetProperty("Error");
                var dataProp  = innerDataObj.GetType().GetProperty("Data");
                innerData     = dataProp != null ? dataProp.GetValue(innerDataObj, null) : null;
                innerError    = errorProp != null ? errorProp.GetValue(innerDataObj, null) : null;
            }

            if (innerError != null)
            {
                return new
                {
                    Error = innerError,
                    Data = innerData
                };
            }

            var catchDataObj = catchData as object;
            var errorDataProp = catchDataObj.GetType().GetProperty("Error");
            var errorData = errorDataProp != null ? errorDataProp.GetValue(catchDataObj, null) : null;
            return new
            {
                Error = errorData,
                Data = innerData
            };
        }

        [WebMethod]
        public static dynamic SendToAsguf(int incomeRequestId)
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при отправке обращения в АСГУФ", () =>
                    Utility.WithSPServiceContext(SPContext.Current, serviceContextWeb =>
                        Utility.WithSafeUpdate(serviceContextWeb, safeWeb =>
                        {
                            // send request only for local items
                            var rList = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                            var rItem = rList.GetItemById(incomeRequestId);
                            var regNumber = rItem["Tm_RegNumber"];
                            if (regNumber != null && regNumber.ToString() != "") return;

                            // getting V5 message
                            var builder = new IncomeRequestMessageBuilder(safeWeb, incomeRequestId);
                            var internalMessage = builder.SynthesizeV5();
                            // getting config values and queue service client
                            var confItem  = Config.GetConfigItem(safeWeb, "MessageQueueServiceUrl");
                            var binding   = new System.ServiceModel.BasicHttpBinding();
                            var address   = new System.ServiceModel.EndpointAddress(Config.GetConfigValue(confItem).ToString());
                            var svcClient = new MessageQueueService.DataServiceClient(binding, address);
                            confItem      = Config.GetConfigItem(safeWeb, "AsGufServiceGuid");
                            var svcGuid   = Config.GetConfigValue(confItem);
                            var svc       = svcClient.GetService(new Guid(svcGuid.ToString()));
                            //getting queue service message
                            var message =  new MessageQueueService.Message
                            {
                                Service       = svc,
                                MessageId     = new Guid(internalMessage.ServiceHeader.MessageId),
                                MessageType   = 2,
                                MessageMethod = 1,
                                MessageDate   = DateTime.Now,
                                MessageText   = internalMessage.ToXElement<CoordinateMessage>().ToString()
                            };

                            bool sent = svcClient.AddMessage(message);
                            if (!sent) throw new Exception("Сообщение не было отправлено");
                        })));
        }

        /// <summary>
        /// Получение идентификатора документа заявителя, удостоверяющего его личность
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <returns>Идентификатор документа, удостоверяющего личность в списке IncomeRequestAttachList</returns>
        [WebMethod]
        public static int GetDeclarantIdentityId(int incomeRequestId)
        {
            var web = SPContext.Current.Web;

            var docList = web.GetListOrBreak("Lists/IncomeRequestAttachList");
            var expressions = new List<Expression<Func<SPListItem, bool>>>
            { 
                x => (int)x["Tm_AttachType"] >= 20001 && (int)x["Tm_AttachType"] <= 20014,
                x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId) incomeRequestId.ToString()
            };

            SPListItemCollection docs = docList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            var doc = docs.Cast<SPListItem>().FirstOrDefault();

            return doc != null ? doc.ID : 0;
        }

        [WebMethod]
        public static dynamic AssignInternalRegNumber(int incomeRequestId)
        {
            var web = SPContext.Current.Web;

            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при проставлении внутреннего номера обращения", () =>
                    Utility.WithSafeUpdate(web, safeWeb =>
                    {
                        var rList   = safeWeb.GetListOrBreak("Lists/IncomeRequestList");
                        var rItem   = rList.GetItemById(incomeRequestId);
                        var number  = rItem.TryGetValue<string>("Tm_InternalRegNumber");
                        var regDate = rItem.TryGetValueOrNull<DateTime>("Tm_ApplyDate");

                        if (!String.IsNullOrEmpty(number))
                            throw new Exception("Внутренний регистрационный номер обращения уже задан");
                        if (regDate != null)
                            throw new Exception("Дата регистрации обращения уже задана");
                        
                        rItem["Tm_InternalRegNumber"] = Utility.GetIncomeRequestInternalRegNumber("InternalRegNumber");
                        rItem["Tm_ApplyDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date);
                        rItem.Update();
                    }));
        }
    }
}

