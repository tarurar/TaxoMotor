﻿// <copyright file="SendRequestPTSPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-09-11 17:00:28Z</date>

using System.Globalization;

namespace TM.SP.AppPages
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
    using CamlexNET;

    using TM.SP.AppPages.ApplicationPages;
    using BcsCoordinateV5Model = TM.SP.BCSModels.CoordinateV5;
    using TM.Utils;
    using TM.Services.CoordinateV5;
    using MessageQueueService = TM.ServiceClients.MessageQueue;

    [Serializable]
    public class PTSRequestItem : RequestItem
    {
        public string TaxiStateNumber { get; set; }

        public PTSRequestItem()
        {
            RequestTypeCode = OutcomeRequestType.Pts;
        }
    }

    /// <summary>
    /// TODO: Add comment for SendRequestPTSPage
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestPTSPage : SendRequestDialogBase
    {
        #region [resourceStrings]
// ReSharper disable InconsistentNaming
        protected static readonly string resRequestListCaption      = "$Resources:PTSRequest_DlgRequestListCaption";
        protected static readonly string resPrecautionMessage       = "$Resources:PTSRequest_DlgPrecautionMessageText";
        protected static readonly string resNoRequestMessage        = "$Resources:PTSRequest_DlgNoRequestMessageText";
        protected static readonly string resRequestListTableHeader1 = "$Resources:PTSRequest_DlgRequestListHeader1";
        protected static readonly string resRequestListTableHeader2 = "$Resources:PTSRequest_DlgRequestListHeader2";
        protected static readonly string resOkButton                = "$Resources:PTSRequest_DlgOkButtonText";
        protected static readonly string resCancelButton            = "$Resources:PTSRequest_DlgCancelButtonText";
        protected static readonly string resErrorListCaption        = "$Resources:PTSRequest_DlgErrorListCaption";
        protected static readonly string resErrorListHeader1        = "$Resources:PTSRequest_DlgErrorListHeader1";
        protected static readonly string resNoDocumentsError        = "$Resources:PTSRequest_DlgNoDocumentsError";
        protected static readonly string resNoStateNumberErrorFmt   = "$Resources:PTSRequest_DlgNoStateNumberErrorFmt";
        protected static readonly string resProcessNotifyText       = "$Resources:PTSRequest_DlgProcessNotifyText";
// ReSharper restore InconsistentNaming
        #endregion

        #region [fields]
        protected static readonly string PTSServiceGuidConfigName = "BR2ServiceGuid";

        protected SPGridView requestListGrid;
        protected SPGridView errorListGrid;
        #endregion

        #region [methods]

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            #region [document list grid]
            requestListGrid = new SPGridView() { AutoGenerateColumns = false };
            requestListGrid.Columns.Add(new SPBoundField()
            {
                HeaderText = "ID",
                DataField = "Id",
                Visible = false
            });
            requestListGrid.Columns.Add(new SPBoundField()
            {
                HeaderText = GetLocalizedString(resRequestListTableHeader1),
                DataField = "Title"
            });
            requestListGrid.Columns.Add(new SPBoundField()
            {
                HeaderText = GetLocalizedString(resRequestListTableHeader2),
                DataField = "TaxiStateNumber"
            });
            requestListGrid.RowDataBound += requestListGrid_RowDataBound;
            RequestListTablePanel.Controls.Add(requestListGrid);
            #endregion
            #region [error list grid]
            errorListGrid = new SPGridView() { AutoGenerateColumns = false };
            errorListGrid.Columns.Add(new SPBoundField()
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
                var rowData = e.Row.DataItem as PTSRequestItem;
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

            this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
            this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
        }

        /// <summary>
        /// Loading documents
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected override List<T> LoadDocuments<T>()
        {
            SPList docList = GetList();
            var listName = docList.RootFolder.Name;
            var idList = ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();

            SPListItemCollection docItems = docList.GetItems(new SPQuery()
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            var retVal = (from SPListItem item in docItems
                select new PTSRequestItem()
                {
                    Id = item.ID,
                    Title = item.Title,
                    TaxiStateNumber =
                        item["Tm_TaxiStateNumber"] != null ? item["Tm_TaxiStateNumber"].ToString() : String.Empty,
                    HasError = false,
                    ListName = listName
                }).ToList();

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
                var doc = document as PTSRequestItem;
                #region [Rule#1 - State Number cannot be null]
                if (doc !=null && String.IsNullOrEmpty(doc.TaxiStateNumber))
                {
                    retVal.Add(new ValidationErrorInfo()
                    {
                        Message = String.Format(GetLocalizedString(resNoStateNumberErrorFmt), doc.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    doc.HasError = true;
                }
                #endregion
            }

            if (documentList.All(i => i.HasError))
                retVal.Add(new ValidationErrorInfo()
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
            this.ViewState["requestDocumentList"] = documentList.Cast<PTSRequestItem>().ToList();
            // UI
            RequestList.Visible = !(documentList.All(i => i.HasError));
            ErrorList.Visible = errorList.Count() > 0;
            BtnOk.Enabled = !errorList.Any<ValidationErrorInfo>(err => err.Severity == ValidationErrorSeverity.Critical);
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.EndOperation(0);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var success = false;
            try
            {
                var documentList = (List<PTSRequestItem>)this.ViewState["requestDocumentList"];
                success = SendRequests<PTSRequestItem>(documentList);
            }
            catch (Exception)
            {
                this.EndOperation(-1);
            }

            this.EndOperation(success ? 1 : -1);
        }

        protected override ServiceClients.MessageQueue.Message BuildMessage<T>(T document)
        {
            var doc = document as PTSRequestItem;
            SPListItem configItem = Config.GetConfigItem(this.Web, PTSServiceGuidConfigName);
            var svcGuid = Config.GetConfigValue(configItem);
            var svc = GetServiceClientInstance().GetService(new Guid(svcGuid.ToString()));
            var internalMessage = GetRelevantCoordinateTaskMessage(doc);

            return new MessageQueueService.Message()
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

        protected XmlElement GetTaskParam(string stateNumber)
        {
            var el = new XElement("ServiceProperties",
                new XAttribute("xmlns", String.Empty),
                new XElement("regno", stateNumber));

            var doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }

        protected virtual CoordinateTaskMessage GetRelevantCoordinateTaskMessage<T>(T item) where T : PTSRequestItem
        {
            const string snPattern = "{0}-{1}-{2}-{3}/{4}";

            #region [Getting list instances]
            var irList  = this.Web.GetListOrBreak("Lists/IncomeRequestList");
            var stList  = this.Web.GetListOrBreak("Lists/GovServiceSubTypeBookList");
            #endregion
            #region [Getting linked items from lists]
            string sNumber = null;
            string sCode = null;

            if (item.ListName == "TaxiList")
            {
                SPListItem rItem = Web.GetListOrBreak(String.Format("Lists/{0}", item.ListName)).GetItemOrBreak(item.Id);
                var irId = rItem["Tm_IncomeRequestLookup"] == null
                    ? 0
                    : new SPFieldLookupValue(rItem["Tm_IncomeRequestLookup"].ToString()).LookupId;
                var irItem = irList.GetItemOrNull(irId);
                if (irItem != null)
                {
                    int irDocId = irItem["Tm_RequestedDocument"] == null
                        ? 0
                        : new SPFieldLookupValue(irItem["Tm_RequestedDocument"].ToString()).LookupId;
                    SPListItem stItem = stList.GetItemOrNull(irDocId);

                    sNumber = irItem["Tm_SingleNumber"] == null ? String.Empty : irItem["Tm_SingleNumber"].ToString();
                    sCode = stItem == null
                        ? String.Empty
                        : (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());
                }
            }
                
            if (String.IsNullOrEmpty(sNumber))
            {
                sNumber = String.Format(snPattern, Consts.TaxoMotorDepCode, Consts.TaxoMotorSysCode, "77200101",
                    String.Format("{0:000000}", 1), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(2));
            }
            if (String.IsNullOrEmpty(sCode))
            {
                sCode = "77200101";
            }

            #endregion
            
            #region [Building outcome request]
            var message = Helpers.GetPTSMessageTemplate(GetTaskParam(item.TaxiStateNumber));
            message.ServiceHeader.ServiceNumber            = sNumber;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName  = this.Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber         = sNumber;
            message.TaskMessage.Task.ServiceTypeCode       = sCode;
            #endregion

            return message;
        }

        protected override SPListItem TrackOutcomeRequest<T>(T document, bool success, Guid requestId)
        {
            if (!success) return null;

            var trackList = Web.GetListOrBreak("Lists/OutcomeRequestStateList");
            var requestTypeList = Web.GetListOrBreak("Lists/OutcomeRequestTypeBookList");
            var requestTypeItem = requestTypeList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                ((int)document.RequestTypeCode).ToString(CultureInfo.InvariantCulture));
            var taxiList = Web.GetListOrBreak("Lists/TaxiList");
            var licList = Web.GetListOrBreak("Lists/LicenseList");

            SPListItem taxiItem = null;
            SPListItem licItem = null;
            if (document.ListName == "TaxiList") taxiItem = taxiList.GetItemById(document.Id);
            if (document.ListName == "LicenseList") licItem = licList.GetItemById(document.Id);        

            var newItem = trackList.AddItem();
            newItem["Title"] = requestTypeItem != null ? requestTypeItem.Title : "Запрос";
            newItem["Tm_OutputDate"] = DateTime.Now;
            newItem["Tm_TaxiLookup"] = taxiItem != null ? new SPFieldLookupValue(document.Id, document.Title) : null;
            newItem["Tm_IncomeRequestLookup"] = taxiItem != null ? taxiItem["Tm_IncomeRequestLookup"] : null;
            newItem["Tm_OutputRequestTypeLookup"] = requestTypeItem != null ? new SPFieldLookupValue(requestTypeItem.ID, requestTypeItem.Title) : null;
            newItem["Tm_LicenseLookup"] = licItem != null ? new SPFieldLookupValue(licItem.ID, licItem.Title) : null;
            newItem["Tm_LicenseRtParentLicenseLookup"] = licItem != null
                ? licItem["Tm_LicenseRtParentLicenseLookup"]
                : null;
            newItem["Tm_AnswerReceived"] = false;
            newItem["Tm_MessageId"] = requestId;
            newItem.Update();

            return newItem;
        }

        #endregion
    }
}

