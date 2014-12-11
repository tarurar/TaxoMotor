using System;
using System.Globalization;
using System.IO;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using Aspose.Words;
using TM.Utils;
using TM.SP.BCSModels.Taxi;
using AsposeLicense = Aspose.Words.License;
using License = TM.SP.BCSModels.Taxi.License;

namespace TM.SP.AppPages
{
    public class TemplatedLicenseDocumentBuilder
    {
        #region [properties]
        private SPWeb _web;
        private SPListItem _taxiItem;
        private SPListItem _requestItem;
        private License _existingLicense;
        private SPList _tmplLib;
        private AsposeLicense _asposeLic;

        public string RequestAccountAddress
        {
            get
            {
                var value = _requestItem["Tm_IncomeRequestDeclarantAddress"];
                if (value != null)
                {
                    var fieldValue = new SPFieldMultiChoiceValue(value.ToString());
                    return fieldValue[0];
                } else return null;
            }
        }

        #endregion

        #region [methods]

        public TemplatedLicenseDocumentBuilder(SPWeb web, int taxiId)
        {
            _web        = web;
            _taxiItem   = _web.GetListOrBreak("Lists/TaxiList").GetItemById(taxiId);
            _tmplLib    = _web.GetListOrBreak("DocumentTemplateLib");
            _asposeLic  = new AsposeLicense();
            _asposeLic.SetLicense("Aspose.Total.lic");

            Utility.TryGetListItemFromLookupValue(_taxiItem["Tm_IncomeRequestLookup"],
                _taxiItem.Fields.GetFieldByInternalName("Tm_IncomeRequestLookup") as SPFieldLookup, out _requestItem);

            _existingLicense = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "GetAnyLicenseForSPTaxiId",
                methodType  = MethodInstanceType.SpecificFinder
            }, taxiId);
        }

        public MemoryStream RenderDocument(int templateNumber)
        {
            var tmplItem = _tmplLib.GetSingleListItemByFieldValue("Tm_ServiceCode",
                templateNumber.ToString(CultureInfo.InvariantCulture));
            if (tmplItem == null)
                throw new Exception(String.Format("Template with number {0} doesn't exist", templateNumber));

            var scalarValueNames = new string[]
            {
                "RequestAccountAddress", "LicenseNumber", "RequestAccountFullName", "RequestAccountShortName",
                "LicenseCreationDate", "TaxiMark", "TaxiModel", "TaxiStateNumber", "LicenseOutputDate", 
                "LicenseChangeDate", "Signer", "SignerPosition"
            };


            var signerName     = Config.GetConfigValue(Config.GetConfigItem(_web, "SignerName")).ToString();
            var signerPosition = Config.GetConfigValue(Config.GetConfigItem(_web, "SignerJob")).ToString();
            var scalarValues = new object[]
            {
                RequestAccountAddress,
                String.Format("{0:00000}", Convert.ToInt32(_existingLicense.RegNumber)),
                _requestItem["Tm_RequestAccountBCSLookup"],
                _requestItem["Tm_RequestAccountBCSLookup"],
                _existingLicense.CreationDate.HasValue ? _existingLicense.CreationDate.Value.ToString("dd.MM.yyyy") : "",
                _taxiItem["Tm_TaxiBrand"],
                _taxiItem["Tm_TaxiModel"],
                _taxiItem["Tm_TaxiStateNumber"],
                _existingLicense.OutputDate.HasValue ? _existingLicense.OutputDate.Value.ToString("dd.MM.yyyy") : "",
                _existingLicense.ChangeDate.HasValue ? _existingLicense.ChangeDate.Value.ToString("dd.MM.yyyy") : "",
                signerName,
                signerPosition
            };

            var doc = new Document(tmplItem.File.OpenBinaryStream());
            doc.MailMerge.Execute(scalarValueNames, scalarValues);
            var ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);

            return ms;
        }

        #endregion
    }
}
