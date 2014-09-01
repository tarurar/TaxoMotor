using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace TM.SP.AppPages.ApplicationPages
{
    public enum ValidationErrorSeverity
    {
        Warning = 0,
        Critical
    }

    public class ValidationErrorInfo
    {
        public string Message { get; set; }
        public ValidationErrorSeverity Severity {get; set;}

    }
    public class SendRequestDialogBase : DialogLayoutsPageBase
    {
        protected SPList GetList()
        {
            return this.Web.Lists.GetList(ListIdParam, false);
        }

        protected Guid ListIdParam
        {
            get
            {
                return String.IsNullOrEmpty(base.Request.Params["ListId"]) ? Guid.Empty : new Guid(base.Request.Params["ListId"]);
            }
        }

        protected string ItemIdListParam
        {
            get
            {
                return String.IsNullOrEmpty(base.Request.Params["Items"]) ? String.Empty : base.Request.Params["Items"];
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                EnsureChildControls();
                
                var documentList = LoadDocuments();
                var errorList = ValidateDocuments(documentList);
                BindDocuments(documentList);
                if (errorList.Count > 0)
                    BindErrors(errorList);
                HandleDocumentsLoad(documentList, errorList);
            }
        }

        protected virtual void HandleDocumentsLoad(List<object> documentList, List<ValidationErrorInfo> errorList)
        {
            throw new NotImplementedException();
        }

        protected virtual void BindErrors(List<ValidationErrorInfo> errorList)
        {
            throw new NotImplementedException();
        }

        protected virtual void BindDocuments(List<object> documentList)
        {
            throw new NotImplementedException();
        }

        protected virtual List<ValidationErrorInfo> ValidateDocuments(List<object> documentList)
        {
            throw new NotImplementedException();
        }

        protected virtual List<object> LoadDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
