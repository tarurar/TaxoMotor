using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.ApplicationPages
{
    public enum ValidationErrorSeverity
    {
        Warning = 0,
        Critical
    }

    public struct ValidationErrorInfo
    {
        public string Message;
        public ValidationErrorSeverity Severity;

    }
    public class SendRequestDialogBase : DialogLayoutsPageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                var documentList = LoadDocuments();
                BindDocuments(documentList);
                var errorList = ValidateDocuments(documentList);
                if (errorList.Length > 0)
                    BindErrors(errorList);
                HandleDocumentsLoad(documentList, errorList);
            }
        }

        protected void HandleDocumentsLoad(IEnumerable<dynamic> documentList, ValidationErrorInfo[] errorList)
        {
            throw new NotImplementedException();
        }

        protected void BindErrors(ValidationErrorInfo[] errorList)
        {
            throw new NotImplementedException();
        }

        protected virtual void BindDocuments(IEnumerable<dynamic> documentList)
        {
            throw new NotImplementedException();
        }

        protected virtual ValidationErrorInfo[] ValidateDocuments(IEnumerable<dynamic> documentList)
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerable<dynamic> LoadDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
