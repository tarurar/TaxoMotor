// <copyright file="LicenseFilterWebPart.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-05-27 15:33:37Z</date>
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
    /// TODO: Add comment for webpart LicenseFilterWebPart
    /// </summary>
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class LicenseFilterWebPart : WebPart, IWebPartPageComponentProvider
    {
        private const string ASCXPATH = @"/_CONTROLTEMPLATES/15/TaxoMotor/LicenseFilterWebPartUserControl.ascx";

        private LicenseFilterWebPartUserControl userControl;

        public LicenseFilterWebPart()
        {
        }

        protected override void CreateChildControls()
        {
            userControl = this.Page.LoadControl(ASCXPATH) as LicenseFilterWebPartUserControl;
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
                return (userControl as LicenseFilterWebPartUserControl).WebPartContextualInfo;
            }
        }

        [WebBrowsable(true)]
        [WebDisplayName("ѕредставление списка")]
        [WebDescription("Ќаименование представлени€, которое будет использовано дл€ отображени€ результатов")]
        [Personalizable(PersonalizationScope.Shared)]
        public string View { get; set; }
    }
}

