// <copyright file="CommonService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-12-03 19:54:19Z</date>

using System.Data.SqlClient;
using System.Web.Services;
using Microsoft.SharePoint;
using TM.Utils;
using ODOPM;

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
    }
}

