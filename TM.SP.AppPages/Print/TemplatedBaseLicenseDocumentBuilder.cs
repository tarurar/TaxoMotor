using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using Aspose.Words;
using TM.Utils;
using AsposeLicense = Aspose.Words.License;
using License = TM.SP.BCSModels.Taxi.License;
using RequestAccount = TM.SP.BCSModels.CoordinateV5.RequestAccount;
using System.IO;

namespace TM.SP.AppPages.Print
{
    public abstract class TemplatedBaseLicenseDocumentBuilder
    {
        #region [fields]
        protected SPWeb _web;
        protected SPListItem _requestItem;
        protected RequestAccount _declarant;
        protected SPListItem _declarantIdentity;
        protected SPList _tmplLib;
        protected AsposeLicense _asposeLic;
        protected string _signerName;
        protected string _signerPosition;

        protected static readonly string _dateFormat = @"«dd» MMMM yyyy г.";
        #endregion

        #region [properties]
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
                var docTypeName = docTypeNameItem != null ? docTypeNameItem.Title : String.Empty;

                var textPattern = "{0} серия {1} №{2}, выдан {3} {4}";
                return String.Format(textPattern,
                    docTypeName, docSeries, docNum, docWho, docWhen.ToString(_dateFormat));
            }
        }
        #endregion

        #region [methods]
        private TemplatedBaseLicenseDocumentBuilder() { }
        protected TemplatedBaseLicenseDocumentBuilder(SPWeb web, int requestId)
        {
            this._web = web;
            this._tmplLib = _web.GetListOrBreak("DocumentTemplateLib");
            this._asposeLic = new AsposeLicense();
            this._asposeLic.SetLicense("Aspose.Total.lic");
            this._requestItem = _web.GetListOrBreak("Lists/IncomeRequestList").GetItemOrBreak(requestId);

            var declarantId = BCS.GetBCSFieldLookupId(_requestItem, "Tm_RequestAccountBCSLookup");
            this._declarant = SendRequestEGRULPage.GetRequestAccount((int)declarantId);

            var docList = _web.GetListOrBreak("Lists/IncomeRequestAttachList");
            var identityDocId = IncomeRequestService.GetDeclarantIdentityId(_requestItem.ID);
            this._declarantIdentity = identityDocId != 0 ? docList.GetItemOrBreak(identityDocId) : null;

            this._signerName = Config.GetConfigValueOrDefault<string>(_web, "SignerName");
            this._signerPosition = Config.GetConfigValueOrDefault<string>(_web, "SignerJob");
        }
        protected Dictionary<string, string> GetLicenseValues(SPListItem taxiItem, License exLicense)
        {
            var values = new Dictionary<string, string>();

            bool isPrvtEntrprnr = _declarant.OrgFormCode.Equals(SendRequestEGRULPage.PrivateEntrepreneurCode);
            var applyDate = _requestItem.TryGetValueOrNull<DateTime>("Tm_ApplyDate");
            var applyDateText = applyDate.HasValue ? applyDate.Value.ToString(_dateFormat)
                : "Дата принятия в работу обращения не указана";

            values.Add("RequestAccountAddress", isPrvtEntrprnr ? String.Empty : _declarant.SingleStrPostalAddress);
            values.Add("LicenseNumber", String.Format("{0:00000}", Convert.ToInt32(exLicense.RegNumber)));
            values.Add("RequestAccountFullName", _declarant.FullName);
            values.Add("DeclarantNamePE", isPrvtEntrprnr ? _declarant.Name : String.Empty);
            values.Add("DeclarantNameJP", isPrvtEntrprnr ? String.Empty : String.Format("({0})", _declarant.Name));
            values.Add("DeclarantBrandName", String.Format("({0})", _declarant.BrandName));
            values.Add("LicenseCreationDate", exLicense.CreationDate.HasValue ? exLicense.CreationDate.Value.ToString(_dateFormat) : "");
            values.Add("TaxiMark", taxiItem.TryGetValue<string>("Tm_TaxiBrand"));
            values.Add("TaxiModel", taxiItem.TryGetValue<string>("Tm_TaxiModel"));
            values.Add("TaxiStateNumber", taxiItem.TryGetValue<string>("Tm_TaxiStateNumber"));
            values.Add("LicenseOutputDate", exLicense.OutputDate.HasValue ? exLicense.OutputDate.Value.ToString(_dateFormat) : "");
            values.Add("LicenseChangeDate", exLicense.ChangeDate.HasValue ? exLicense.ChangeDate.Value.ToString(_dateFormat) : "");
            values.Add("Signer", _signerName);
            values.Add("SignerPosition", _signerPosition);
            values.Add("RequestApplyDate", applyDateText);
            values.Add("LicenseTillDate", exLicense.TillDate.HasValue ? exLicense.TillDate.Value.ToString(_dateFormat) : "");
            values.Add("PE_Caption", isPrvtEntrprnr ? "Индивидуальный предприниматель" : String.Empty);
            values.Add("PE_Passport", isPrvtEntrprnr ? DeclarantIdentityText : String.Empty);
            values.Add("PE_Address", isPrvtEntrprnr ? _declarant.SingleStrPostalAddress : String.Empty);

            return values;
        }
        protected SPListItem ValidateTemplate(int tmplNumber)
        {
            var tmplItem = _tmplLib.GetSingleListItemByFieldValue("Tm_ServiceCode", tmplNumber.ToString());
            if (tmplItem == null)
                throw new Exception(String.Format("Template with number {0} doesn't exist", tmplNumber));

            return tmplItem;
        }
        public virtual MemoryStream RenderDocument(int templateNumber, out string fileExtension)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
