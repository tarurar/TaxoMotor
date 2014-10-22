// <copyright file="LicenseService.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-16 19:53:14Z</date>
namespace TM.SP.AppPages
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.Services;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Administration;

    using TM.Utils;
    using TM.SP.BCSModels.Taxi;
    using Microsoft.BusinessData.MetadataModel;

    /// <summary>
    /// TODO: Add comment for LicenseService
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class LicenseService : LayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the LicenseService class
        /// </summary>
        public LicenseService()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        private static void SaveSigned(int licenseId, Action<License> contextAction)
        {
            SPWeb web         = SPContext.Current.Web;
            SPList spList     = web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);

            License license = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo()
            {
                contentType = "License",
                lob         = BCS.LOBTaxiSystemName,
                methodName  = "ReadLicenseItem",
                methodType  = MethodInstanceType.SpecificFinder,
                ns          = BCS.LOBTaxiSystemNamespace
            }, Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

            var newLicense = license.Clone();
            contextAction(newLicense);

            BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo() 
            {
                contentType = "License",
                lob         = BCS.LOBTaxiSystemName,
                methodName  = "CreateLicense",
                methodType  = MethodInstanceType.Creator,
                ns          = BCS.LOBTaxiSystemNamespace
            }, newLicense);
        }

        private static string GetLicenseXml(int licenseId, Action<License> contextAction)
        {
            SPWeb web         = SPContext.Current.Web;
            SPList spList     = web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);

            License license = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo()
            {
                contentType = "License",
                lob         = BCS.LOBTaxiSystemName,
                methodName  = "ReadLicenseItem",
                methodType  = MethodInstanceType.SpecificFinder,
                ns          = BCS.LOBTaxiSystemNamespace
            }, Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

            var newLicense = license.Clone();
            contextAction(newLicense);

            //serialization
            StringWriter intWriter = new StringWriter(new StringBuilder());
            XmlWriter writer = new XmlTextWriter(intWriter);
            XmlSerializer serializer = new XmlSerializer(typeof(License));
            writer.WriteStartElement("Data");
            serializer.Serialize(writer, newLicense);
            writer.WriteEndElement();
            
            return intWriter.ToString();
        }

        [WebMethod]
        public static string SuspensionGetXml(int licenseId, DateTime dateFrom, DateTime dateTo, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate       = dateFrom;
                l.TillSuspensionDate = dateTo.IsJavascriptNullDate() ? (DateTime?)null : dateTo;
                l.SuspensionReason   = reason;
            });
        }

        [WebMethod]
        public static string CancellationGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate       = dateFrom != null ? dateFrom : DateTime.Now;
                l.CancellationReason = reason;
            });
        }

        [WebMethod]
        public static string RenewalGetXml(int licenseId, DateTime dateFrom, string reason)
        {
            return GetLicenseXml(licenseId, l =>
            {
                l.CreationDate = dateFrom != null ? dateFrom : DateTime.Now;
                l.ChangeReason = reason;
            });
        }

        [WebMethod]
        public static void SaveSignedSuspension(int licenseId, DateTime dateFrom, DateTime dateTo, string reason, string signature)
        {
            SaveSigned(licenseId, l =>
            {
                l.CreationDate       = dateFrom;
                l.TillSuspensionDate = dateTo.IsJavascriptNullDate() ? (DateTime?)null : dateTo;
                l.SuspensionReason   = reason;
                l.Signature          = System.Uri.UnescapeDataString(signature);
                l.Status             = 2;
            });
        }
    }
}

