// <copyright file="SendRequestPTSPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-09-11 17:00:28Z</date>

using System.Globalization;
using TM.SP.AppPages.Communication;

// ReSharper disable once CheckNamespace
namespace TM.SP.AppPages
{
    using System;
    using System.Security.Permissions;
    using System.Web.UI.WebControls;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using CamlexNET;

    using ApplicationPages;
    using BcsCoordinateV5Model = BCSModels.CoordinateV5;
    using Utils;
    using MessageQueueService = ServiceClients.MessageQueue;

    [Serializable]
    public class PtsRequestItem : RequestItem
    {
        public string TaxiStateNumber { get; set; }

        public PtsRequestItem()
        {
            RequestTypeCode = OutcomeRequestType.Pts;
        }
    }

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
// ReSharper disable once InconsistentNaming
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
        protected static readonly string PtsServiceGuidConfigName = "BR2ServiceGuid";

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
                var rowData = e.Row.DataItem as PtsRequestItem;
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
            var docList = GetList();
            var listName = docList.RootFolder.Name;
            var idList = ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();

            var docItems = docList.GetItems(new SPQuery
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            var retVal = (from SPListItem item in docItems
                select new PtsRequestItem
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

            foreach (var document in documentList)
            {
                var doc = document as PtsRequestItem;
                #region [Rule#1 - State Number cannot be null]
                if (doc !=null && String.IsNullOrEmpty(doc.TaxiStateNumber))
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
            ViewState["requestDocumentList"] = documentList.Cast<PtsRequestItem>().ToList();
            // UI
            RequestList.Visible = !(documentList.All(i => i.HasError));
            ErrorList.Visible = errorList.Any();
            BtnOk.Enabled = errorList.All(err => err.Severity != ValidationErrorSeverity.Critical);
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
                var documentList = (List<PtsRequestItem>)ViewState["requestDocumentList"];
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
            var doc = document as PtsRequestItem;
            if (doc == null)
                throw new Exception("Must be of type PtsRequestItem");

            var svcGuid = new Guid(Config.GetConfigValueOrDefault<string>(Web, PtsServiceGuidConfigName));
            var spItem = GetList().GetItemOrBreak(doc.Id);
            var buildOptions = new QueueMessageBuildOptions {Date = DateTime.Now, Method = 2, ServiceGuid = svcGuid};
            return QueueMessageBuilder.Build(new CoordinateV5PtsMessageBuilder(spItem), QueueClient, buildOptions);
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

            var pFolder = CreateOutcomeRequestFolder(trackList);
            var newItem = trackList.AddItem(pFolder.ServerRelativeUrl, SPFileSystemObjectType.File);
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

