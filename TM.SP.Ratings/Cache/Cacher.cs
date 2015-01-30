using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Reflection;
using TM.SP.Ratings.Helpers;

namespace TM.SP.Ratings.Cache
{
    public interface ICacher
    {
        void Dump(DataTable table, Guid reportGuid, string connectionString);
    }
    public class Cacher : ICacher
    {
        #region [methods]
        private string GetDumpingColumns(DataTable table)
        {
            string columnsStr = "[ReportSessionId]";

            foreach (DataColumn column in table.Columns)
            {
                if (String.IsNullOrEmpty(columnsStr))
                {
                    columnsStr = "[" + column.ColumnName + "]";
                }
                else
                {
                    columnsStr += ", [" + column.ColumnName + "]";
                }
            }

            return columnsStr;
        }
        private string GetDumpingParams(DataTable table)
        {
            string paramsStr = "@ReportSessionId";

            foreach (DataColumn column in table.Columns)
            {
                if (String.IsNullOrEmpty(paramsStr))
                {
                    paramsStr = "@" + column.ColumnName;
                }
                else
                {
                    paramsStr += ", @" + column.ColumnName;
                }
            }

            return paramsStr;
        }
        private void DoDump(DataTable table, Guid reportGuid, SqlConnection conn)
        {
            int reportId = SqlHelper.GetReportIdByGuid(reportGuid, conn);

            if (reportId != 0)
            {
                int sessionId = SqlHelper.NewReportSession(reportId, conn);

                if (sessionId != 0)
                {
                    string columnsStr = GetDumpingColumns(table);
                    string paramsStr = GetDumpingParams(table);

                    string insertStmt = String.Format("INSERT INTO [dbo].[ReportData] ({0}) VALUES({1});", columnsStr, paramsStr);
                    using (SqlCommand cmd = new SqlCommand(insertStmt, conn))
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            cmd.Parameters.Clear();

                            foreach (DataColumn column in table.Columns)
                            {
                                var cName = column.ColumnName;
                                cmd.Parameters.AddWithValue("@" + cName, row[cName]);
                            }
                            cmd.Parameters.AddWithValue("@ReportSessionId", sessionId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        public virtual void Dump(DataTable table, Guid reportGuid, string connectionString)
        {
            if (table.Rows.Count == 0) return;

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    DoDump(table, reportGuid, conn);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion
    }
}
