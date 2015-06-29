using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using Aspose.Words;
using TM.Utils;
using TM.SP.BCSModels.Taxi;
using TM.SP.BCSModels.CoordinateV5;
using AsposeLicense = Aspose.Words.License;
using License = TM.SP.BCSModels.Taxi.License;
using RequestAccount = TM.SP.BCSModels.CoordinateV5.RequestAccount;
using System.Collections.Generic;

namespace TM.SP.AppPages.Print
{
    public class TemplatedLicenseDocumentBuilder: TemplatedBaseLicenseDocumentBuilder
    {
        #region [properties]
        private SPListItem _taxiItem;
        private License _existingLicense;
        #endregion

        #region [methods]
        public TemplatedLicenseDocumentBuilder(SPWeb web, int requestId, int taxiId): base(web, requestId)
        {
            _taxiItem        = _web.GetListOrBreak("Lists/TaxiList").GetItemById(taxiId);
            _existingLicense = LicenseHelper.GetAnyLicenseForSPTaxiId(taxiId);
        }
        public override MemoryStream RenderDocument(int templateNumber, out string fileExtension)
        {
            var tmplItem = ValidateTemplate(templateNumber);

            Dictionary<string, string> dict = GetLicenseValues(_taxiItem, _existingLicense);
            var scalarValueNames = dict.Keys.ToArray<string>();
            var scalarValues = dict.Values.ToArray<string>();

            var doc = new Document(tmplItem.File.OpenBinaryStream());
            doc.MailMerge.Execute(scalarValueNames, scalarValues);
            var ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Doc);
            fileExtension = ".doc";

            return ms;
        }
        #endregion
    }
}
