using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

using TM.Utils;
using MessageQueueService = TM.ServiceClients.MessageQueue;

namespace TM.SP.AppPages.ApplicationPages
{
    /// <summary>
    /// Abstraction for item to send request for
    /// Descedants have to inherite
    /// </summary>
    [Serializable]
    public class RequestItem
    {
        public int Id {get; set;}
        public string Title { get; set; }
        public bool HasError {get; set;}
    }
    
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
        /// <summary>
        /// Getting current list instance which triggered the send event
        /// </summary>
        /// <returns></returns>
        protected SPList GetList()
        {
            return this.Web.Lists.GetList(ListIdParam, false);
        }
        /// <summary>
        /// ListId parameter from query string
        /// </summary>
        protected Guid ListIdParam
        {
            get
            {
                return String.IsNullOrEmpty(Request.Params["ListId"]) ? Guid.Empty : new Guid(Request.Params["ListId"]);
            }
        }
        /// <summary>
        /// Items parameter from query string
        /// </summary>
        protected string ItemIdListParam
        {
            get
            {
                return String.IsNullOrEmpty(Request.Params["Items"]) ? String.Empty : Request.Params["Items"];
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                EnsureChildControls();

                var documentList = LoadDocuments<RequestItem>();
                var errorList = ValidateDocuments<RequestItem>(documentList);
                BindDocuments<RequestItem>(documentList);
                if (errorList.Count > 0)
                    BindErrors(errorList);
                HandleDocumentsLoad<RequestItem>(documentList, errorList);
            }
        }
        /// <summary>
        /// Post processing operations for documents list to be sent 
        /// Fires after the dialog loads for the first time
        /// Descedants have to implement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentList"></param>
        /// <param name="errorList"></param>
        protected virtual void HandleDocumentsLoad<T>(List<T> documentList, List<ValidationErrorInfo> errorList) where T : RequestItem
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Binding errors list to the UI
        /// Descedants have to implement
        /// </summary>
        /// <param name="errorList"></param>
        protected virtual void BindErrors(List<ValidationErrorInfo> errorList)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Binding documents list to the UI
        /// Descedants have to implement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentList"></param>
        protected virtual void BindDocuments<T>(List<T> documentList) where T : RequestItem
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Validating documents list
        /// Descedants have to implement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentList"></param>
        /// <returns>List of errors</returns>
        protected virtual List<ValidationErrorInfo> ValidateDocuments<T>(List<T> documentList) where T : RequestItem
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Loading documents list
        /// Descedants have to implement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual List<T> LoadDocuments<T>() where T : RequestItem
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Getting queue service client instance
        /// </summary>
        /// <returns></returns>
        protected virtual MessageQueueService.DataServiceClient GetServiceClientInstance()
        {
            SPListItem confItem = Config.GetConfigItem(this.Web, "MessageQueueServiceUrl");
            var binding = new System.ServiceModel.BasicHttpBinding();
            var address = new System.ServiceModel.EndpointAddress(Config.GetConfigValue(confItem).ToString());

            return new MessageQueueService.DataServiceClient(binding, address);
        }
        /// <summary>
        /// Building message for queue
        /// Descedants have to implement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        protected virtual MessageQueueService.Message BuildMessage<T>(T document) where T : RequestItem
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Tracking outcome requests
        /// Adds new item to the tracking list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="success"></param>
        /// <param name="requestId">Message Id value</param>
        /// <returns></returns>
        protected virtual SPListItem TrackOutcomeRequest<T>(T document, bool success, Guid requestId) where T : RequestItem
        {
            if (!success) return null;

            var trackList = this.Web.GetListOrBreak("Lists/OutcomeRequestStateList");
            var newItem = trackList.AddItem();
            newItem["Title"]                        = requestId.ToString("B");
            newItem["Tm_OutputDate"]                = DateTime.Now;
            newItem["Tm_IncomeRequestLookup"]       = new SPFieldLookupValue(document.Id, document.Title);
            newItem["Tm_OutputRequestTypeLookup"]   = null;   // todo
            newItem["Tm_AnswerReceived"]            = false;
            newItem["Tm_MessageId"]                 = requestId;
            newItem.Update();

            return newItem;
        }
        /// <summary>
        /// Sending all requests
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentList">Documents requests have to be send for</param>
        /// <returns></returns>
        protected bool SendRequests<T>(List<T> documentList) where T : RequestItem
        {
            bool success = true;
            var svcClient = GetServiceClientInstance();

            foreach (T document in documentList)
            {
                if (document.HasError) continue;

                var newMessage = BuildMessage<T>(document);
                bool sent = svcClient.AddMessage(newMessage);
                TrackOutcomeRequest<T>(document, sent, newMessage.MessageId);
                success |= sent;
            }

            return success;
        }
    }
}
