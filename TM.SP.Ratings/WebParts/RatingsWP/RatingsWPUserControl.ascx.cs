// <copyright file="RatingsWPUserControl.ascx.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\Developer</author>
// <date>2015-01-29 18:16:23Z</date>
namespace TM.SP.Ratings
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using TM.SP.Ratings.Timers;
    using TM.SP.Ratings.Cache;
    using TM.SP.Ratings.Helpers;
    using TM.Utils;
    using System.Web.UI.HtmlControls;

    public partial class RatingsWPUserControl : System.Web.UI.UserControl
    {
        #region [fields]
        private static readonly int MaxNumberOfItemsToDisplay = 10;
        private static readonly string FeatureId = "{1af64a86-6794-459c-abb7-bf484221801b}";
        #endregion

        #region [properties]
        public RatingsWP WebPart { get; set; }
        public int ItemsToDisplay
        {
            get
            {
                return WebPart._ItemCount;
            }
        }
        public RatingsWP.Quality Quality
        {
            get
            {
                return WebPart._QualityDropDown;
            }
        }
        private int? _reportId = null;
        public int? ReportId
        {
            get
            {
                if (_reportId == null)
                {
                    var connectionString = SqlHelper.GetConnectionString(SPContext.Current.Web);
                    var guidStr = StringEnum.GetStringValue(WebPart._RatingDropDown);
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        try
                        {
                            _reportId = SqlHelper.GetReportIdByGuid(new Guid(guidStr), conn);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                return _reportId;
            }
        }
        public string Header
        {
            get { return WebPart._Header; }
        }
        #endregion

        #region [methods]
        private void FillTableWithData(DataTable dt)
        {
            DataTable.Rows.Clear();
            var dtCssClass = String.Format("top-{0}", dt.Rows.Count <= MaxNumberOfItemsToDisplay ? dt.Rows.Count : MaxNumberOfItemsToDisplay);
            var tdCssClass = String.Format("top-{0}-div", dt.Rows.Count <= MaxNumberOfItemsToDisplay ? dt.Rows.Count : MaxNumberOfItemsToDisplay);

            var tr = new TableRow();
            foreach (DataRow dtRow in dt.Rows)
            {
                var cell = new TableCell();

                var div = new HtmlGenericControl("div");
                div.Attributes.Add("class", tdCssClass);
                div.InnerText = dtRow["IntValue"].ToString();

                var span = new HtmlGenericControl("span");
                var spanValue = dtRow["Indicator"].ToString();
                if (String.IsNullOrEmpty(spanValue))
                    spanValue = SPFeatureHelper.GetFeatureLocalizedResource("NoIndicatorTitlePlaceHolder", FeatureId);
                span.InnerText = spanValue; 

                cell.Controls.Add(div);
                cell.Controls.Add(span);

                tr.Cells.Add(cell);
                if (tr.Cells.Count >= MaxNumberOfItemsToDisplay) break;
            }
            
            DataTable.Rows.Add(tr);
            DataTable.CssClass = dtCssClass;
        }
        private DataTable LoadRatingData()
        {
            DataTable dt = null;
            var connectionString = SqlHelper.GetConnectionString(SPContext.Current.Web);
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    dt = SqlHelper.GetLatestReportData(
                        ReportId.Value,
                        ItemsToDisplay,
                        Quality == RatingsWP.Quality.Top ? "TOP" : "BOTTOM",
                        conn);
                }
                finally
                {
                    conn.Close();
                }
            }

            return dt;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            ErrorMessage.Text = String.Empty;

            if (!IsPostBack)
            {
                string errMsg = String.Empty;
                if (ItemsToDisplay == 0)
                    errMsg = SPFeatureHelper.GetFeatureLocalizedResource("NoItemsToDisplayParamErr", FeatureId);
                if (ReportId == null)
                    errMsg = SPFeatureHelper.GetFeatureLocalizedResource("NoRatingParamErr", FeatureId);
                if (ReportId == 0)
                    errMsg = SPFeatureHelper.GetFeatureLocalizedResource("NoRatingMaintainerErr", FeatureId);

                if (!String.IsNullOrEmpty(errMsg))
                {
                    ErrorMessage.Text = errMsg;
                    return;
                }

                var dt = LoadRatingData();
                if (dt.Rows.Count != 0) FillTableWithData(dt);
                else
                    ErrorMessage.Text = SPFeatureHelper.GetFeatureLocalizedResource("NoRatingDataErr", FeatureId);
            }
        }

        #endregion
    }
}

