// <copyright file="DeleteListItems.aspx.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-03-30 14:21:49Z</date>
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
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;

    /// <summary>
    /// TODO: Add comment for DeleteListItems
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class DeleteListItems : LayoutsPageBase
    {
        protected static readonly string resFilePathRelative = @"TaxoMotor\TM.SP.AppPages";
        private static readonly string resComment = "$Resources:DeleteListItems_CommentText";

        protected string GetLocalizedString(string Key)
        {
            return SPUtility.GetLocalizedString(Key, resFilePathRelative, this.Web != null ? this.Web.Language : 1033);
        }
        /// <summary>
        /// Initializes a new instance of the DeleteListItems class
        /// </summary>
        public DeleteListItems()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Defines which rights are required
        /// </summary>
        protected override SPBasePermissions RightsRequired
        {
            get
            {
                return base.RightsRequired | SPBasePermissions.ManageLists;
            }
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            SPSite siteCollection = this.Site;
            SPWeb site = this.Web;

            this.LiteralComment.Text = GetLocalizedString(resComment);
            this.BtnRun.Click += new EventHandler(this.BtnRun_Click);
            this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
        }

        protected void BtnRun_Click(object sender, EventArgs e)
        {

        }

       
    }
}

