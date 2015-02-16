using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Utils.Sql
{
    public static class SqlHelper
    {
        public static DataTable CreateDataTableStub(string connectionString, string sqlTableNamespace, string sqlTableName)
        {
            var dt = new DataTable();
            var conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                var cmd = new SqlCommand(String.Format(StringsRes.SqlFakeSelectMetadataFmt, sqlTableNamespace, sqlTableName), conn);
                var da = new SqlDataAdapter(cmd);

                da.Fill(dt);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        public static string GetConnectionString(SPWeb web)
        {
            // getting db
            var dbHost = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBHost")).ToString();
            var dbName = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBName")).ToString();
            // getting credentials
            var credentialsAppId = Config.GetConfigValue(Config.GetConfigItem(web, "LocalDBWriterAccessSingleSignOnAppId")).ToString();

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
    }
}
