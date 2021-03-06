﻿// <copyright file="SendRequestPenaltyPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-11-24 18:47:01Z</date>

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CamlexNET;
using TM.SP.AppPages.ApplicationPages;
using TM.SP.AppPages.Communication;
using TM.SP.AppPages.Tracker;
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
            RequestTypeCode = OutcomeRequest.Penalty;
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
                              TaxiStateNumber = item.TryGetValue<string>("Tm_TaxiStateNumber"),
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
            if (doc == null)
                throw new Exception("Must be of type PenaltyRequestItem");

            var svcGuid = new Guid(Config.GetConfigValueOrDefault<string>(Web, PenaltyServiceGuidConfigName));
            var spItem = GetList().GetItemOrBreak(doc.Id);
            var buildOptions = new QueueMessageBuildOptions { Date = DateTime.Now, Method = 2, ServiceGuid = svcGuid };
            return QueueMessageBuilder.Build(new CoordinateV5PenaltyMessageBuilder(spItem), QueueClient, buildOptions);
        }

        protected override void TrackOutcomeRequest<T>(T document, bool success, Guid requestId)
        {
            if (!success) return;

            var licItem = Web.GetListOrBreak("Lists/LicenseList").GetItemById(document.Id);
            var tracker = new RequestTracker(new LicenseTrackingContext(licItem),
                new OutcomeRequestTrackingData {Id = requestId, Type = OutcomeRequest.Penalty});
            tracker.Track();
        }
    }
}

