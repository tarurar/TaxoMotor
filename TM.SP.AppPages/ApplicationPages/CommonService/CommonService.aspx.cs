// <copyright file="CommonService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-12-03 19:54:19Z</date>

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Services;
using System.Xml.Linq;
using CamlexNET;
using Microsoft.SharePoint;
using TM.Utils;
using ODOPM;
using WebServiceMO;

// ReSharper disable CheckNamespace


namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
{
    using System.Security.Permissions;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;

    /// <summary>
    /// TODO: Add comment for CommonService
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class CommonService : LayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the CommonService class
        /// </summary>
        public CommonService()
        {
            RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Обновление сроков предоставления государственных услуг
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static dynamic RefreshGovServiceTerms()
        {
            var web = SPContext.Current.Web;

            return
                Utility.WithCatchExceptionOnWebMethod("Обновление сроков оказания госуслуг", () =>
                    Utility.WithSafeUpdate(web, safeWeb =>
                    {
                        var list = safeWeb.GetListOrBreak("Lists/GovServiceSubTypeBookList");
                        SPListItemCollection items = list.GetItems(new SPQuery());

                        foreach (SPListItem spItem in items)
                        {
                            var serviceCode = spItem["Tm_ServiceCode"] != null
                                ? spItem["Tm_ServiceCode"].ToString()
                                : null;
                            if (serviceCode == null) continue;

                            switch (serviceCode)
                            {
                                case "020202":
                                    spItem["Tm_TermOfService"] = 10;
                                    break;
                                case "020203":
                                    spItem["Tm_TermOfService"] = 5;
                                    break;
                                case "020204":
                                    spItem["Tm_TermOfService"] = 1;
                                    break;
                                case "77200101":
                                    spItem["Tm_TermOfService"] = 10;
                                    break;
                                default:
                                    spItem["Tm_TermOfService"] = 0;
                                    break;
                            }
                            spItem.Update();
                        }
                    }));
        }

        [WebMethod]
        public static dynamic SendOdopm()
        {
            return Utility.WithCatchExceptionOnWebMethod("Отправка данных в ОДОПМ", () =>
                Utility.WithSPServiceContext(SPContext.Current, web =>
                {
                    var ssOdopmAppId =
                        Config.GetConfigValue(Config.GetConfigItem(web, "OdopmSingleSignOnAppId")).ToString();
                    var ssLocalDbAccessAppId =
                        Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBWriterAccessSingleSignOnAppId"))
                            .ToString();

                    var odopmUserId     = Security.GetSecureStoreUserNameCredential(ssOdopmAppId);
                    var odopmPassword   = Security.GetSecureStorePasswordCredential(ssOdopmAppId);
                    var odopmUrl        = Config.GetConfigValue(Config.GetConfigItem(web, "OdopmUrl")).ToString();
                    var odopmClientCert = Config.GetConfigValue(Config.GetConfigItem(web, "OdopmClientCertificate")).ToString();
                    var storeUserId     = Security.GetSecureStoreUserNameCredential(ssLocalDbAccessAppId);
                    var storePassword   = Security.GetSecureStorePasswordCredential(ssLocalDbAccessAppId);
                    var storeHost       = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBHost")).ToString();
                    var storeDbName     = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBName")).ToString();

                    var cBuilder = new SqlConnectionStringBuilder
                    {
                        DataSource     = storeHost,
                        InitialCatalog = storeDbName,
                        UserID         = storeUserId,
                        Password       = storePassword
                    };

                    var sender = new ODOPM_Class();
                    var parameters = new ODOPM_Class.Parametrs
                    {
                        UserNameServiceString     = odopmUserId,
                        UserPasswordServiceString = odopmPassword,
                        EndPointUrlString         = odopmUrl,
                        ConnectionString          = cBuilder.ConnectionString,
                        ClientSertificate         = odopmClientCert,
                        ServerSertificate         = odopmClientCert
                    };

                    sender.Process(parameters);
                }));
        }

        [WebMethod]
        public static dynamic SendMo()
        {
            return Utility.WithCatchExceptionOnWebMethod("Отправка данных в Московскую область", () =>
                Utility.WithSPServiceContext(SPContext.Current, web =>
                {
                    var ssLocalDbAccessAppId =
                        Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBWriterAccessSingleSignOnAppId"))
                            .ToString();

                    var storeUserId   = Security.GetSecureStoreUserNameCredential(ssLocalDbAccessAppId);
                    var storePassword = Security.GetSecureStorePasswordCredential(ssLocalDbAccessAppId);
                    var storeHost     = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBHost")).ToString();
                    var storeDbName   = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBName")).ToString();
                    var moUrl         = Config.GetConfigValue(Config.GetConfigItem(web, "MoServiceUrl")).ToString();

                    var cBuilder = new SqlConnectionStringBuilder
                    {
                        DataSource     = storeHost,
                        InitialCatalog = storeDbName,
                        UserID         = storeUserId,
                        Password       = storePassword
                    };

                    var sender = new ServiceMO();
                    sender.Process(cBuilder.ConnectionString, moUrl);
                }));
        }

        [WebMethod]
        public static dynamic GetPtsRequestDetails(int taxiId, int licenseId)
        {
            string payLoad = String.Empty;

            var catchData = Utility.WithCatchExceptionOnWebMethod("Получение деталей по запросу ПТС", () =>
            {
                if ((taxiId == 0) && licenseId == 0)
                    throw new Exception("Необходимо указать идентификатор транспортного средства или разрешения");

                var web = SPContext.Current.Web;
                var outRequestList = web.GetListOrBreak("Lists/OutcomeRequestStateList");
                var orTypeList     = web.GetListOrBreak("Lists/OutcomeRequestTypeBookList");
                var orPtsItem      = orTypeList.GetSingleListItemByFieldValue("Tm_ServiceCode", "1");
                var taxiIdStr      = taxiId.ToString(CultureInfo.InvariantCulture);
                var licenseIdStr   = licenseId.ToString(CultureInfo.InvariantCulture);
                var orTypeIdStr    = orPtsItem.ID.ToString(CultureInfo.InvariantCulture);
                const string formatMessagePattern = "{{ \"message\": \"{0}\" }}";

                var expressions = new List<Expression<Func<SPListItem, bool>>>
                {
                    x => x["Tm_OutputRequestTypeLookup"] == (DataTypes.LookupId) orTypeIdStr,
                    x => (bool) x["Tm_AnswerReceived"],
                };
                if (taxiId != 0)
                {
                    expressions.Add(x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) taxiIdStr);
                }
                else if (licenseId != 0)
                {
                    expressions.Add(x => x["Tm_LicenseLookup"] == (DataTypes.LookupId)licenseIdStr);
                }
                SPListItemCollection outRequestItems = outRequestList.GetItems(new SPQuery
                {
                    Query =
                        Camlex.Query()
                            .WhereAll(expressions)
                            .OrderBy(x => x["Tm_LastProcessDate"] as Camlex.Desc)
                            .ToString(),
                    ViewAttributes = "Scope='RecursiveAll'"
                });

                if (outRequestItems.Count > 0)
                {
                    var outRequestItem   = outRequestItems[0];
                    var oResultCode      = outRequestItem["Tm_ResultCode"];
                    var oXmlValue        = outRequestItem["Tm_XmlValue"];
                    var resultCode       = oResultCode != null ? oResultCode.ToString() : null;
                    var customAttributes = oXmlValue != null ? oXmlValue.ToString() : null;

                    if (!String.IsNullOrEmpty(resultCode))
                    {
                        switch (resultCode)
                        {
                            case "1":
                                if (!String.IsNullOrEmpty(customAttributes))
                                {
                                    XElement xdoc = XElement.Parse(customAttributes);
                                    var dict = xdoc.Descendants().ToDictionary(c => c.Name.LocalName, c => c.Value);
                                    payLoad = PtsDictionaryToJson(dict);
                                }
                                else
                                    payLoad = String.Format(formatMessagePattern, "Ответ получен: Данных ПТС нет");
                                break;
                            case "3":
                                payLoad = String.Format(formatMessagePattern, "Ответ получен: Данные не найдены");
                                break;
                            default:
                                payLoad = String.Format(formatMessagePattern, "Ответ получен: Неизвестный код результата");
                                break;
                        }
                    }
                    else payLoad = String.Format(formatMessagePattern, "Ответ получен: Пустой код результата");
                }
                else payLoad = String.Format(formatMessagePattern, "Ответ еще не получен либо запрос не был отправлен");
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

        private static string PtsDictionaryToJson(Dictionary<string, string> dict)
        {
            const string formatItemPattern =
                "{{ \"name\": \"{0}\", \"translation\": \"{1}\", \"value\": \"{2}\", \"order\": \"{3}\" }}";

            var entries =
                dict.Where(d => GetPtsDetailItemOrder(d.Key) != 0)
                    .Select(
                        d =>
                            string.Format(formatItemPattern, d.Key, GetPtsDetailItemTranslation(d.Key),
                                FormatPtsItemValue(d.Value), GetPtsDetailItemOrder(d.Key)));

            return "[" + string.Join(",", entries) + "]";
        }

        private static string FormatPtsItemValue(string value)
        {
            DateTime date;

            if (DateTime.TryParse(value, out date))
            {
                return date.ToString("dd.MM.yyyy");
            }
            
            return value;
        }

        private static int GetPtsDetailItemOrder(string itemName)
        {
            switch (itemName)
            {
                case "LastName":
                    return 1;
                case "FirstName":
                    return 2;
                case "MiddleName":
                    return 3;
                case "Sex":
                    return 4;
                case "Birthday":
                    return 5;
                case "citizenship":
                    return 6;
                case "personType":
                    return 7;
                case "Organization":
                    return 8;
                case "INN":
                    return 9;
                case "KPP":
                    return 10;
                case "OGRN":
                    return 11;
                case "postalCode":
                    return 12;
                case "Region":
                    return 13;
                case "Area":
                    return 14;
                case "Place":
                    return 15;
                case "Street":
                    return 16;
                case "House":
                    return 17;
                case "Building":
                    return 18;
                case "Apartment":
                    return 19;
                case "regNo":
                    return 20;
                case "BrandAndModel":
                    return 21;
                case "VehicleType":
                    return 22;
                case "VehicleCategory":
                    return 23;
                case "vin":
                    return 24;
                case "color":
                    return 27;
                case "registrationDocument":
                    return 28;
                case "registrationDate":
                    return 29;
                case "maxAllowedWeight":
                    return 25;
                case "weightWithoutLoad":
                    return 26;
                default:
                    return 0;
            }
        }

        private static string GetPtsDetailItemTranslation(string itemName)
        {
            switch (itemName)
            {
                case "LastName":
                    return "Фамилия";
                case "FirstName":
                    return "Имя";
                case "MiddleName":
                    return "Отчество";
                case "Sex":
                    return "Пол";
                case "Birthday":
                    return "Дата рождения";
                case "citizenship":
                    return "Гражданство";
                case "personType":
                    return "Тип субъекта";
                case "Organization":
                    return "Наименование организации";
                case "INN":
                    return "ИНН";
                case "KPP":
                    return "КПП";
                case "OGRN":
                    return "ОГРН";
                case "postalCode":
                    return "Индекс";
                case "Region":
                    return "Наименование субъекта РФ";
                case "Area":
                    return "Наименование района";
                case "Place":
                    return "Наименование населенного пункта";
                case "Street":
                    return "Наименование улицы";
                case "House":
                    return "Дом";
                case "Building":
                    return "Корпус";
                case "Apartment":
                    return "Квартира";
                case "regNo":
                    return "Государственный регистрационный знак";
                case "BrandAndModel":
                    return "Марка, модель (модификация)";
                case "VehicleType":
                    return "Наименование типа ТС";
                case "VehicleCategory":
                    return "Категория ТС";
                case "vin":
                    return "VIN-номер";
                case "color":
                    return "Цвет";
                case "registrationDocument":
                    return "Номер свидетельства о регистрации ТС";
                case "registrationDate":
                    return "Дата свидетельства о регистрации ТС";
                case "maxAllowedWeight":
                    return "Разрешенная максимальная масса";
                case "weightWithoutLoad":
                    return "Масса без нагрузки";
                default:
                    return "Нет перевода для элемента";
            }
        }
    }
}

