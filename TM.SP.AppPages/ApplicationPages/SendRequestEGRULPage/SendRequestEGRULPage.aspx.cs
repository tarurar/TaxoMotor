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

    /// <summary>
    /// TODO: Add comment for SendRequestEGRULPage
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestEGRULPage : DialogLayoutsPageBase
    {
        protected static readonly string resRequestListHeader       = "$Resources:EGRULRequest_DlgRequestListHeaderText";
        protected static readonly string resPrecautionMessage       = "$Resources:EGRULRequest_DlgPrecautionMessageText";
        protected static readonly string resNoRequestMessage        = "$Resources:EGRULRequest_DlgNoRequestMessageText";
        protected static readonly string resRequestListTableHeader1 = "$Resources:EGRULRequest_DlgRequestListHeader1";
        protected static readonly string resRequestListTableHeader2 = "$Resources:EGRULRequest_DlgRequestListHeader2";

        /// <summary>
        /// Initializes a new instance of the SendRequestEGRULPage class
        /// </summary>
        public SendRequestEGRULPage()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        private string BuildCAMLInValuesClause(string values, string valueType)
        {
            string[] list = values.Split(',');
            if (list.Length == 0) return String.Empty;

            string retVal = String.Empty;
            foreach (string value in list)
            {
                retVal += @"<Value Type='" + valueType + "'>" + value + @"</Value>";
            }
            return @"<Values>" + retVal + @"</Values>";
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            SPSite siteCollection = this.Site;
            SPWeb site = this.Web;

            //var listId = Page.Request.Params["ListId"].ToString();

            BdcService svc = SPFarm.Local.Services.GetValue<BdcService>();
            if (svc == null) throw new Exception("No BDC Service Application found");
            DatabaseBackedMetadataCatalog catalog = svc.GetDatabaseBackedMetadataCatalog(SPServiceContext.GetContext(siteCollection));
            IEntity ect = catalog.GetEntity("TM.SP.BCSModels.CoordinateV5", "RequestAccountData");
            ILobSystem lob = ect.GetLobSystem();
            ILobSystemInstance lobi = lob.GetLobSystemInstances()["CoordinateV5"];
            IMethodInstance mi = ect.GetMethodInstance("GetItemsByIdListInstance", MethodInstanceType.SpecificFinder);
            IParameterCollection parameters = mi.GetMethod().GetParameters();
            object[] arguments = new object[parameters.Count];

            arguments[0] = Page.Request.Params["Items"].ToString();



            TM.SP.BCSModels.CoordinateV5.RequestAccountData requestAD = (TM.SP.BCSModels.CoordinateV5.RequestAccountData)ect.Execute(mi, lobi, ref arguments);
            TestLabel.Text = "request message id=" + requestAD.Title;
            
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
            this.EndOperation(0);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.EndOperation();
        }
    }
}

