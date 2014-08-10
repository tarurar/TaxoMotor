using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;

namespace TM.SP.AppPages.ApplicationPages
{
    public abstract class DialogLayoutsPageBase : LayoutsPageBase
    {
        protected static readonly string resFilePathRelative = @"TaxoMotor\TM.SP.AppPages";
        /// <summary>
        /// URL of the page to redirect to when not in Dialog mode.
        /// </summary>
        protected string GetLocalizedString(string Key)
        {
            return SPUtility.GetLocalizedString(Key, resFilePathRelative, this.Web != null ? this.Web.Language : 1033);
        }

        /// 
        /// <summary>
        /// URL of the page to redirect to when not in Dialog mode.
        /// </summary>
        protected string PageToRedirectOnOK { get; set; }
        /// <summary>
        /// Returns true if the Application Page is displayed in Modal Dialog.
        /// </summary>
        protected bool IsPopUI
        {
            get
            {
                return !String.IsNullOrEmpty(base.Request.QueryString["IsDlg"]);
            }
        }
        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// Returns the OK response.
        /// </summary>
        protected void EndOperation()
        {
            EndOperation(1);
        }
        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// </summary>
        /// <param name="result">Result code to pass to the output. Available results: -1 = invalid; 0 = cancel; 1 = OK</param>
        protected void EndOperation(int result)
        {
            EndOperation(result, PageToRedirectOnOK);
        }
        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// </summary>
        /// <param name="result">Result code to pass to the output. Available results: -1 = invalid; 0 = cancel; 1 = OK</param>
        /// <param name="returnValue">Value to pass to the callback method defined when opening the Modal Dialog.</param>
        protected void EndOperation(int result, string returnValue)
        {
            if (IsPopUI)
            {
                Page.Response.Clear();
                Page.Response.Write(String.Format(CultureInfo.InvariantCulture, "<script type=\"text/javascript\">window.frameElement.commonModalDialogClose({0}, {1});</script>", new object[] { result, String.IsNullOrEmpty(returnValue) ? "null" : String.Format("\"{0}\"", returnValue) }));
                Page.Response.End();
            }
            else
            {
                RedirectOnOK();
            }
        }
        /// <summary>
        /// Redirects to the URL specified in the PageToRedirectOnOK property.
        /// </summary>
        private void RedirectOnOK()
        {
            SPUtility.Redirect(PageToRedirectOnOK ?? SPContext.Current.Web.Url, SPRedirectFlags.UseSource, Context);
        }
    }
}
