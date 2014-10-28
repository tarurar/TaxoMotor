using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace TM.SP.BCSModels.Taxi
{
    public partial class LicenseEntityService
    {
        public MigratingLicense TakeItemForMigration(int ItemId)
        {
            MigratingLicense retVal = null;

            var thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand {Connection = thisConn};

            const string selectAnyText = @"INSERT INTO [dbo].[LicenseMigrationTicket] ([Status], [StartDate], [LicenseId])
                                            OUTPUT INSERTED.[Id], INSERTED.[Status], INSERTED.[LicenseId]
                                            SELECT TOP 1 @Status, GETDATE(), l.[Id] FROM [dbo].[License] l
                                            LEFT JOIN [dbo].[LicenseMigrationTicket] m on m.[LicenseId] = l.[Id]
                                            WHERE m.[Title] IS NULL
                                            ORDER BY l.[Parent] ASC"; // we need an order instruction to be sure in the parent existance
                
            const string selectItemText = @"INSERT INTO [dbo].[LicenseMigrationTicket] ([Status], [StartDate], [LicenseId])
                                            OUTPUT INSERTED.[Id], INSERTED.[Status], INSERTED.[LicenseId]
                                            SELECT @Status, GETDATE(), l.[Id] FROM [dbo].[License] l
                                            LEFT JOIN [dbo].[LicenseMigrationTicket] m on m.[LicenseId] = l.[Id]
                                            WHERE m.[Title] IS NULL AND l.[Id] = @ItemId
                                            ORDER BY l.[Parent] ASC";

            if (ItemId == 0)
            {
                selectCommand.CommandText = selectAnyText;
            }
            else
            {
                selectCommand.CommandText = selectItemText;
                selectCommand.Parameters.AddWithValue("@ItemId", ItemId);
            }
            selectCommand.Parameters.AddWithValue("@Status", (Int32)MigratingStatus.Reserved);
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                retVal = new MigratingLicense()
                {
                    TicketId = (System.Int32)thisReader["Id"],
                    Status = thisReader["Status"] == DBNull.Value ? (System.Int32)MigratingStatus.Undefined : (System.Int32)thisReader["Status"],
                    ItemId = (System.Int32)thisReader["LicenseId"],
                    ErrorInfo = String.Empty,
                    StackInfo = String.Empty
                };
            }

            return retVal;
        }

        public void FinishMigration(MigratingLicense migratingLicense)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                SqlCommand updateCommand = new SqlCommand() { Connection = thisConn };
                updateCommand.CommandText = @"UPDATE [dbo].[LicenseMigrationTicket]
                                            SET [Status] = @Status, [FinishDate] = GETDATE(), [ErrorInfo] = @ErrorInfo, [StackTrace] = @StackTrace
                                            WHERE [Id] = @Id";

                var errorInfo = String.IsNullOrEmpty(migratingLicense.ErrorInfo) ? (object)DBNull.Value : migratingLicense.ErrorInfo;
                var stackTrace = String.IsNullOrEmpty(migratingLicense.StackInfo) ? (object)DBNull.Value : migratingLicense.StackInfo;

                updateCommand.Parameters.AddWithValue("@Status", migratingLicense.Status);
                updateCommand.Parameters.AddWithValue("@ErrorInfo", errorInfo);
                updateCommand.Parameters.AddWithValue("@StackTrace", stackTrace);
                updateCommand.Parameters.AddWithValue("@Id", migratingLicense.TicketId);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }

        public void UpdateMigrationStatus(MigratingLicense request)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                SqlCommand updateCommand = new SqlCommand() { Connection = thisConn };
                updateCommand.CommandText = @"UPDATE [dbo].[LicenseMigrationTicket]
                                            SET [Status] = @Status
                                            WHERE [Id] = @Id";
                updateCommand.Parameters.AddWithValue("@Status", request.Status);
                updateCommand.Parameters.AddWithValue("@Id", request.TicketId);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }
    }
}
