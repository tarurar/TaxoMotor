// <copyright file="SendRequestCombi.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-12-10 17:38:53Z</date>

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CamlexNET;
using Microsoft.BusinessData.MetadataModel;
using TM.SP.AppPages.ApplicationPages;

// ReSharper disable CheckNamespace
namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using MessageQueueService = ServiceClients.MessageQueue;
    using BcsCoordinateV5Model = BCSModels.CoordinateV5;
    using Utils;
    using Services.CoordinateV5;


    [Serializable]
    public class CombiRequestItem : RequestItem, IEquatable<CombiRequestItem>
    {
        public string RequestAccount { get; set; }
        public int RequestAccountId { get; set; }
        public string OrgFormCode { get; set; }
        public string Ogrn { get; set; }
        public string Inn { get; set; }
        public bool Equals(CombiRequestItem other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return Ogrn.Equals(other.Ogrn);
        }

        public override int GetHashCode()
        {
            return Ogrn != String.Empty ? Ogrn.GetHashCode() : 0;
        }
    }

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestCombi : SendRequestDialogBase
    {
        #region [resourceStrings]
// ReSharper disable InconsistentNaming
        protected static readonly string resRequestListCaption      = "$Resources:CombiRequest_DlgRequestListCaption";
        protected static readonly string resProcessNotifyText       = "$Resources:CombiRequest_DlgProcessNotifyText";
        protected static readonly string resErrorListCaption        = "$Resources:CombiRequest_DlgErrorListCaption";
        protected static readonly string resPrecautionMessage       = "$Resources:CombiRequest_DlgPrecautionMessageText";
        protected static readonly string resNoRequestMessage        = "$Resources:CombiRequest_DlgNoRequestMessageText";
        protected static readonly string resRequestListTableHeader1 = "$Resources:CombiRequest_DlgRequestListHeader1";
        protected static readonly string resRequestListTableHeader2 = "$Resources:CombiRequest_DlgRequestListHeader2";
        protected static readonly string resOkButton                = "$Resources:CombiRequest_DlgOkButtonText";
        protected static readonly string resCancelButton            = "$Resources:CombiRequest_DlgCancelButtonText";
        protected static readonly string resNoAccountErrorFmt       = "$Resources:CombiRequest_DlgNoAccountErrorFmt";
        protected static readonly string resErrorListHeader1        = "$Resources:CombiRequest_DlgErrorListHeader1";
        protected static readonly string resNoDocumentsError        = "$Resources:CombiRequest_DlgNoDocumentsError";
// ReSharper restore InconsistentNaming
        #endregion

        #region [fields]
        protected static readonly string ServiceGuidConfigName = "BR2ServiceGuid";
        public static readonly string PrivateEntrepreneurCode  = "91";

        protected SPGridView RequestListGrid;
        protected SPGridView ErrorListGrid;
        #endregion

        #region [methods]

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            #region [document list grid]
            RequestListGrid = new SPGridView { AutoGenerateColumns = false };
            RequestListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = "ID",
                DataField = "Id",
                Visible = false
            });
            RequestListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = GetLocalizedString(resRequestListTableHeader1),
                DataField = "Title"
            });
            RequestListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = GetLocalizedString(resRequestListTableHeader2),
                DataField = "RequestAccount"
            });
            RequestListGrid.RowDataBound += requestListGrid_RowDataBound;
            RequestListTablePanel.Controls.Add(RequestListGrid);
            #endregion
            #region [error list grid]
            ErrorListGrid = new SPGridView { AutoGenerateColumns = false };
            ErrorListGrid.Columns.Add(new SPBoundField
            {
                HeaderText = GetLocalizedString(resErrorListHeader1),
                DataField = "Message",
            });
            ErrorListTablePanel.Controls.Add(ErrorListGrid);
            #endregion

            BtnOk.Text     = GetLocalizedString(resOkButton);
            BtnCancel.Text = GetLocalizedString(resCancelButton);
        }

        void requestListGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var rowData = e.Row.DataItem as CombiRequestItem;
                if ((rowData != null) && rowData.HasError)
                    e.Row.CssClass = "error-row";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            EndOperation(0);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var success = false;
            try
            {
                var documentList = (List<CombiRequestItem>)ViewState["requestDocumentList"];
                success = SendRequests(documentList);
            }
            catch (Exception)
            {
                EndOperation(-1);
            }

            EndOperation(success ? 1 : -1);
        }

        protected override ServiceClients.MessageQueue.Message BuildMessage<T>(T document)
        {
            var doc = document as CombiRequestItem;
            SPListItem configItem = Config.GetConfigItem(Web, ServiceGuidConfigName);
            var svcGuid = Config.GetConfigValue(configItem);
            var svc = GetServiceClientInstance().GetService(new Guid(svcGuid.ToString()));
            var internalMessage = GetRelevantCoordinateTaskMessage(doc);

            return new MessageQueueService.Message
            {
                Service       = svc,
                MessageId     = new Guid(internalMessage.ServiceHeader.MessageId),
                MessageType   = 2,
                MessageMethod = 2,
                MessageDate   = DateTime.Now,
                MessageText   = internalMessage.ToXElement<CoordinateTaskMessage>().ToString(),
                RequestId     = new Guid(internalMessage.TaskMessage.Task.RequestId)
            };
        }

        protected virtual CoordinateTaskMessage GetRelevantCoordinateTaskMessage<T>(T item) where T : CombiRequestItem
        {
            const string snPattern = "{0}-{1}-{2}-{3}/{4}";
            string sn = String.Format(snPattern, Consts.TaxoMotorDepCode, Consts.TaxoMotorSysCode, "77200101",
                String.Format("{0:000000}", 1), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(2));
            var isJuridical = item.OrgFormCode.Trim() != PrivateEntrepreneurCode;
            var paramValue = GetTaskParam(item.Ogrn, item.Inn);

            var message = isJuridical
                ? Helpers.GetEGRULMessageTemplate(paramValue)
                : Helpers.GetEGRIPMessageTemplate(paramValue);
            message.ServiceHeader.ServiceNumber = sn;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName = Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber = sn;
            message.TaskMessage.Task.ServiceTypeCode = "77200101";
            return message;
        }

        protected XmlElement GetTaskParam(string ogrn, string inn)
        {
            var el = new XElement("ServiceProperties",
                new XAttribute("xmlns", String.Empty),
                new XElement("ogrn", ogrn),
                new XElement("inn", inn));

            var doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }

        protected override List<T> LoadDocuments<T>()
        {
            SPList docList = GetList();
            var listName = docList.RootFolder.Name;
            var idList = ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();

            SPListItemCollection docItems = docList.GetItems(new SPQuery
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            var retVal = (from SPListItem item in docItems
                let orgCode = item["Tm_OrgLfb"] != null ? item["Tm_OrgLfb"].ToString().Trim() : String.Empty
                select new CombiRequestItem
                {
                    Id = item.ID,
                    Title = item.Title,
                    RequestAccount =
                        item["Tm_OrganizationName"] != null ? item["Tm_OrganizationName"].ToString() : String.Empty,
                    RequestAccountId = 0,
                    OrgFormCode = orgCode,
                    HasError = false,
                    Ogrn = item["Tm_OrgOgrn"] != null ? item["Tm_OrgOgrn"].ToString() : String.Empty,
                    Inn = item["Tm_OrgInn"] != null ? item["Tm_OrgInn"].ToString() : String.Empty,
                    RequestTypeCode =
                        orgCode == PrivateEntrepreneurCode ? OutcomeRequestType.Egrip : OutcomeRequestType.Egrul,
                    ListName = listName
                }).ToList();

            return retVal.Cast<T>().ToList();
        }

        protected override void BindDocuments<T>(List<T> documentList)
        {
            RequestListGrid.DataSource = documentList;
            RequestListGrid.DataBind();
        }


        protected override List<ValidationErrorInfo> ValidateDocuments<T>(List<T> documentList)
        {
            var retVal = new List<ValidationErrorInfo>();

            foreach (T document in documentList)
            {
                var doc = document as CombiRequestItem;
                #region [Rule#1 - RequestAccount cannot be null]
                if (doc != null && String.IsNullOrEmpty(doc.RequestAccount))
                {
                    retVal.Add(new ValidationErrorInfo
                    {
                        Message = String.Format(GetLocalizedString(resNoAccountErrorFmt), doc.Title),
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
            ErrorListGrid.DataSource = errorList;
            ErrorListGrid.DataBind();
        }

        protected override void HandleDocumentsLoad<T>(List<T> documentList, List<ValidationErrorInfo> errorList)
        {
            // Save state
            ViewState["requestDocumentList"] = documentList.Cast<CombiRequestItem>().ToList();
            // UI
            RequestList.Visible = !(documentList.All(i => i.HasError));
            ErrorList.Visible   = errorList.Any();
            BtnOk.Enabled       = errorList.All(err => err.Severity != ValidationErrorSeverity.Critical);
        }

        protected override SPListItem TrackOutcomeRequest<T>(T document, bool success, Guid requestId)
        {
            if (!success) return null;

            var trackList = Web.GetListOrBreak("Lists/OutcomeRequestStateList");
            var requestTypeList = Web.GetListOrBreak("Lists/OutcomeRequestTypeBookList");
            var requestTypeItem = requestTypeList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                ((int)document.RequestTypeCode).ToString(CultureInfo.InvariantCulture));
            var requestList = Web.GetListOrBreak("Lists/IncomeRequestList");
            var licList = Web.GetListOrBreak("Lists/LicenseList");

            SPListItem rItem = null;
            SPListItem licItem = null;
            if (document.ListName == "IncomeRequestList") rItem = requestList.GetItemById(document.Id);
            if (document.ListName == "LicenseList") licItem = licList.GetItemById(document.Id);        

            var newItem = trackList.AddItem();
            newItem["Title"] = requestTypeItem != null ? requestTypeItem.Title : "Запрос";
            newItem["Tm_OutputDate"] = DateTime.Now;
            newItem["Tm_IncomeRequestLookup"] = rItem != null ? new SPFieldLookupValue(rItem.ID, rItem.Title) : null;
            newItem["Tm_OutputRequestTypeLookup"] = requestTypeItem != null
                ? new SPFieldLookupValue(requestTypeItem.ID, requestTypeItem.Title)
                : null;
            newItem["Tm_LicenseLookup"] = licItem != null ? licItem["Tm_LicenseRtParentLicenseLookup"] : null;
            newItem["Tm_AnswerReceived"] = false;
            newItem["Tm_MessageId"] = requestId;
            newItem.Update();

            return newItem;
        }

        #endregion

    }


}

