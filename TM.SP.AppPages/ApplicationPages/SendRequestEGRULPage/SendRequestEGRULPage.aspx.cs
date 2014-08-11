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

    using TM.SP.AppPages.ApplicationPages;
    using TM.SP.BCSModels.CoordinateV5;
    using TM.Utils;

    /// <summary>
    /// TODO: Add comment for SendRequestEGRULPage
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestEGRULPage : DialogLayoutsPageBase
    {
        protected static readonly string resRequestListHeader = "$Resources:EGRULRequest_DlgRequestListHeaderText";
        protected static readonly string resPrecautionMessage = "$Resources:EGRULRequest_DlgPrecautionMessageText";
        protected static readonly string resNoRequestMessage = "$Resources:EGRULRequest_DlgNoRequestMessageText";
        protected static readonly string resRequestListTableHeader1 = "$Resources:EGRULRequest_DlgRequestListHeader1";
        protected static readonly string resRequestListTableHeader2 = "$Resources:EGRULRequest_DlgRequestListHeader2";
        protected static readonly string EGRULDataEntityName = "RequestAccountData";
        protected static readonly string EGRULDataFetchMethodName = "GetItemsByIdListInstance";

        /// <summary>
        /// Initializes a new instance of the SendRequestEGRULPage class
        /// </summary>
        public SendRequestEGRULPage()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            //TODO: add sharepoint grid view to display "request before" data
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            SPSite siteCollection = this.Site;
            SPWeb site = this.Web;

            // load initial data from external system
            var arguments = new object[] { Page.Request.Params["Items"].ToString() };
            IEntity externalCT = BCS.GetEntity(
                SPServiceContext.GetContext(siteCollection), 
                String.Empty, 
                BCS.LOBRequestSystemNamespace, 
                SendRequestEGRULPage.EGRULDataEntityName);
            IList<RequestAccountData> showData = (IList<RequestAccountData>)BCS.GetDataFromMethod(
                BCS.LOBRequestSystemName,
                externalCT, 
                SendRequestEGRULPage.EGRULDataFetchMethodName, 
                MethodInstanceType.Finder, 
                ref arguments);

            //TODO: bind showData to the grid view on the page
            //TODO: show/hide the message ab items which won't be included into the request
            //DataTable table = showData.ToDataTable<RequestAccountData>();

            this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
            this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
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

