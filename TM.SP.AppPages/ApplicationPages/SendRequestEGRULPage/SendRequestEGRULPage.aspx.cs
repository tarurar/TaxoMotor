// <copyright file="SendRequestEGRULPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-08-06 17:56:53Z</date>
// ReSharper disable CheckNamespace
namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.BusinessData.MetadataModel;
    using CamlexNET;

    using ApplicationPages;
    using BcsCoordinateV5Model = BCSModels.CoordinateV5;
    using Utils;
    using Services.CoordinateV5;
    using MessageQueueService = ServiceClients.MessageQueue;


    [Serializable]
    public class EGRULRequestItem : RequestItem
    {
        public string RequestAccount { get; set; }
        public int RequestAccountId { get; set; }
        public string OrgFormCode { get; set; }

        public EGRULRequestItem()
        {
            RequestTypeCode = OutcomeRequestType.Egrul;
        }
    }

    /// <summary>
    /// TODO: Add comment for SendRequestEGRULPage
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestEGRULPage : SendRequestDialogBase
    {
        #region [resourceStrings]
        protected static readonly string resRequestListCaption      = "$Resources:EGRULRequest_DlgRequestListCaption";
        protected static readonly string resPrecautionMessage       = "$Resources:EGRULRequest_DlgPrecautionMessageText";
        protected static readonly string resNoRequestMessage        = "$Resources:EGRULRequest_DlgNoRequestMessageText";
        protected static readonly string resRequestListTableHeader1 = "$Resources:EGRULRequest_DlgRequestListHeader1";
        protected static readonly string resRequestListTableHeader2 = "$Resources:EGRULRequest_DlgRequestListHeader2";
        protected static readonly string resOkButton                = "$Resources:EGRULRequest_DlgOkButtonText";
        protected static readonly string resCancelButton            = "$Resources:EGRULRequest_DlgCancelButtonText";
        protected static readonly string resNoAccountErrorFmt       = "$Resources:EGRULRequest_DlgNoAccountErrorFmt";
        protected static readonly string resErrorListCaption        = "$Resources:EGRULRequest_DlgErrorListCaption";
        protected static readonly string resErrorListHeader1        = "$Resources:EGRULRequest_DlgErrorListHeader1";
        protected static readonly string resNoDocumentsError        = "$Resources:EGRULRequest_DlgNoDocumentsError";
        protected static readonly string resProcessNotifyText       = "$Resources:EGRULRequest_DlgProcessNotifyText";
        protected static readonly string resAccIsEntrprnrErrorFmt   = "$Resources:EGRULRequest_DlgAccountIsEntrepreneurErrorFmt";
        #endregion

        #region [fields]
        protected static readonly string EGRULServiceGuidConfigName   = "BR2ServiceGuid";
        public static readonly string PrivateEntrepreneurCode      = "91";

        protected SPGridView requestListGrid;
        protected SPGridView errorListGrid;
        #endregion

        #region [methods]
        /// <summary>
        /// Getting bcs entity RequestAccount by Id
        /// </summary>
        /// <param name="id">Id of entity RequestAccount</param>
        /// <returns></returns>
        public static BcsCoordinateV5Model.RequestAccount GetRequestAccount(int id)
        {
            IEntity contentType = BCS.GetEntity(SPServiceContext.Current, String.Empty, 
                BCS.LOBRequestSystemNamespace, "RequestAccount");
            List<object> args = new List<object>();
            args.Add(id);
            var parameters = args.ToArray();
            return (BcsCoordinateV5Model.RequestAccount)BCS.GetDataFromMethod(BCS.LOBRequestSystemName, 
                contentType, "ReadRequestAccountItem", MethodInstanceType.SpecificFinder, ref parameters);
        }
        /// <summary>
        /// Building TaskMessage.Data.Parameter for CoordinateTaskMessage according to EGRUL request
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        protected XmlElement getTaskParam(BcsCoordinateV5Model.RequestAccount account)
        {
            XElement el = new XElement("ServiceProperties", 
                            new XAttribute("xmlns", String.Empty), 
                            new XElement("ogrn", account.Ogrn),
                            new XElement("inn", account.Inn));

            XmlDocument doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }
        /// <summary>
        ///  Building CoordinateTaskMessage
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual CoordinateTaskMessage GetRelevantCoordinateTaskMessage<T>(T item) where T : EGRULRequestItem
        {
            // request item
            SPListItem rItem = GetList().GetItemOrBreak(item.Id);
            var rDocument    = rItem["Tm_RequestedDocument"] == null ? 0 : new SPFieldLookupValue(rItem["Tm_RequestedDocument"].ToString()).LookupId;
            var sNumber      = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // request account
            BcsCoordinateV5Model.RequestAccount rAccount = GetRequestAccount(item.RequestAccountId);
            if (rAccount == null)
                throw new Exception(String.Format("Bcs entity with Id = {0} not found", item.RequestAccountId));
            // service code lookup item
            var stList  = Web.GetListOrBreak("Lists/GovServiceSubTypeBookList");
            var stItem  = stList.GetItemOrNull(rDocument);
            var sCode   = stItem == null ? String.Empty : 
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            var message = Helpers.GetEGRULMessageTemplate(getTaskParam(rAccount));
            message.ServiceHeader.ServiceNumber            = sNumber;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName  = Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber         = sNumber;
            message.TaskMessage.Task.ServiceTypeCode       = sCode;
            return message;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            #region [document list grid]
            requestListGrid = new SPGridView { AutoGenerateColumns = false };
            requestListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = "ID",
                DataField = "Id",
                Visible = false
            });
            requestListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = GetLocalizedString(resRequestListTableHeader1),
                DataField = "Title"
            });
            requestListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = GetLocalizedString(resRequestListTableHeader2),
                DataField = "RequestAccount"
            });
            requestListGrid.RowDataBound += requestListGrid_RowDataBound;
            RequestListTablePanel.Controls.Add(requestListGrid);
            #endregion
            #region [error list grid]
            errorListGrid = new SPGridView { AutoGenerateColumns = false };
            errorListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = GetLocalizedString(resErrorListHeader1),
                DataField = "Message",
            });
            ErrorListTablePanel.Controls.Add(errorListGrid);
            #endregion

            BtnOk.Text = GetLocalizedString(resOkButton);
            BtnCancel.Text = GetLocalizedString(resCancelButton);
        }

        void requestListGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var rowData = e.Row.DataItem as EGRULRequestItem;
                if ((rowData != null) && rowData.HasError)
                    e.Row.CssClass = "error-row";
            }
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }
        /// <summary>
        /// Loading documents
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected override List<T> LoadDocuments<T>()
        {
            SPList docList = GetList();
            var idList = ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();
            
            SPListItemCollection docItems = docList.GetItems(new SPQuery 
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString()
            });
            
            var retVal = new List<EGRULRequestItem>();
            foreach (SPListItem item in docItems)
            {
                BcsCoordinateV5Model.RequestAccount accountEntity = null;
                var accountStr  = item["Tm_RequestAccountBCSLookup"] != null ? item["Tm_RequestAccountBCSLookup"].ToString() : String.Empty;
                var accountId   = item["Tm_RequestAccountBCSLookup"] != null ? BCS.GetBCSFieldLookupId(item, "Tm_RequestAccountBCSLookup") : null;
                if (accountId != null)
                    accountEntity = GetRequestAccount((int)accountId);

                retVal.Add(new EGRULRequestItem
                {
                    Id                  = item.ID,
                    Title               = item.Title,
                    RequestAccount      = accountStr,
                    RequestAccountId    = accountId != null ? (int)accountId : 0,
                    OrgFormCode         = accountEntity != null ? accountEntity.OrgFormCode : String.Empty,
                    HasError            = false
                });
            }

            return retVal.Cast<T>().ToList();
        }

        protected override void BindDocuments<T>(List<T> documentList)
        {
            requestListGrid.DataSource = documentList;
            requestListGrid.DataBind();
        }

        protected override List<ValidationErrorInfo> ValidateDocuments<T>(List<T> documentList)
        {
            var retVal = new List<ValidationErrorInfo>();

            foreach (T document in documentList)
            {
                EGRULRequestItem doc = document as EGRULRequestItem;
                #region [Rule#1 - RequestAccount cannot be null]
                if (String.IsNullOrEmpty(doc.RequestAccount))
                {
                    retVal.Add(new ValidationErrorInfo
                    {
                        Message = String.Format(GetLocalizedString(resNoAccountErrorFmt), doc.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    doc.HasError = true;
                }
                #endregion
                #region [Rule#2 - RequestAccount must be legal person (not private entrepreneur)]
                if (doc.OrgFormCode == PrivateEntrepreneurCode)
                {
                    retVal.Add(new ValidationErrorInfo
                    {
                        Message = String.Format(GetLocalizedString(resAccIsEntrprnrErrorFmt), doc.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    doc.HasError = true;
                }
                #endregion
            }

            if (documentList.All(i => i.HasError))
                retVal.Add(new ValidationErrorInfo
                {
                    Message = GetLocalizedString(resNoDocumentsError),
                    Severity = ValidationErrorSeverity.Critical
                });

            return retVal;
        }

        protected override void BindErrors(List<ValidationErrorInfo> errorList)
        {
            errorListGrid.DataSource = errorList;
            errorListGrid.DataBind();
        }

        protected override void HandleDocumentsLoad<T>(List<T> documentList, List<ValidationErrorInfo> errorList)
        {
            // Save state
            ViewState["requestDocumentList"] = documentList.Cast<EGRULRequestItem>().ToList();
            // UI
            RequestList.Visible = !(documentList.All(i => i.HasError));
            ErrorList.Visible = errorList.Count() > 0;
            BtnOk.Enabled = !errorList.Any(err => err.Severity == ValidationErrorSeverity.Critical);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            EndOperation(0);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            var success = false;
            try
            {
                var documentList = (List<EGRULRequestItem>)ViewState["requestDocumentList"];
                success = SendRequests<EGRULRequestItem>(documentList);
            }
            catch (Exception)
            {
                EndOperation(-1);
            }
            
            EndOperation(success ? 1 : -1);
        }

        protected override ServiceClients.MessageQueue.Message BuildMessage<T>(T document)
        {
            EGRULRequestItem doc = document as EGRULRequestItem;
            SPListItem configItem = Config.GetConfigItem(Web, EGRULServiceGuidConfigName);
            var svcGuid = Config.GetConfigValue(configItem);
            var svc = GetServiceClientInstance().GetService(new Guid(svcGuid.ToString()));
            var internalMessage = GetRelevantCoordinateTaskMessage(doc);
            
            return new MessageQueueService.Message
            {
                Service         = svc,
                MessageId       = new Guid(internalMessage.ServiceHeader.MessageId),
                MessageType     = 2,
                MessageMethod   = 2,
                MessageDate     = DateTime.Now,
                MessageText     = internalMessage.ToXElement<CoordinateTaskMessage>().ToString(),
                RequestId       = new Guid(internalMessage.TaskMessage.Task.RequestId)
            };
        }
        #endregion

    }
}

