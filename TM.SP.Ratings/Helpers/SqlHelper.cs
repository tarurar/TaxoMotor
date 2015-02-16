using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using TM.Utils;
using System.IO;
using System.Reflection;

namespace TM.SP.Ratings.Helpers
{
    public static class SqlHelper
    {
        #region [methods]
        public static string GetConnectionString(SPWeb web)
        {
            // getting db
            var dbHost = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBHost")).ToString();
            var dbName = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBName")).ToString();
            // getting credentials
            var credentialsAppId = Config.GetConfigValue(Config.GetConfigItem(web, "RatingsSSOAppId")).ToString();

            var cBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = dbHost,
                InitialCatalog = dbName
            };

            Utility.WithSPServiceContext(web, serviceContextWeb =>
            {
                cBuilder.UserID = Security.GetSecureStoreUserNameCredential(credentialsAppId);
                cBuilder.Password = Security.GetSecureStorePasswordCredential(credentialsAppId);
            });

            return cBuilder.ConnectionString;
        }
        public static string LoadSQLStatement(string statementName)
        {
            string sqlStatement = string.Empty;

            string namespacePart = "TM.SP.Ratings.SQL";
            string resourceName = namespacePart + "." + statementName;

            using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stm != null)
                {
                    sqlStatement = new StreamReader(stm).ReadToEnd();
                }
            }

            return sqlStatement;
        }
        public static int GetReportIdByGuid(Guid guid, SqlConnection conn)
        {
            string sqlText = LoadSQLStatement("ReportIdByGuid.sql");
            using (SqlCommand cmd = new SqlCommand(sqlText, conn))
            {
                cmd.Parameters.AddWithValue("@Guid", guid);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public static int NewReportSession(int reportId, SqlConnection conn)
        {
            string sqlText = LoadSQLStatement("NewReportSession.sql");
            using (SqlCommand cmd = new SqlCommand(sqlText, conn))
            {
                cmd.Parameters.AddWithValue("@ReportId", reportId);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public static DataTable GetLatestReportData(int reportId, int itemsToDisplay, string order, SqlConnection conn)
        {
            string sqlText = LoadSQLStatement("GetLatestReportData.sql");
            using (SqlCommand cmd = new SqlCommand(sqlText, conn))
            {
                cmd.Parameters.AddWithValue("@ReportId", reportId);
                cmd.Parameters.AddWithValue("@ItemsToDisplay", itemsToDisplay);
                cmd.Parameters.AddWithValue("@Order", order);
                SqlDataReader dr = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(dr);
                dr.Close();

                return dt;
            }
        }
        #endregion
    }
}
