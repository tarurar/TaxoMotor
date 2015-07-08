// <copyright file="SendRequestEGRULPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-08-06 17:56:53Z</date>

using TM.SP.AppPages.Communication;
// ReSharper disable CheckNamespace


namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
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
// ReSharper disable once InconsistentNaming
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

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
// ReSharper disable once InconsistentNaming
    public partial class SendRequestEGRULPage : SendRequestDialogBase
    {
        #region [resourceStrings]
// ReSharper disable InconsistentNaming
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
// ReSharper restore InconsistentNaming
        #endregion

        #region [fields]
        protected static readonly string EgrulServiceGuidConfigName   = "BR2ServiceGuid";
        public static readonly string PrivateEntrepreneurCode = "91";

        protected SPGridView requestListGrid;
        protected SPGridView errorListGrid;
        #endregion

        #region [methods]
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
            var listName = docList.RootFolder.Name;
            var idList = ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();
            
            SPListItemCollection docItems = docList.GetItems(new SPQuery 
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });
            
            if (listName == "LicenseList")
            {
                var taxiIdList = (from SPListItem item in docItems
                    select item["Tm_TaxiLookup"]
                    into taxiLookup
                    where taxiLookup != null
                    select new SPFieldLookupValue(taxiLookup.ToString())
                    into taxiLookupValue
                    select taxiLookupValue.LookupId).ToList();

                var taxiList = Web.GetListOrBreak("Lists/TaxiList");
                SPListItemCollection taxiItems = taxiList.GetItems(new SPQuery
                {
                    Query = Camlex.Query().Where(x => taxiIdList.Contains((int)x["ID"])).ToString(),
                    ViewAttributes = "Scope='RecursiveAll'"
                });

                var requestIdList = (from SPListItem item in taxiItems
                    select item["Tm_IncomeRequestLookup"]
                    into requestLookup
                    where requestLookup != null
                    select new SPFieldLookupValue(requestLookup.ToString())
                    into requestLookupValue
                    select requestLookupValue.LookupId).ToList();

                var rList = Web.GetListOrBreak("Lists/IncomeRequestList");
                docItems = rList.GetItems(new SPQuery
                {
                    Query = Camlex.Query().Where(x => requestIdList.Contains((int)x["ID"])).ToString(),
                    ViewAttributes = "Scope='RecursiveAll'"
                });
            }

            var retVal = new List<EGRULRequestItem>();
            foreach (SPListItem item in docItems)
            {
                BcsCoordinateV5Model.RequestAccount accountEntity = null;
                var accountStr  = item["Tm_RequestAccountBCSLookup"] != null ? item["Tm_RequestAccountBCSLookup"].ToString() : String.Empty;
                var accountId   = item["Tm_RequestAccountBCSLookup"] != null ? BCS.GetBCSFieldLookupId(item, "Tm_RequestAccountBCSLookup") : null;
                if (accountId != null)
                    accountEntity = IncomeRequestHelper.ReadRequestAccountItem((int)accountId);

                retVal.Add(new EGRULRequestItem
                {
                    Id                  = item.ID,
                    Title               = item.Title,
                    RequestAccount      = accountStr,
                    RequestAccountId    = accountId != null ? (int)accountId : 0,
                    OrgFormCode         = accountEntity != null ? accountEntity.OrgFormCode : String.Empty,
                    HasError            = false,
                    ListName            = "IncomeRequestList"
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

            foreach (var document in documentList)
            {
                var doc = document as EGRULRequestItem;
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
                #region [Rule#2 - RequestAccount must be legal person (not private entrepreneur)]
                if (doc != null && doc.OrgFormCode == PrivateEntrepreneurCode)
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
                var documentList = (List<EGRULRequestItem>)ViewState["requestDocumentList"];
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
            var doc = document as EGRULRequestItem;
            if (doc == null)
                throw new Exception("Must be of type EGRULRequestItem");

            var requestAccount = IncomeRequestHelper.ReadRequestAccountItem(doc.RequestAccountId);
            var svcGuid = new Guid(Config.GetConfigValueOrDefault<string>(Web, EgrulServiceGuidConfigName));
            var spItem = GetList().GetItemOrBreak(doc.Id);
            var buildOptions = new QueueMessageBuildOptions {Date = DateTime.Now, Method = 2, ServiceGuid = svcGuid};
            return
                QueueMessageBuilder.Build(
                    new CoordinateV5EgrulMessageBuilder(spItem,
                        new RequestAccountData {Ogrn = requestAccount.Ogrn, Inn = requestAccount.Inn}),
                    QueueClient, buildOptions);
        }
        #endregion

    }
}

