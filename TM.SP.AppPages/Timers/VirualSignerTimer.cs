using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;
using TM.SP.BCSModels.Taxi;
using Microsoft.BusinessData.MetadataModel;
using TM.SP.AppPages.VirtualSigner;

namespace TM.SP.AppPages.Timers
{
    public class VirualSignerTimer : SPJobDefinition
    {
        #region [resource strings]

        public static readonly string TaxiListsFeatureId = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        #endregion

        #region [methods]
        public VirualSignerTimer()
        {}

        public VirualSignerTimer(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            Title = "ТаксоМотор: Виртуальный подписант разрешений";
        }

        public VirualSignerTimer(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            Title = "ТаксоМотор: Виртуальный подписант разрешений";
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                var webApp = Parent as SPWebApplication;
                if (webApp != null)
                {
                    foreach (SPSite siteCollection in webApp.Sites)
                    {
                        try
                        {
                            SPWeb web = siteCollection.RootWeb;

                            if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                                web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                            {
                                Work(web);
                            }
                        }
                        finally
                        {
                            siteCollection.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
	        {
                throw new Exception(String.Format("Детали: {0}", ex.Message));
	        }
        }

        private void Work(SPWeb web)
        {
            var configParamName = "OdopmClientCertificate";
            var certThumbprint = Config.GetConfigValueOrDefault<string>(web, configParamName);
            if (String.IsNullOrEmpty(certThumbprint))
                throw new Exception(String.Format("В конфигурации не указан отпечаток сертификата для подписания. Параметр конфигурации: {0}", configParamName));

            Utility.WithSPServiceContext(web, serviceContextWeb => 
            {
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
            });
        }

        #endregion
    }
}
