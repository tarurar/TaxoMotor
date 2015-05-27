// <copyright file="IncomRequestFilterWebPartUserControl.ascx.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-05-26 13:00:56Z</date>
namespace TM.SP.AppPages
{
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.WebPartPages;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Xml.Linq;
    using System.Linq.Expressions;
    using TM.Utils;
    using CamlexNET;
    using System.Xml;

    /// <summary>
    /// TODO: Add comment for usercontrol IncomRequestFilterWebPartUserControl 
    /// </summary>
    public partial class IncomRequestFilterWebPartUserControl : System.Web.UI.UserControl
    {
        protected static readonly string resFilePathRelative = @"TaxoMotor\TM.SP.AppPages";

        public IncomRequestFilterWebPart WebPart { get; set; }

        protected List<Expression<Func<SPListItem, bool>>> Parameters
        {
            get
            {
                var list = new List<Expression<Func<SPListItem, bool>>>();

                if (!InputDateParamFrom.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(InputDateParamFrom.SelectedDate);
                    list.Add(x => x["Tm_RegistrationDate"] >= ((DataTypes.DateTime)d));
                }
                if (!InputDateParamTo.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(InputDateParamTo.SelectedDate);
                    list.Add(x => x["Tm_RegistrationDate"] <= ((DataTypes.DateTime)d));
                }
                if (!PrepareFactDateParamFrom.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(PrepareFactDateParamFrom.SelectedDate);
                    list.Add(x => x["Tm_PrepareFactDate"] >= ((DataTypes.DateTime)d));
                }
                if (!PrepareFactDateParamTo.IsDateEmpty)
                {
                    var d = SPUtility.CreateISO8601DateTimeFromSystemDateTime(PrepareFactDateParamTo.SelectedDate);
                    list.Add(x => x["Tm_PrepareFactDate"] <= ((DataTypes.DateTime)d));
                }

                return list;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitResourceValues();

            var web = SPContext.Current.Web;
            var list = web.GetListOrBreak("Lists/IncomeRequestList");
            var viewName = WebPart.View;

            if (Parameters.Count > 0)
            {
                lvRequests.WebId = web.ID;
                lvRequests.ListId = list.ID;
                lvRequests.ListName = list.ID.ToString("B").ToUpper();

                SPView view = null;
                if (!String.IsNullOrEmpty(viewName))
                {
                    view = list.Views.Cast<SPView>().Where(x => x.Title == viewName).FirstOrDefault();
                }
                if (view == null)
                {
                    view = list.DefaultView;
                }

                lvRequests.ViewGuid = view.ID.ToString("B").ToUpper();
                lvRequests.DataBinding += lvRequests_DataBinding;
            }
            else
            {
                DataPanel.Controls.Remove(lvRequests);
            }
        }

        void lvRequests_DataBinding(object sender, EventArgs e)
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
                return lvRequests.WebPartContextualInfo;
            }
        }

        private void InitResourceValues()
        {
            btnFind.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_BtnFind", resFilePathRelative, SPContext.Current.Web.Language);
            InputDateText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_InputDateText", resFilePathRelative, SPContext.Current.Web.Language);
            InputDateDescriptionText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_InputDateDescrText", resFilePathRelative, SPContext.Current.Web.Language);
            InputDateParamFromText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_DateFrom", resFilePathRelative, SPContext.Current.Web.Language);
            InputDateParamToText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_DateTo", resFilePathRelative, SPContext.Current.Web.Language);
            PrepareFactDateParamFromText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_DateFrom", resFilePathRelative, SPContext.Current.Web.Language);
            PrepareFactDateParamToText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_DateTo", resFilePathRelative, SPContext.Current.Web.Language);
            PrepareFactDateText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_PrepareFactDateText", resFilePathRelative, SPContext.Current.Web.Language);
            PrepareFactDateDescriptionText.Text = SPUtility.GetLocalizedString("$Resources:IncomeRequestFilterPage_PrepareFactDateDescrText", resFilePathRelative, SPContext.Current.Web.Language);
        }
    }

}

