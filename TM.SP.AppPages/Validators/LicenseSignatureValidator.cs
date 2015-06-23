using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TM.Utils;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using TM.SP.BCSModels.Taxi;

namespace TM.SP.AppPages.Validators
{
    /// <summary>
    /// Проверка ЭЦП
    /// </summary>
    public class LicenseSignatureValidator: Validator
    {
        #region [fields]
        private License curLicense;
        private X509Certificate2 signerCertificate;
        #endregion

        public LicenseSignatureValidator(SPWeb web, int licenseId)
            : base(web) 
        {
            SPList spList = _web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);
            this.curLicense = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));
        }

        private byte[] GetRawCertificateFromSignature(XmlElement signature)
        {
            XmlNodeList certList = signature.GetElementsByTagName("X509Certificate");

            if (certList.Count > 0)
            {
                return Convert.FromBase64String(certList[0].InnerText);
            }

            return null;
        }

        private XmlDocument  GetLicenseAsXmlDocument()
        {
            var signature = curLicense.Signature;
            if (String.IsNullOrEmpty(signature))
            {
                throw new Exception(String.Format("Разрешение {0} не содержит подписанных данных", curLicense.RegNumber));
            }

            XmlDocument xmlDoc = new XmlDocument { PreserveWhitespace = true };
            xmlDoc.LoadXml(signature);
            return xmlDoc;
        }

        public override bool Execute(params object[] paramsList)
        {
            bool valid = false;

            XmlDocument xmlLicense = GetLicenseAsXmlDocument();
            XmlNodeList signatureList = xmlLicense.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl);
            for (int curSignatureIdx = 0; curSignatureIdx < signatureList.Count; curSignatureIdx++)
            {
                // getting information about certificate
                XmlElement curSignature = (XmlElement)signatureList[curSignatureIdx];
                signerCertificate = new X509Certificate2(GetRawCertificateFromSignature(curSignature));
                // checking signature
                SignedXml signedXml = new SignedXml(xmlLicense);
                signedXml.LoadXml(curSignature);
                valid = signedXml.CheckSignature();
                if (!valid) break;
            }
            
            return valid;
        }

        public override object GetResult()
        {
            return signerCertificate;
        }
    }
}
