using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;

namespace TM.SP.Ratings.Timers
{
    public class RatingCarrierActingLicences : RatingBaseJobDefinition
    {
        #region [interfaces]

        public override string GetName()
        {
            return "TaxoMotorRatingCarrierActingLicenses";
        }

        public override string GetTitle()
        {
            return GetFeatureLocalizedResource("CarrierActingLicensesJobTitle");
        }

        public override Guid GetGuid()
        {
            return new Guid("{C42AFE57-BB02-4E61-9C37-E72AF82A7592}");
        }

        #endregion
        public RatingCarrierActingLicences() : base() { }
        public RatingCarrierActingLicences(string jobName, string rsJobTitle, SPService service)
            : base(jobName, rsJobTitle, service) { }

        public RatingCarrierActingLicences(string jobName, string rsJobTitle, SPWebApplication webapp)
            : base(jobName, rsJobTitle, webapp) { }

        protected override DataTable WebExecuteJob(SPWeb web)
        {
            DataTable data = null;

            using (var conn = new SqlConnection(GetConnectionString(web)))
            {
                conn.Open();

                SqlCommand select = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT 
	                                [OrgName] as Indicator, 
	                                COUNT(*) as IntValue1 
                                FROM [dbo].[LicenseActing] 
                                GROUP BY [OrgName]"
                };
                
                using (SqlDataReader dr = select.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    data = new DataTable();
                    data.Load(dr);
                    dr.Close();
                }
            }

            return data;
        }
    }
}
