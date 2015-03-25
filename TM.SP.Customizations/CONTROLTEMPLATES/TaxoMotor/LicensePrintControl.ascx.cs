using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using TM.SP.AppPages;
using TM.SP.AppPages.Print;
using TM.Utils;
using System.IO;

namespace TM.SP.Customizations.CONTROLTEMPLATES.TaxoMotor
{
    public partial class LicensePrintControl : UserControl
    {
        private static readonly Dictionary<char, char> AndroidAllowedChars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ._-+,@£$€!½§~'=()[]{}0123456789".ToDictionary(c => c);


        private MemoryStream PrintLicense(out string fileName)
        {
            SPWeb web = SPContext.Current.Web;

            var taxiId = Convert.ToInt32(Page.Request["__EVENTARGUMENT"]);
            var taxiItem = web.GetListOrBreak("Lists/TaxiList").GetItemOrBreak(taxiId);
            if (taxiItem["Tm_IncomeRequestLookup"] == null)
                throw new Exception("У транспортного средства не указана ссылка на обращение");
            var requestId = new SPFieldLookupValue(taxiItem["Tm_IncomeRequestLookup"].ToString()).LookupId;
            var licenseNum = taxiItem["Tm_TaxiPrevLicenseNumber"];

            var docBuilder = new TemplatedLicenseDocumentBuilder(web, requestId, taxiId);
            string fileExtension;
            var ms = docBuilder.RenderDocument(7, out fileExtension);
            fileName = licenseNum == null
                ? "Разрешение" + fileExtension
                : String.Format("Разрешение №{0}" + fileExtension, licenseNum);
            fileName = Utility.MakeFileNameSharePointCompatible(fileName);

            return ms;
        }

        private MemoryStream PrintLicenseMultiple(out string fileName)
        {
            SPWeb web = SPContext.Current.Web;

            var taxiItems = Page.Request["__EVENTARGUMENT"];
            if (String.IsNullOrEmpty(taxiItems))
                throw new ArgumentException("Необходимо указать список транспортных средств");

            var taxiArr = taxiItems.Split(';').Select(x => Convert.ToInt32(x)).ToArray();
            var taxiItem0 = web.GetListOrBreak("Lists/TaxiList").GetItemOrBreak(taxiArr[0]);
            if (taxiItem0["Tm_IncomeRequestLookup"] == null)
                throw new Exception("У транспортного средства не указана ссылка на обращение");
            var requestId = new SPFieldLookupValue(taxiItem0["Tm_IncomeRequestLookup"].ToString()).LookupId;
            var requestItem = web.GetListOrBreak("Lists/IncomeRequestList").GetItemOrBreak(requestId);

            var docBuilder = new TemplatedMultiLicenseDocumentBuilder(web, requestId, taxiArr);
            string fileExtension;
            var ms = docBuilder.RenderDocument(8, out fileExtension);
            fileName = String.Format("Разрешения по {0}" + fileExtension, requestItem.Title);
            fileName = Utility.MakeFileNameSharePointCompatible(fileName);

            return ms;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var @event = Page.Request["__EVENTTARGET"];
            bool isPrintEvent = (@event != null) && (@event.Equals("PrintTaxiLicenseAction") || @event.Equals("PrintTaxiLicenseMultipleAction"));

            if (isPrintEvent)
            {
                string fileName = "";
                MemoryStream content = null;

                if (@event.Equals("PrintTaxiLicenseAction"))
                {
                    content = PrintLicense(out fileName);
                } else if (@event.Equals("PrintTaxiLicenseMultipleAction"))
                {
                    content = PrintLicenseMultiple(out fileName);
                }
                var fileExt = Path.GetExtension(fileName);

                if (content != null)
                {
                    var response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearHeaders();
                    response.ContentType = MimeTypeMap.GetMimeType(fileExt);
                    response.AddHeader("Content-Disposition", GetContentDisposition(fileName));
                    response.AddHeader("Content-Length", content.Length.ToString(CultureInfo.InvariantCulture));
                    content.WriteTo(response.OutputStream);
                    response.End();
                }
            }
        }

        //http://stackoverflow.com/questions/93551/how-to-encode-the-filename-parameter-of-content-disposition-header-in-http
        private string GetContentDisposition(string filename)
        {
            var request = Page.Request;
            string contentDisposition;
            if (request.Browser.Browser == "IE" && (request.Browser.Version == "7.0" || request.Browser.Version == "8.0"))
                contentDisposition = "attachment; filename=" + Uri.EscapeDataString(filename);
            else if (request.UserAgent != null && request.UserAgent.ToLowerInvariant().Contains("android")) // android built-in download manager (all browsers on android)
                contentDisposition = "attachment; filename=\"" + MakeAndroidSafeFileName(filename) + "\"";
            else
                contentDisposition = "attachment; filename=\"" + filename + "\"; filename*=UTF-8''" + Uri.EscapeDataString(filename);
            return contentDisposition;
        }

        private string MakeAndroidSafeFileName(string fileName)
        {
            char[] newFileName = fileName.ToCharArray();
            for (int i = 0; i < newFileName.Length; i++)
            {
                if (!AndroidAllowedChars.ContainsKey(newFileName[i]))
                    newFileName[i] = '_';
            }
            return new string(newFileName);
        }
    }
}
