using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using Aspose.Words;
using TM.Utils;
using TM.SP.BCSModels.Taxi;
using TM.SP.BCSModels.CoordinateV5;
using AsposeLicense = Aspose.Words.License;
using License = TM.SP.BCSModels.Taxi.License;
using RequestAccount = TM.SP.BCSModels.CoordinateV5.RequestAccount;
using System.IO;
using System.Data;
using CamlexNET;

namespace TM.SP.AppPages.Print
{
    public class TemplatedMultiLicenseDocumentBuilder: TemplatedBaseLicenseDocumentBuilder
    {
        #region [fields]
        private IEnumerable<SPListItem> _taxiItems;
        #endregion

        #region [methods]
        public TemplatedMultiLicenseDocumentBuilder(SPWeb web, int requestId, int[] taxiItems) : base(web, requestId)
        {
            var taxiList = this._web.GetListOrBreak("Lists/TaxiList");

            var taxiQuery = new SPQuery
            {
                Query = Camlex.Query().Where(x => taxiItems.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='Recursive'"
            };
            this._taxiItems = taxiList.GetItems(taxiQuery).Cast<SPListItem>();
        }
        private DataTable CreateDataTable()
        {
            var dt = new DataTable("License");

            dt.Columns.Add("RequestAccountAddress",   typeof(string));
            dt.Columns.Add("LicenseNumber",           typeof(string));
            dt.Columns.Add("DeclarantNamePE",         typeof(string));
            dt.Columns.Add("DeclarantNameJP",         typeof(string));
            dt.Columns.Add("RequestAccountFullName",  typeof(string));
            dt.Columns.Add("RequestAccountShortName", typeof(string));
            dt.Columns.Add("LicenseCreationDate",     typeof(string));
            dt.Columns.Add("TaxiMark",                typeof(string));
            dt.Columns.Add("TaxiModel",               typeof(string));
            dt.Columns.Add("TaxiStateNumber",         typeof(string));
            dt.Columns.Add("LicenseOutputDate",       typeof(string));
            dt.Columns.Add("LicenseChangeDate",       typeof(string));
            dt.Columns.Add("Signer",                  typeof(string));
            dt.Columns.Add("SignerPosition",          typeof(string));
            dt.Columns.Add("RequestApplyDate",        typeof(string));
            dt.Columns.Add("LicenseTillDate",         typeof(string));
            dt.Columns.Add("PE_Caption",              typeof(string));
            dt.Columns.Add("PE_Passport",             typeof(string));
            dt.Columns.Add("PE_Address",              typeof(string));
            dt.Columns.Add("DeclarantBrandName",      typeof(string));

            return dt;
        }
        private void FillDataTable(DataTable dt)
        {
            foreach (SPListItem taxiItem in _taxiItems)
            {
                var existingLicense = LicenseHelper.GetAnyLicenseForSPTaxiId(taxiItem.ID);

                var dr = dt.NewRow();

                Dictionary<string, string> values = GetLicenseValues(taxiItem, existingLicense);
                foreach (KeyValuePair<string, string> pair in values)
                {
                    var cName = pair.Key;
                    var cValue = pair.Value;

                    if (dt.Columns.Contains(cName))
                    {
                        dr[cName] = cValue;
                    }
                }

                dt.Rows.Add(dr);
            }
        }
        public override MemoryStream RenderDocument(int templateNumber, out string fileExtension)
        {
            var tmplItem = ValidateTemplate(templateNumber);

            var doc = new Document(tmplItem.File.OpenBinaryStream());
            var dt = CreateDataTable();
            FillDataTable(dt);
            doc.MailMerge.ExecuteWithRegions(dt);

            var ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Doc);
            fileExtension = ".doc";

            return ms;
        }
        #endregion
    }
}
