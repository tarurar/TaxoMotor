// <copyright file="SendRequestPenaltyPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-11-24 18:47:01Z</date>

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CamlexNET;
using TM.Services.CoordinateV5;
using TM.SP.AppPages.ApplicationPages;
using TM.Utils;

// ReSharper disable CheckNamespace
namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using MessageQueueService = ServiceClients.MessageQueue;


    [Serializable]
    public class PenaltyRequestItem : RequestItem
    {
        public string TaxiStateNumber { get; set; }

        public PenaltyRequestItem()
        {
            RequestTypeCode = OutcomeRequestType.Penalty;
        }
    }

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestPenaltyPage : SendRequestDialogBase
    {
        #region [resourceStrings]
// ReSharper disable InconsistentNaming
        protected static readonly string resRequestListCaption      = "$Resources:PenaltyRequest_DlgRequestListCaption";
        protected static readonly string resPrecautionMessage       = "$Resources:PenaltyRequest_DlgPrecautionMessageText";
        protected static readonly string resNoRequestMessage        = "$Resources:PenaltyRequest_DlgNoRequestMessageText";
        protected static readonly string resRequestListTableHeader1 = "$Resources:PenaltyRequest_DlgRequestListHeader1";
        protected static readonly string resRequestListTableHeader2 = "$Resources:PenaltyRequest_DlgRequestListHeader2";
        protected static readonly string resOkButton                = "$Resources:PenaltyRequest_DlgOkButtonText";
        protected static readonly string resCancelButton            = "$Resources:PenaltyRequest_DlgCancelButtonText";
        protected static readonly string resErrorListCaption        = "$Resources:PenaltyRequest_DlgErrorListCaption";
        protected static readonly string resErrorListHeader1        = "$Resources:PenaltyRequest_DlgErrorListHeader1";
        protected static readonly string resNoDocumentsError        = "$Resources:PenaltyRequest_DlgNoDocumentsError";
        protected static readonly string resProcessNotifyText       = "$Resources:PenaltyRequest_DlgProcessNotifyText";
        protected static readonly string resNoStateNumberErrorFmt   = "$Resources:PenaltyRequest_DlgNoStateNumberErrorFmt";
// ReSharper restore InconsistentNaming
        #endregion

        #region [fields]
        protected static readonly string PenaltyServiceGuidConfigName = "BR2ServiceGuid";

// ReSharper disable InconsistentNaming
        protected SPGridView requestListGrid;
        protected SPGridView errorListGrid;
// ReSharper restore InconsistentNaming
        #endregion


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
                DataField = "TaxiStateNumber"
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
                var rowData = e.Row.DataItem as PenaltyRequestItem;
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            EndOperation(0);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var success = false;
            try
            {
                var documentList = (List<PenaltyRequestItem>)ViewState["requestDocumentList"];
                success = SendRequests(documentList);
            }
            catch (Exception)
            {
                EndOperation(-1);
            }

            EndOperation(success ? 1 : -1);
        }

        protected override List<T> LoadDocuments<T>()
        {
            SPList docList = GetList();
            var idList = ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();

            SPListItemCollection docItems = docList.GetItems(new SPQuery
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            var retVal = (from SPListItem item in docItems
                          select new PenaltyRequestItem
                          {
                              Id              = item.ID,
                              Title           = item.Title,
                              TaxiStateNumber = item["Tm_TaxiStateNumber"] != null ? item["Tm_TaxiStateNumber"].ToString() : String.Empty,
                              HasError        = false
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
                var doc = document as PenaltyRequestItem;
                if (doc == null) continue;

                #region [Rule#1 - State Number cannot be null]
                if (String.IsNullOrEmpty(doc.TaxiStateNumber))
                {
                    retVal.Add(new ValidationErrorInfo
                    {
                        Message = String.Format(GetLocalizedString(resNoStateNumberErrorFmt), doc.Title),
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
            ViewState["requestDocumentList"] = documentList.Cast<PenaltyRequestItem>().ToList();
            // UI
            RequestList.Visible = !(documentList.All(i => i.HasError));
            ErrorList.Visible = errorList.Any();
            BtnOk.Enabled = errorList.All(err => err.Severity != ValidationErrorSeverity.Critical);
        }

        protected override ServiceClients.MessageQueue.Message BuildMessage<T>(T document)
        {
            var doc = document as PenaltyRequestItem;
            SPListItem configItem = Config.GetConfigItem(Web, PenaltyServiceGuidConfigName);
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

// ReSharper disable InconsistentNaming
        protected XmlElement getTaskParam(SPListItem licenseItem)
// ReSharper restore InconsistentNaming
        {
            var el = new XElement("ServiceProperties",
                            new XAttribute("xmlns", String.Empty),
                            new XElement("regpointnum", licenseItem["Tm_TaxiStateNumber"]));

            var doc = new XmlDocument();
            doc.Load(el.CreateReader());

            return doc.DocumentElement;
        }

        protected virtual CoordinateTaskMessage GetRelevantCoordinateTaskMessage<T>(T item) where T : PenaltyRequestItem
        {
            const string snPattern = "{0}-{1}-{2}-{3}/{4}";
            string sn = String.Format(snPattern, Consts.TaxoMotorDepCode, Consts.TaxoMotorSysCode, "77200101",
                String.Format("{0:000000}", 1), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(2));

            #region [Getting list instances]
            var licList = Web.GetListOrBreak("Lists/LicenseList");
            var licItem = licList.GetItemById(item.Id);
            #endregion

            #region [Building outcome request]
            var message = Helpers.GetPenaltyMessageTemplate(getTaskParam(licItem));
            message.ServiceHeader.ServiceNumber            = sn;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName  = Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber         = sn;
            message.TaskMessage.Task.ServiceTypeCode       = "77200101";
            #endregion

            return message;
        }

        protected override SPListItem TrackOutcomeRequest<T>(T document, bool success, Guid requestId)
        {
            if (!success) return null;

            var trackList       = Web.GetListOrBreak("Lists/OutcomeRequestStateList");
            var requestTypeList = Web.GetListOrBreak("Lists/OutcomeRequestTypeBookList");
            var requestTypeItem = requestTypeList.GetSingleListItemByFieldValue("Tm_ServiceCode",
                ((int) document.RequestTypeCode).ToString(CultureInfo.InvariantCulture));
            var licList = Web.GetListOrBreak("Lists/LicenseList");
            var licItem = licList.GetItemById(document.Id);
            SPListItem taxiItem;
            Utility.TryGetListItemFromLookupValue(licItem["Tm_TaxiLookup"],
                licItem.Fields.GetFieldByInternalName("Tm_TaxiLookup") as SPFieldLookup, out taxiItem);

            var pFolder = CreateOutcomeRequestFolder(trackList);
            var newItem = trackList.AddItem(pFolder.ServerRelativeUrl, SPFileSystemObjectType.File);
            newItem["Title"]                           = requestTypeItem != null ? requestTypeItem.Title : "Запрос";
            newItem["Tm_OutputDate"]                   = DateTime.Now;
            newItem["Tm_TaxiLookup"]                   = licItem["Tm_TaxiLookup"];
            newItem["Tm_IncomeRequestLookup"]          = taxiItem != null ? taxiItem["Tm_IncomeRequestLookup"] : null;
            newItem["Tm_LicenseLookup"]                = new SPFieldLookupValue(licItem.ID, licItem.Title);
            newItem["Tm_LicenseRtParentLicenseLookup"] = licItem["Tm_LicenseRtParentLicenseLookup"];
            newItem["Tm_OutputRequestTypeLookup"]      = requestTypeItem != null ? new SPFieldLookupValue(requestTypeItem.ID, requestTypeItem.Title) : null;
            newItem["Tm_AnswerReceived"]               = false;
            newItem["Tm_MessageId"]                    = requestId;
            newItem.Update();

            return newItem;
        }
    }
}

