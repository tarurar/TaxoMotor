using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SharePoint;
using TM.SP.AppPages.Communication;
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
        public OutcomeRequestType RequestTypeCode { get; set; }
        public string ListName { get; set; }
    }

    public enum OutcomeRequestType
    {
        Pts = 1,
        Egrul,
        Egrip,
        Penalty
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
        protected ServiceClients.MessageQueue.IDataService QueueClient
        {
            get { return ServiceLocator.Instance.GetService<ServiceClients.MessageQueue.IDataService>(); } 
        }

        protected IQueueMessageBuilder QueueMessageBuilder
        {
            get { return ServiceLocator.Instance.GetService<IQueueMessageBuilder>(); }
        }

        /// <summary>
        /// Getting current list instance which triggered the send event
        /// </summary>
        /// <returns></returns>
        protected SPList GetList()
        {
            return Web.Lists.GetList(ListIdParam, false);
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
                var errorList = ValidateDocuments(documentList);
                BindDocuments(documentList);
                if (errorList.Count > 0)
                    BindErrors(errorList);
                HandleDocumentsLoad(documentList, errorList);
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
            
            var trackList = Web.GetListOrBreak("Lists/OutcomeRequestStateList");
            var requestTypeList = Web.GetListOrBreak("Lists/OutcomeRequestTypeBookList");
            var requestTypeItem = requestTypeList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                ((int) document.RequestTypeCode).ToString(CultureInfo.InvariantCulture));

            var pFolder = CreateOutcomeRequestFolder(trackList);
            var newItem = trackList.AddItem(pFolder.ServerRelativeUrl, SPFileSystemObjectType.File);
            newItem["Title"]                        = requestTypeItem != null ? requestTypeItem.Title : "Запрос";
            newItem["Tm_OutputDate"]                = DateTime.Now;
            newItem["Tm_IncomeRequestLookup"]       = new SPFieldLookupValue(document.Id, document.Title);
            newItem["Tm_OutputRequestTypeLookup"]   = new SPFieldLookupValue(requestTypeItem.ID, requestTypeItem.Title);
            newItem["Tm_AnswerReceived"]            = false;
            newItem["Tm_MessageId"]                 = requestId;
            newItem.Update();

            return newItem;
        }

        protected virtual SPFolder CreateOutcomeRequestFolder(SPList list)
        {
            var yearStr  = DateTime.Now.ToString("yyyy");
            var monthstr = DateTime.Now.ToString("MMMM");
            var dayStr   = DateTime.Now.ToString("dd");
            var hourStr  = DateTime.Now.ToString("hh");

            return list.RootFolder.CreateSubFolders(new[] {yearStr, monthstr, dayStr, hourStr});
        }

        /// <summary>
        /// Sending all requests
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentList">Documents requests have to be send for</param>
        /// <param name="needsTracking">Whether request have to be tracked</param>
        /// <returns></returns>
        protected bool SendRequests<T>(List<T> documentList, bool needsTracking = true) where T : RequestItem
        {
            var success = true;

            foreach (var document in documentList)
            {
                if (document.HasError) continue;

                var newMessage = BuildMessage(document);
                var sent = QueueClient.AddMessage(newMessage);
                if (needsTracking)
                {
                    TrackOutcomeRequest(document, sent, newMessage.RequestId);
                }
                success |= sent;
            }

            return success;
        }
    }
}
