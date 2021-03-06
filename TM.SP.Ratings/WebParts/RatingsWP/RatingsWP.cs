// <copyright file="RatingsWP.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\Developer</author>
// <date>2015-01-29 18:16:20Z</date>
namespace TM.SP.Ratings
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
    using System.ComponentModel;
    using TM.Utils;
    using WebPart = System.Web.UI.WebControls.WebParts.WebPart;
    
    /// <summary>
    /// TODO: Add comment for webpart RatingsWP
    /// </summary>
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class RatingsWP : WebPart
    {
        private const string ASCXPATH = @"/_CONTROLTEMPLATES/15/TaxoMotor/RatingsWPUserControl.ascx";

        private RatingsWPUserControl userControl;

        public enum Quality
        {
            Top = 0,
            Bottom
        }

        public enum Rating
        {
            // ����������� ����������
            [StringValue("{C42AFE57-BB02-4E61-9C37-E72AF82A7592}")]
            ActiveLicensesCount = 0
        }

        public RatingsWP()
        {
        }

        protected override void CreateChildControls()
        {
            userControl = (RatingsWPUserControl)this.Page.LoadControl(ASCXPATH);
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

        [WebBrowsable(true)]
        [WebDisplayName("���������� ��������� ��� ����������� (�� 1 �� 10)")]
        [WebDescription("���������� ��������� �������� � ������ ����������� ������")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("�������")]
        [DefaultValue(5)]
        public int _ItemCount { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("����������� ������")]
        [WebDescription("������� ������ ��� ������ �����������")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("�������")]
        public Quality _QualityDropDown { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("������ ��������")]
        [WebDescription("����� �� ������ ��������� ���������")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("�������")]
        public Rating _RatingDropDown { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("���������")]
        [WebDescription("�������� ��������, ������������ � ��������� ��� �����")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("�������")]
        public string _Header { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("Url ���������")]
        [WebDescription("������, �� ������� ����� ��������� ��������� ��� �����")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("�������")]
        public string _TitleUrl { get; set; }
    }
}

