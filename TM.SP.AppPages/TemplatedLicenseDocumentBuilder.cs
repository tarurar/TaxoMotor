using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CamlexNET;
using Microsoft.SharePoint;
using Aspose.Words;
using TM.Utils;
using AsposeLicense = Aspose.Words.License;

namespace TM.SP.AppPages
{
    public class TemplatedLicenseDocumentBuilder
    {
        #region [properties]
        private SPWeb _web;
        private SPListItem _taxiItem;
        private SPListItem _requestItem;
        private SPListItem _existingLicense;
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

            var prevLicNumber = _taxiItem["Tm_TaxiPrevLicenseNumber"];
            if (prevLicNumber != null)
            {
                var licenseList = _web.GetListOrBreak("Lists/LicenseList");

                var expressions = new List<Expression<Func<SPListItem, bool>>>
                {
                    // IsLast field - checking if license is acting
                    x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Integer) "1",
                    // previous license number
                    x => (string)x["Tm_RegNumber"] == prevLicNumber.ToString()
                };
                SPListItemCollection licenseItems = licenseList.GetItems(new SPQuery
                {
                    Query = Camlex.Query().WhereAll(expressions).ToString(),
                    ViewAttributes = "Scope='RecursiveAll'"
                });

                if (licenseItems.Count > 0)
                    _existingLicense = licenseItems[0];
            }
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
                "LicenseBeginDate", "TaxiMark", "TaxiModel", "TaxiStateNumber", "LicenseTillDate"
            };


            var currentDate = DateTime.Now;
            var scalarValues = new object[]
            {
                RequestAccountAddress,
                _existingLicense != null ? _existingLicense["Tm_RegNumber"] : null,
                _requestItem["Tm_RequestAccountBCSLookup"],
                _requestItem["Tm_RequestAccountBCSLookup"],
                 currentDate.ToString("dd.MM.yyyy"), // todo
                _taxiItem["Tm_TaxiBrand"],
                _taxiItem["Tm_TaxiModel"],
                _taxiItem["Tm_TaxiStateNumber"],
                _existingLicense != null ? _existingLicense["Tm_LicenseTillDate"] : currentDate.AddYears(5).AddDays(-1).ToString("dd.MM.yyyy")
            };

            var doc = new Document(tmplItem.File.OpenBinaryStream());

            doc.MailMerge.Execute(scalarValueNames, scalarValues);
            var ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Doc);

            return ms;
        }

        #endregion
    }
}
