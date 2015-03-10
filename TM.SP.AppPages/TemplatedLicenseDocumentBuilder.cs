using System;
using System.Globalization;
using System.IO;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using Aspose.Words;
using TM.Utils;
using TM.SP.BCSModels.Taxi;
using TM.SP.BCSModels.CoordinateV5;
using AsposeLicense = Aspose.Words.License;
using License = TM.SP.BCSModels.Taxi.License;
using RequestAccount = TM.SP.BCSModels.CoordinateV5.RequestAccount;

namespace TM.SP.AppPages
{
    public class TemplatedLicenseDocumentBuilder
    {
        #region [properties]
        private SPWeb _web;
        private SPListItem _taxiItem;
        private SPListItem _requestItem;
        private License _existingLicense;
        private RequestAccount _declarant;
        private SPListItem _declarantIdentity;
        private SPList _tmplLib;
        private AsposeLicense _asposeLic;

        private static readonly string dateFormat = "dd MMMM yyyy";

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

        public string DeclarantIdentityText 
        {
            get
            {
                if (_declarantIdentity == null) return String.Empty;

                var docTypeCode = _declarantIdentity.TryGetValue<int>("Tm_AttachType");
                var docSeries   = _declarantIdentity.TryGetValue<string>("Tm_AttachDocSerie");
                var docNum      = _declarantIdentity.TryGetValue<string>("Tm_AttachDocNumber");
                var docWho      = _declarantIdentity.TryGetValue<string>("Tm_AttachWhoSigned");
                var docWhen     = _declarantIdentity.TryGetValue<DateTime>("Tm_AttachDocDate");

                var docTypeBookList = _web.GetListOrBreak("Lists/IdentityDocumentTypeBookList");
                var docTypeNameItem = docTypeBookList.GetSingleListItemByFieldValue("Tm_ServiceCode", docTypeCode.ToString());
                var docTypeName     = docTypeNameItem != null ? docTypeNameItem.Title : String.Empty;

                var textPattern = "{0} серия {1} №{2}, выдан {3} {4}";
                return String.Format(textPattern, 
                    docTypeName, docSeries, docNum, docWho, docWhen.ToString(dateFormat));
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

            #region [getting income request]
            Utility.TryGetListItemFromLookupValue(_taxiItem["Tm_IncomeRequestLookup"],
                _taxiItem.Fields.GetFieldByInternalName("Tm_IncomeRequestLookup") as SPFieldLookup, out _requestItem);
            #endregion
            #region [getting declarant]
            var declarantId = BCS.GetBCSFieldLookupId(_requestItem, "Tm_RequestAccountBCSLookup");
            _declarant = SendRequestEGRULPage.GetRequestAccount((int)declarantId);
            #endregion
            #region [getting declarant identity]
            var docList = _web.GetListOrBreak("Lists/IncomeRequestAttachList");
            var identityDocId = IncomeRequestService.GetDeclarantIdentityId(_requestItem.ID);
            _declarantIdentity = identityDocId != 0 ? docList.GetItemById(identityDocId) : null;
            #endregion
            #region [getting existing license]
            _existingLicense = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "GetAnyLicenseForSPTaxiId",
                methodType  = MethodInstanceType.SpecificFinder
            }, taxiId);
            #endregion
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
                "LicenseChangeDate", "Signer", "SignerPosition", "RequestApplyDate", "LicenseTillDate",
                "PE_Caption", "PE_Passport", "PE_Address"
            };



            var signerName = Config.GetConfigValueOrDefault<string>(_web, "SignerName");
            var signerPosition = Config.GetConfigValueOrDefault<string>(_web, "SignerJob");
            var requestApplyDateText = _requestItem["Tm_ApplyDate"] != null
                    ? DateTime.Parse(_requestItem["Tm_ApplyDate"].ToString()).ToString(dateFormat)
                    : "Дата принятия в работу обращения не указана";
            var scalarValues = new object[]
            {
                _declarant.OrgFormCode.Equals("91") ? String.Empty : _declarant.SingleStrPostalAddress,
                String.Format("{0:00000}", Convert.ToInt32(_existingLicense.RegNumber)),
                _declarant.FullName,
                _declarant.Name,
                _existingLicense.CreationDate.HasValue ? _existingLicense.CreationDate.Value.ToString(dateFormat) : "",
                _taxiItem["Tm_TaxiBrand"],
                _taxiItem["Tm_TaxiModel"],
                _taxiItem["Tm_TaxiStateNumber"],
                _existingLicense.OutputDate.HasValue ? _existingLicense.OutputDate.Value.ToString(dateFormat) : "",
                _existingLicense.ChangeDate.HasValue ? _existingLicense.ChangeDate.Value.ToString(dateFormat) : "",
                signerName,
                signerPosition,
                requestApplyDateText,
                _existingLicense.TillDate.HasValue ? _existingLicense.TillDate.Value.ToString(dateFormat) : "",
                _declarant.OrgFormCode.Equals("91") ? "Индивидуальный предприниматель" : String.Empty,
                _declarant.OrgFormCode.Equals("91") ? DeclarantIdentityText : String.Empty,
                _declarant.OrgFormCode.Equals("91") ? _declarant.SingleStrPostalAddress : String.Empty,
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
