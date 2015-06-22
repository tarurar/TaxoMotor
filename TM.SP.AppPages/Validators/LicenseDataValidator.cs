using Microsoft.SharePoint;
using Microsoft.XmlDiffPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TM.SP.AppPages.Validators
{
    /// <summary>
    /// Сравнение данных разрешения в подписанном xml и в SQL
    /// </summary>
    public class LicenseDataValidator: Validator
    {
        #region [fields]
        private int licenseId;
        #endregion
        public LicenseDataValidator(SPWeb web, int licenseId) : base(web) 
        {
            this.licenseId = licenseId;
        }

        public override bool Execute(params object[] paramsList)
        {
            bool valid = false;
            var currentXml = LicenseHelper.GetLicenseXml(licenseId, _web);
            var signedXml = LicenseHelper.GetLicenseSavedXml(licenseId, _web);

            if (!String.IsNullOrEmpty(currentXml) && !String.IsNullOrEmpty(signedXml))
            {
                var diff = new XmlDiff(XmlDiffOptions.IgnoreChildOrder
                    | XmlDiffOptions.IgnoreComments
                    | XmlDiffOptions.IgnoreNamespaces
                    | XmlDiffOptions.IgnoreWhitespace
                    | XmlDiffOptions.IgnoreXmlDecl
                    | XmlDiffOptions.IgnoreDtd
                    | XmlDiffOptions.IgnorePI
                    | XmlDiffOptions.IgnorePrefixes);

                var currentXmlDoc = new XmlDocument();
                currentXmlDoc.LoadXml(currentXml);
                var signedXmlDoc = new XmlDocument();
                signedXmlDoc.LoadXml(signedXml);

                valid = diff.Compare(signedXmlDoc.DocumentElement, currentXmlDoc.DocumentElement);
            }

            return valid;
        }
    }
}
