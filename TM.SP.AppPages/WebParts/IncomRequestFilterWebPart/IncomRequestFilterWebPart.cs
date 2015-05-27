// <copyright file="IncomRequestFilterWebPart.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-05-26 13:00:52Z</date>
namespace TM.SP.AppPages
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;

    /// <summary>
    /// TODO: Add comment for webpart IncomRequestFilterWebPart
    /// </summary>
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class IncomRequestFilterWebPart : WebPart, IWebPartPageComponentProvider
    {
        private const string ASCXPATH = @"/_CONTROLTEMPLATES/15/TaxoMotor/IncomRequestFilterWebPartUserControl.ascx";

        private IncomRequestFilterWebPartUserControl userControl;

        public IncomRequestFilterWebPart()
        {
        }

        protected override void CreateChildControls()
        {
            userControl = this.Page.LoadControl(ASCXPATH) as IncomRequestFilterWebPartUserControl;
            if (userControl != null)
            {
                userControl.WebPart = this;
                Controls.Add(userControl);
            }

            base.CreateChildControls();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        public WebPartContextualInfo WebPartContextualInfo
        {
            get
            {
                EnsureChildControls();
                return (userControl as IncomRequestFilterWebPartUserControl).WebPartContextualInfo;
            }
        }

        [WebBrowsable(true)]
        [WebDisplayName("ѕредставление списка")]
        [WebDescription("Ќаименование представлени€, которое будет использовано дл€ отображени€ результатов")]
        [Personalizable(PersonalizationScope.Shared)]
        public string View { get; set; }
    }
}

