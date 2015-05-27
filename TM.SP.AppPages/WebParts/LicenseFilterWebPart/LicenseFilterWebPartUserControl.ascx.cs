// <copyright file="LicenseFilterWebPartUserControl.ascx.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-05-27 15:33:41Z</date>
namespace TM.SP.AppPages
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using CamlexNET;
    using TM.Utils;
    using Microsoft.SharePoint.WebPartPages;
    using Microsoft.SharePoint.WebControls;
    using System.Xml;

    /// <summary>
    /// TODO: Add comment for usercontrol LicenseFilterWebPartUserControl 
    /// </summary>
    public partial class LicenseFilterWebPartUserControl : System.Web.UI.UserControl
    {
        protected static readonly string resFilePathRelative = @"TaxoMotor\TM.SP.AppPages";

        public LicenseFilterWebPart WebPart { get; set; }

        protected List<Expression<Func<SPListItem, bool>>> Parameters
        {
            get
            {
                var list = new List<Expression<Func<SPListItem, bool>>>();

                if (!OutputDateParamFrom.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(OutputDateParamFrom.SelectedDate);
                    list.Add(x => x["Tm_LicenseOutputDate"] >= ((DataTypes.DateTime)d));
                }
                if (!OutputDateParamTo.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(OutputDateParamTo.SelectedDate);
                    list.Add(x => x["Tm_LicenseOutputDate"] <= ((DataTypes.DateTime)d));
                }
                if (!FromDateParamFrom.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(FromDateParamFrom.SelectedDate);
                    list.Add(x => x["Tm_LicenseFromDate"] >= ((DataTypes.DateTime)d));
                }
                if (!FromDateParamTo.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(FromDateParamTo.SelectedDate);
                    list.Add(x => x["Tm_LicenseFromDate"] <= ((DataTypes.DateTime)d));
                }

                return list;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitResourceValues();

            var web = SPContext.Current.Web;
            var list = web.GetListOrBreak("Lists/LicenseList");
            var viewName = WebPart.View;

            if (Parameters.Count > 0)
            {
                lvLicenses.WebId = web.ID;
                lvLicenses.ListId = list.ID;
                lvLicenses.ListName = list.ID.ToString("B").ToUpper();

                SPView view = null;
                if (!String.IsNullOrEmpty(viewName))
                {
                    view = list.Views.Cast<SPView>().Where(x => x.Title == viewName).FirstOrDefault();
                }
                if (view == null)
                {
                    view = list.DefaultView;
                }

                lvLicenses.ViewGuid = view.ID.ToString("B").ToUpper();
                lvLicenses.DataBinding += lvLicenses_DataBinding;
            }
            else
            {
                DataPanel.Controls.Remove(lvLicenses);
            }
        }

        void lvLicenses_DataBinding(object sender, EventArgs e)
        {
            XsltListViewWebPart wp = sender as XsltListViewWebPart;
            if (wp != null)
            {
                wp.DisableColumnFiltering = true;
                var ds = wp.DataSource as SPDataSource;
                if (ds != null && Parameters.Count > 0)
                {
                    XmlDocument viewXml = new XmlDocument();
                    viewXml.LoadXml(ds.SelectCommand);
                    var query = viewXml.DocumentElement.SelectSingleNode("Query");

                    if (query != null)
                    {
                        var caml = Camlex.Query().WhereAll(Parameters).ToString();
                        query.InnerXml = caml;
                    }

                    ds.SelectCommand = viewXml.OuterXml;
                }
            }
        }

        public WebPartContextualInfo WebPartContextualInfo
        {
            get
            {
                EnsureChildControls();
                return lvLicenses.WebPartContextualInfo;
            }
        }

        private void InitResourceValues()
        {
            btnFind.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_BtnFind", resFilePathRelative, SPContext.Current.Web.Language);
            OutputDateText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_OutputDateText", resFilePathRelative, SPContext.Current.Web.Language);
            OutputDateDescriptionText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_OutputDateDescrText", resFilePathRelative, SPContext.Current.Web.Language);
            OutputDateParamFromText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_DateFrom", resFilePathRelative, SPContext.Current.Web.Language);
            OutputDateParamToText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_DateTo", resFilePathRelative, SPContext.Current.Web.Language);
            FromDateParamFromText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_DateFrom", resFilePathRelative, SPContext.Current.Web.Language);
            FromDateParamToText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_DateTo", resFilePathRelative, SPContext.Current.Web.Language);
            FromDateText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_FromDateText", resFilePathRelative, SPContext.Current.Web.Language);
            FromDateDescriptionText.Text = SPUtility.GetLocalizedString("$Resources:LicenseFilterPage_FromDateDescrText", resFilePathRelative, SPContext.Current.Web.Language);
        }
    }
}

