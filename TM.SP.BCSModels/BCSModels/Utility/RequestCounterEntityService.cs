using System;
using System.Data;
using System.Data.SqlClient;

// ReSharper disable CheckNamespace
namespace TM.SP.BCSModels.Utility
// ReSharper restore CheckNamespace
{
    public partial class RequestCounterEntityService
    {
        public int GetNextNumber(string serviceCode)
        {
            var retVal = 0;
            var thisConn = GetSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                Connection = thisConn,
                CommandText = @"DECLARE @year INT = @yearParam;
                                DECLARE @sc NVARCHAR(10) = @serviceCodeParam;

                                MERGE [dbo].[RequestCounter] AS target
                                USING (
	                                SELECT @year, @sc
	                                ) AS source([Year], [ServiceCode])
	                                ON (
			                                target.[Year] = source.[Year]
			                                AND target.[ServiceCode] = source.[ServiceCode]
			                                )
                                WHEN MATCHED
	                                THEN
		                                UPDATE
		                                SET [CounterValue] = [CounterValue] + 1
                                WHEN NOT MATCHED
	                                THEN
		                                INSERT (
			                                [Year]
			                                ,[ServiceCode]
			                                ,[CounterValue]
			                                )
		                                VALUES (
			                                source.[Year]
			                                ,source.[ServiceCode]
			                                ,1
			                                )
                                OUTPUT inserted.[CounterValue];"
            };

            selectCommand.Parameters.AddWithValue("@yearParam", DateTime.Now.Year);
            selectCommand.Parameters.AddWithValue("@serviceCodeParam", serviceCode);
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                retVal = (Int32)thisReader["CounterValue"];
            }

            return retVal;
        }
    }
}
