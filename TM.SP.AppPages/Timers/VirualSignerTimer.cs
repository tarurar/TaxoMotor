using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;
using TM.Utils.TimerJobs;
using TM.SP.BCSModels.Taxi;
using Microsoft.BusinessData.MetadataModel;
using TM.SP.AppPages.VirtualSigner;

namespace TM.SP.AppPages.Timers
{
    public class VirualSignerTimer : JobDefinition
    {
        #region [methods]
        public VirualSignerTimer()
        {}

        public VirualSignerTimer(string jobName, string jobTitle, SPService service): base(jobName, jobTitle, service) { }

        public VirualSignerTimer(string jobName, string jobTitle, SPWebApplication webapp): base(jobName, jobTitle, webapp) { }

        protected override void Work(SPWeb web)
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
