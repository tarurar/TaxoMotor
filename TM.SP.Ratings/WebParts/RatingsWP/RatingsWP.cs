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

        private UserControl userControl;

        public enum Quality
        {
            Top = 0,
            Bottom
        }

        public enum Rating
        {
            // Действующие разрешения
            [StringValue("{C42AFE57-BB02-4E61-9C37-E72AF82A7592}")]
            ActiveLicensesCount = 0
        }

        public RatingsWP()
        {
        }

        protected override void CreateChildControls()
        {
            userControl = (UserControl)this.Page.LoadControl(ASCXPATH);

            Controls.Add(userControl);

            base.CreateChildControls();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        public static int ItemCount;
        [WebBrowsable(true)]
        [WebDisplayName("Количество элементов для отображения (от 1 до 10)")]
        [WebDescription("Количество элементов рейтинга с утетом очередности отбора")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("Рейтинг")]
        public int _ItemCount 
        {
            get { return ItemCount; }
            set { ItemCount = value; } 
        }

        public static Quality QualityDropDown;
        [WebBrowsable(true)]
        [WebDisplayName("Очередность отбора")]
        [WebDescription("Выборка лучших или худших показателей")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("Рейтинг")]
        public Quality _QualityDropDown
        {
            get { return QualityDropDown; }
            set { QualityDropDown = value; }
        }

        public static Rating RatingDropDown;
        [WebBrowsable(true)]
        [WebDisplayName("Шаблон рейтинга")]
        [WebDescription("Выбор из списка доступных рейтингов")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("Рейтинг")]
        public Rating _RatingDropDown
        {
            get { return RatingDropDown; }
            set { RatingDropDown = value; }
        }

        public static string Header;
        [WebBrowsable(true)]
        [WebDisplayName("Заголовок")]
        [WebDescription("Название рейтинга, отображаемое в заголовке веб части")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("Рейтинг")]
        public string _Header
        {
            get { return Header; }
            set { Header = value; }
        }
    }
}

