// <copyright file="SendRequestEGRULPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-08-06 17:56:53Z</date>
namespace TM.SP.AppPages
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Data;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.BusinessData.SharedService;
    using Microsoft.SharePoint.BusinessData.MetadataModel;
    using Microsoft.BusinessData.Runtime;
    using Microsoft.BusinessData.MetadataModel;
    using Microsoft.BusinessData.MetadataModel.Collections;
    using Microsoft.CSharp.RuntimeBinder;

    using TM.SP.AppPages.ApplicationPages;
    using TM.SP.BCSModels.CoordinateV5;
    using TM.Utils;


    internal class EGRULRequestItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string RequestAccount { get; set; }
        public bool HasError { get; set; }
    }

    /// <summary>
    /// TODO: Add comment for SendRequestEGRULPage
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestEGRULPage : SendRequestDialogBase
    {
        protected static readonly string resRequestListCaption = "$Resources:EGRULRequest_DlgRequestListCaption";
        protected static readonly string resPrecautionMessage = "$Resources:EGRULRequest_DlgPrecautionMessageText";
        protected static readonly string resNoRequestMessage = "$Resources:EGRULRequest_DlgNoRequestMessageText";
        protected static readonly string resRequestListTableHeader1 = "$Resources:EGRULRequest_DlgRequestListHeader1";
        protected static readonly string resRequestListTableHeader2 = "$Resources:EGRULRequest_DlgRequestListHeader2";
        protected static readonly string resOkButton = "$Resources:EGRULRequest_DlgOkButtonText";
        protected static readonly string resCancelButton = "$Resources:EGRULRequest_DlgCancelButtonText";
        protected static readonly string resNoAccountErrorFmt = "$Resources:EGRULRequest_DlgNoAccountErrorFmt";
        protected static readonly string resErrorListCaption = "$Resources:EGRULRequest_DlgErrorListCaption";
        protected static readonly string resErrorListHeader1 = "$Resources:EGRULRequest_DlgErrorListHeader1";
        protected static readonly string resNoDocumentsError = "$Resources:EGRULRequest_DlgNoDocumentsError";

        protected static readonly string EGRULDataEntityName = "RequestAccountData";
        protected static readonly string EGRULDataFetchMethodName = "GetItemsByIdListInstance";

        private SPGridView requestListGrid;
        private SPGridView errorListGrid;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            // document list grid
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
                DataField = "RequestAccount"
            });
            requestListGrid.RowDataBound += requestListGrid_RowDataBound;
            RequestListTablePanel.Controls.Add(requestListGrid);
            // error list grid
            errorListGrid = new SPGridView() { AutoGenerateColumns = false };
            errorListGrid.Columns.Add(new SPBoundField()
            {
                HeaderText = GetLocalizedString(resErrorListHeader1),
                DataField = "Message",
            });
            ErrorListTablePanel.Controls.Add(errorListGrid);

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

            this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
            this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
        }

        protected override List<object> LoadDocuments()
        {
            SPList docList = GetList();
            var idList = ItemIdListParam.Split(',');

            return (from item in docList.Items.OfType<SPListItem>()
                   let account = item["Tm_RequestAccountBCSLookup"] != null ? item["Tm_RequestAccountBCSLookup"].ToString() : null
                   where idList.Contains<String>(item.ID.ToString())
                   select new EGRULRequestItem()
                   {
                       Id = item.ID,
                       Title = item.Title,
                       RequestAccount = account,
                       HasError = false
                   }).ToList<object>();
        }

        protected override void BindDocuments(List<object> documentList)
        {
            requestListGrid.DataSource = documentList;
            requestListGrid.DataBind();
        }

        protected override List<ValidationErrorInfo> ValidateDocuments(List<object> documentList)
        {
            var retVal = new List<ValidationErrorInfo>();

            foreach (EGRULRequestItem document in documentList)
            {
                if (document.RequestAccount == null)
                {
                    retVal.Add(new ValidationErrorInfo()
                    {
                        Message = String.Format(GetLocalizedString(resNoAccountErrorFmt), document.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    document.HasError = true;
                }
            }

            if (documentList.Cast<EGRULRequestItem>().All(i => i.HasError))
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

        protected override void HandleDocumentsLoad(List<object> documentList, List<ValidationErrorInfo> errorList)
        {
            // UI
            RequestList.Visible = !(documentList.Cast<EGRULRequestItem>().All(i => i.HasError));
            ErrorList.Visible = errorList.Count() > 0;
            BtnOk.Enabled = !errorList.Any<ValidationErrorInfo>(err => err.Severity == ValidationErrorSeverity.Critical);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //TODO: endoperation(-1) ?
            this.EndOperation(0);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            //TODO: make a requests for EGRUL
            this.EndOperation();
        }
    }
}

