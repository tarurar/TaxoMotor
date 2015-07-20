using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace TM.SP.BCSModels.CoordinateV5
{
    public partial class RequestEntityService
    {
        public MigratingRequest TakeItemForMigration(int ItemId)
        {
            MigratingRequest retVal = null;

            SqlConnection thisConn = getSqlConnection();
            thisConn.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.Connection = thisConn;

            const string selectAnyText = @"INSERT INTO [dbo].[RequestMigrationTicket] ([MessageId], [Status], [StartDate], [RequestId])
                                        OUTPUT INSERTED.[Id], INSERTED.[Status], INSERTED.[RequestId]
                                        SELECT TOP 1 r.[MessageId], @Status, GETDATE(), r.[Id] FROM [dbo].[Request] r
                                        WITH (XLOCK, ROWLOCK)
                                        LEFT JOIN [dbo].[RequestMigrationTicket] m on m.[RequestId] = r.[Id]
                                        WHERE m.[Title] IS NULL";

            const string selectItemText = @"INSERT INTO [dbo].[RequestMigrationTicket] ([MessageId], [Status], [StartDate], [RequestId])
                                        OUTPUT INSERTED.[Id], INSERTED.[Status], INSERTED.[RequestId]
                                        SELECT r.[MessageId], @Status, GETDATE(), r.[Id] FROM [dbo].[Request] r
                                        WITH (XLOCK, ROWLOCK)
                                        LEFT JOIN [dbo].[RequestMigrationTicket] m on m.[RequestId] = r.[Id]
                                        WHERE m.[Title] IS NULL AND r.[Id] = @ItemId";

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
            SqlDataReader thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                retVal = new MigratingRequest()
                {
                    TicketId = (System.Int32)thisReader["Id"],
                    Status = thisReader["Status"] == DBNull.Value ? (System.Int32)MigratingStatus.Undefined : (System.Int32)thisReader["Status"],
                    ItemId = (System.Int32)thisReader["RequestId"],
                    ErrorInfo = String.Empty,
                    StackInfo = String.Empty
                };
            }

            return retVal;
        }

        public void FinishMigration(MigratingRequest migratingRequest)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                SqlCommand updateCommand = new SqlCommand() { Connection = thisConn };
                updateCommand.CommandText = @"UPDATE [dbo].[RequestMigrationTicket]
                                            SET [Status] = @Status, [FinishDate] = GETDATE(), [ErrorInfo] = @ErrorInfo, [StackTrace] = @StackTrace
                                            WHERE [Id] = @Id";

                var errorInfo = String.IsNullOrEmpty(migratingRequest.ErrorInfo) ? (object)DBNull.Value : migratingRequest.ErrorInfo;
                var stackTrace = String.IsNullOrEmpty(migratingRequest.StackInfo) ? (object)DBNull.Value : migratingRequest.StackInfo;

                updateCommand.Parameters.AddWithValue("@Status", migratingRequest.Status);
                updateCommand.Parameters.AddWithValue("@ErrorInfo", errorInfo);
                updateCommand.Parameters.AddWithValue("@StackTrace", stackTrace);
                updateCommand.Parameters.AddWithValue("@Id", migratingRequest.TicketId);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }

        public void UpdateMigrationStatus(MigratingRequest request)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                SqlCommand updateCommand = new SqlCommand() { Connection = thisConn };
                updateCommand.CommandText = @"UPDATE [dbo].[RequestMigrationTicket]
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
