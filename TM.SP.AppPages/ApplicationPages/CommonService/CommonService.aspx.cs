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
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using TM.SP.BCSModels.CoordinateV5;
using TM.Utils;
using TM.SP.AppPages.VirtualSigner;
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
                Utility.WithSPServiceContext(SPContext.Current, web => DoSendOdopm(web)));
        }

        public static void SendOdopm(SPWeb web)
        {
            var ctx = SPContext.GetContext(web);

            Utility.WithSPServiceContext(ctx, serviceWeb => DoSendOdopm(serviceWeb));
        }

        private static void DoSendOdopm(SPWeb web)
        {
            var ssOdopmAppId =
                        Config.GetConfigValue(Config.GetConfigItem(web, "OdopmSingleSignOnAppId")).ToString();
            var ssLocalDbAccessAppId =
                Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBWriterAccessSingleSignOnAppId"))
                    .ToString();

            var odopmUserId = Security.GetSecureStoreUserNameCredential(ssOdopmAppId);
            var odopmPassword = Security.GetSecureStorePasswordCredential(ssOdopmAppId);
            var odopmUrl = Config.GetConfigValue(Config.GetConfigItem(web, "OdopmUrl")).ToString();
            var odopmClientCert = Config.GetConfigValue(Config.GetConfigItem(web, "OdopmClientCertificate")).ToString();
            var storeUserId = Security.GetSecureStoreUserNameCredential(ssLocalDbAccessAppId);
            var storePassword = Security.GetSecureStorePasswordCredential(ssLocalDbAccessAppId);
            var storeHost = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBHost")).ToString();
            var storeDbName = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBName")).ToString();

            var cBuilder = new SqlConnectionStringBuilder
            {
                DataSource = storeHost,
                InitialCatalog = storeDbName,
                UserID = storeUserId,
                Password = storePassword
            };

            var sender = new ODOPM_Class();
            var parameters = new ODOPM_Class.Parametrs
            {
                UserNameServiceString = odopmUserId,
                UserPasswordServiceString = odopmPassword,
                EndPointUrlString = odopmUrl,
                ConnectionString = cBuilder.ConnectionString,
                ClientSertificate = odopmClientCert,
                ServerSertificate = odopmClientCert
            };

            sender.Process(parameters);
        }

        [WebMethod]
        public static dynamic SendMo()
        {
            return Utility.WithCatchExceptionOnWebMethod("Отправка данных в Московскую область", () =>
                Utility.WithSPServiceContext(SPContext.Current, web => {
                    DoSendMo(web);
                }));
        }

        public static void SendMo(SPWeb web)
        {
            var ctx = SPContext.GetContext(web);

            Utility.WithSPServiceContext(ctx, serviceWeb => DoSendMo(serviceWeb));
        }

        private static void DoSendMo(SPWeb web)
        {
            var ssLocalDbAccessAppId = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBWriterAccessSingleSignOnAppId")).ToString();

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
        }

        [WebMethod]
        public static dynamic UpdateSQLViews()
        {
            return Utility.WithCatchExceptionOnWebMethod("Обновление представлений SQL", () =>
                Utility.WithSPServiceContext(SPContext.Current, web =>
                {
                    UpdateSQLViews(web);
                }));
        }

        public static void UpdateSQLViews(SPWeb web)
        {
            var ctx = SPContext.GetContext(web);

            Utility.WithSPServiceContext(ctx, serviceWeb => DoUpdateSQLViews(serviceWeb));
        }

        private static void DoUpdateSQLViews(SPWeb web)
        {
            var ssLocalDbAccessAppId = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBWriterAccessSingleSignOnAppId")).ToString();

            var storeUserId   = Security.GetSecureStoreUserNameCredential(ssLocalDbAccessAppId);
            var storePassword = Security.GetSecureStorePasswordCredential(ssLocalDbAccessAppId);
            var storeHost     = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBHost")).ToString();
            var storeDbName   = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBName")).ToString();

            var updater = new ViewUpdater.ViewUpdater(storeHost, storeDbName, storeUserId, storePassword);
            updater.UpdateViews();
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

                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                if (taxiId != 0)
                {
                    expressions.Add(x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) taxiIdStr);
                }
                else if (licenseId != 0)
                {
                    expressions.Add(x => x["Tm_LicenseLookup"] == (DataTypes.LookupId)licenseIdStr);
                }
                expressions.Add(x => x["Tm_OutputRequestTypeLookup"] == (DataTypes.LookupId) orTypeIdStr);
                expressions.Add(x => (bool) x["Tm_AnswerReceived"]);

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

        [WebMethod]
        public static dynamic GetLatestRequestAccountId()
        {
            int payLoad = 0;

            var catchData =
                Utility.WithCatchExceptionOnWebMethod("Ошибка при получении идентификатора последнего добавленного юридического лица", () =>
                    Utility.WithSPServiceContext(SPContext.Current, serviceContextWeb =>
                        Utility.WithSafeUpdate(serviceContextWeb, safeWeb =>
                        {
                            var item = BCS.ExecuteBcsMethod<RequestAccount>(new BcsMethodExecutionInfo
                            {
                                lob         = BCS.LOBRequestSystemName,
                                ns          = BCS.LOBRequestSystemNamespace,
                                contentType = "RequestAccount",
                                methodName  = "GetLatest",
                                methodType  = MethodInstanceType.Scalar
                            }, null);

                            if (item != null)
                                payLoad = item.Id;
                        })));

            var catchDataObj = catchData as object;
            var errorDataProp = catchDataObj.GetType().GetProperty("Error");
            var errorData = errorDataProp != null ? errorDataProp.GetValue(catchDataObj, null) : null;

            return new
            {
                Error = errorData,
                Data = payLoad
            };
        }

        [WebMethod]
        public static dynamic GetLatestRequestContactId()
        {
            int payLoad = 0;

            var catchData =
                Utility.WithCatchExceptionOnWebMethod("Ошибка при получении идентификатора последнего добавленного физического лица", () =>
                    Utility.WithSPServiceContext(SPContext.Current, serviceContextWeb =>
                        Utility.WithSafeUpdate(serviceContextWeb, safeWeb =>
                        {
                            var item = BCS.ExecuteBcsMethod<RequestContact>(new BcsMethodExecutionInfo
                            {
                                lob         = BCS.LOBRequestSystemName,
                                ns          = BCS.LOBRequestSystemNamespace,
                                contentType = "RequestContact",
                                methodName  = "GetLatest",
                                methodType  = MethodInstanceType.Scalar
                            }, null);

                            if (item != null)
                                payLoad = item.Id_Auto;
                        })));

            var catchDataObj = catchData as object;
            var errorDataProp = catchDataObj.GetType().GetProperty("Error");
            var errorData = errorDataProp != null ? errorDataProp.GetValue(catchDataObj, null) : null;

            return new
            {
                Error = errorData,
                Data = payLoad
            };
        }

        private static WebClientGIBDD.WebServiceClient GetGibddClientInstance(SPWeb web)
        {
            var serviceUrl1 = Config.GetConfigValue(Config.GetConfigItem(web, "GibddSpecTransportServiceUrl")).ToString();
            var serviceUrl2 = Config.GetConfigValue(Config.GetConfigItem(web, "GibddTakeOffServiceUrl")).ToString();

            var ssLocalDbAccessAppId = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBWriterAccessSingleSignOnAppId")).ToString();
            var storeUserId          = Security.GetSecureStoreUserNameCredential(ssLocalDbAccessAppId);
            var storePassword        = Security.GetSecureStorePasswordCredential(ssLocalDbAccessAppId);
            var storeHost            = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBHost")).ToString();
            var storeDbName          = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBName")).ToString();

            var cBuilder = new SqlConnectionStringBuilder
            {
                DataSource               = storeHost,
                InitialCatalog           = storeDbName,
                UserID                   = storeUserId,
                Password                 = storePassword,
                MultipleActiveResultSets = true
            };

            return new WebClientGIBDD.WebServiceClient("Реестр СТ ГИБДД", serviceUrl1, serviceUrl2, cBuilder.ConnectionString);
        }

        [WebMethod]
        public static dynamic GibddPutDataPackage()
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при отправке данных в реестр спецтранспорта ГИБДД", () =>
                    Utility.WithSPServiceContext(SPContext.Current, (serviceContextWeb) =>
                        Utility.WithSafeUpdate(serviceContextWeb, (safeWeb) =>
                        {
                            DoGibddPutDataPAckage(safeWeb);
                        })));
        }

        private static void DoGibddPutDataPAckage(SPWeb web)
        {
            var client = GetGibddClientInstance(web);
            client.putDataPackages();
        }

        public static void GibddPutDataPackage(SPWeb web)
        {
            var ctx = SPContext.GetContext(web);

            Utility.WithSPServiceContext(ctx, serviceWeb => DoGibddPutDataPAckage(serviceWeb));
        }

        [WebMethod]
        public static dynamic GibddGetCancelledLicenses()
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при получении сведений о снятых с учета ТС в ГИБДД", () =>
                    Utility.WithSPServiceContext(SPContext.Current, (serviceContextWeb) =>
                        Utility.WithSafeUpdate(serviceContextWeb, (safeWeb) =>
                        {
                            DoGibddGetCancelledLicenses(safeWeb);
                        })));
        }

        private static void DoGibddGetCancelledLicenses(SPWeb web)
        {
            var client = GetGibddClientInstance(web);
            client.getCancelledLicenses();
        }

        public static void GibddGetCancelledLicenses(SPWeb web)
        {
            var ctx = SPContext.GetContext(web);

            Utility.WithSPServiceContext(ctx, serviceWeb => DoGibddGetCancelledLicenses(serviceWeb));
        }

        [WebMethod]
        public static dynamic GibddGetDataPackagesInfo()
        {
            return
                Utility.WithCatchExceptionOnWebMethod("Ошибка при получении сведений о стадии обработки отправленных данных", () =>
                    Utility.WithSPServiceContext(SPContext.Current, (serviceContextWeb) =>
                        Utility.WithSafeUpdate(serviceContextWeb, (safeWeb) =>
                        {
                            DoGibddGetDataPackagesInfo(safeWeb);
                        })));
        }

        private static void DoGibddGetDataPackagesInfo(SPWeb web)
        {
            var client = GetGibddClientInstance(web);
            client.getDataPackagesInfo();
        }

        public static void GibddGetDataPackagesInfo(SPWeb web)
        {
            var ctx = SPContext.GetContext(web);

            Utility.WithSPServiceContext(ctx, serviceWeb => DoGibddGetDataPackagesInfo(serviceWeb));
        }


        [WebMethod]
        public static dynamic RunVirtualSigner()
        {
            return Utility.WithCatchExceptionOnWebMethod("Подписание разрешений", () =>
                Utility.WithSPServiceContext(SPContext.Current, web =>
                {
                    DoRunVirtualSigner(web);
                }));
        }

        public static void RunVirtualSigner(SPWeb web)
        {
            var ctx = SPContext.GetContext(web);

            Utility.WithSPServiceContext(ctx, serviceWeb => DoRunVirtualSigner(serviceWeb));
        }

        private static void DoRunVirtualSigner(SPWeb web)
        {
            var configParamName = "OdopmClientCertificate";
            var certThumbprint = Config.GetConfigValueOrDefault<string>(web, configParamName);
            if (String.IsNullOrEmpty(certThumbprint))
                throw new Exception(String.Format("В конфигурации не указан отпечаток сертификата для подписания. Параметр конфигурации: {0}", configParamName));

            bool hasUnsigned = true;
            do
            {
                try
                {
                    var license       = LicenseHelper.GetUnsignedLicense();
                    var xmltoSign     = Utility.PrepareXmlDataForSign(LicenseHelper.Serialize(license));
                    var signer        = new X509Signer(CertificateHelper.GetCryptoProCertificate(certThumbprint));
                    license.Signature = signer.SignXml(xmltoSign);

                    LicenseHelper.UpdateLicense(license);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("UnsignedNotFoundException")) throw;
                    hasUnsigned = false;
                }

            } while (hasUnsigned);
        }

    }
}

