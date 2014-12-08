using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using TM.SP.AppPages;
using TM.Utils;

namespace TM.SP.Customizations.CONTROLTEMPLATES.TaxoMotor
{
    public partial class LicensePrintControl : UserControl
    {
        private static readonly Dictionary<char, char> AndroidAllowedChars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ._-+,@£$€!½§~'=()[]{}0123456789".ToDictionary(c => c);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request["__EVENTTARGET"] == "PrintTaxiLicenseAction")
            {
                SPWeb web = SPContext.Current.Web;

                var taxiId = Convert.ToInt32(Page.Request["__EVENTARGUMENT"]);
                var taxiItem = web.GetListOrBreak("Lists/TaxiList").GetItemById(taxiId);
                var licenseNum = taxiItem["Tm_TaxiPrevLicenseNumber"];
                var filename = licenseNum == null
                    ? "Разрешение.doc"
                    : String.Format("Разрешение №{0}.doc", licenseNum);

                var docBuilder = new TemplatedLicenseDocumentBuilder(web, taxiId);
                var ms = docBuilder.RenderDocument(7);
                var response = HttpContext.Current.Response;

                response.Clear();
                response.ClearHeaders();
                response.ContentType = "application/ms-word";
                response.AddHeader("Content-Disposition", GetContentDisposition(filename));
                response.AddHeader("Content-Length", ms.Length.ToString(CultureInfo.InvariantCulture));
                ms.WriteTo(response.OutputStream);
                response.End();
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
