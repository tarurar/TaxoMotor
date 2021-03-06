﻿using System;
using System.Data;
using System.Data.SqlClient;
using TM.SP.BCSModels.Helpers;
using TM.SP.BCSModels.Taxi.Exceptions;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
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

            if (ItemId == 0)
            {
                selectCommand.CommandText = SqlHelper.LoadSQLStatement("License-TakeAnyItemForMigration.sql");
            }
            else
            {
                selectCommand.CommandText = SqlHelper.LoadSQLStatement("License-TakeItemForMigration.sql");
                selectCommand.Parameters.AddWithValue("@ItemId", ItemId);
            }
            selectCommand.Parameters.AddWithValue("@Status", (Int32)MigratingStatus.Reserved);
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                retVal = new MigratingLicense()
                {
                    TicketId  = (Int32)thisReader["Id"],
                    Status    = thisReader["Status"] == DBNull.Value ? (Int32)MigratingStatus.Undefined : (Int32)thisReader["Status"],
                    ItemId    = (Int32)thisReader["LicenseId"],
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

                var updateCommand = new SqlCommand
                {
                    Connection = thisConn,
                    CommandText = SqlHelper.LoadSQLStatement("License-FinishMigration.sql")
                };

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

                var updateCommand = new SqlCommand
                {
                    Connection = thisConn,
                    CommandText = SqlHelper.LoadSQLStatement("License-UpdateMigrationStatus.sql")
                };
                updateCommand.Parameters.AddWithValue("@Status", request.Status);
                updateCommand.Parameters.AddWithValue("@Id", request.TicketId);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }

        public void DeleteLicenseDraftForSPTaxiId(int spTaxiId)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = getSqlConnection();
                thisConn.Open();

                var deleteCommand = new SqlCommand 
                {
                    Connection = thisConn,
                    CommandText = SqlHelper.LoadSQLStatement("License-DeleteDraftForTaxi.sql")
                };
                deleteCommand.Parameters.AddWithValue("@Status", 4);
                deleteCommand.Parameters.AddWithValue("@TaxiId", spTaxiId);

                deleteCommand.ExecuteNonQuery();
            }
            finally
            {
                thisConn.Dispose();
            }
        }

        public License GetLicenseDraftForSPTaxiId(int spTaxiId)
        {
            var entity = new License();
            var thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                CommandText = SqlHelper.LoadSQLStatement("License-ReadDraftForTaxi.sql")
            };
            selectCommand.Parameters.AddWithValue("@TaxiId", spTaxiId);
            selectCommand.Parameters.AddWithValue("@Status", 4);

            selectCommand.Connection = thisConn;
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                SqlHelper.LicenseFillFromReader(entity, thisReader);
            }
            else
            {
                throw new DraftNotFoundException();
            }
            thisReader.Close();
            return entity;
        }

        public License GetAnyLicenseForSPTaxiId(int spTaxiId)
        {
            var entity = new License();
            var thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                CommandText = SqlHelper.LoadSQLStatement("License-ReadAnyForTaxi.sql")
            };
            selectCommand.Parameters.AddWithValue("@TaxiId", spTaxiId);

            selectCommand.Connection = thisConn;
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                SqlHelper.LicenseFillFromReader(entity, thisReader);
            }
            else
            {
                throw new Exception("Data not found");
            }
            thisReader.Close();
            return entity;
        }

        public License TakeAnyUnsignedLicense()
        {
            var entity = new License();
            var thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                CommandText = SqlHelper.LoadSQLStatement("License-TakeAnyUnsigned.sql"),
                Connection = thisConn
            };

            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                SqlHelper.LicenseFillFromReader(entity, thisReader);
            }
            else
            {
                throw new UnsignedNotFoundException();
            }
            thisReader.Close();
            return entity;
        }

        public License GetLicenseRequestToSend(int daysCycleCount)
        {
            var entity = new License();
            var thisConn = getSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                Connection = thisConn,
                CommandText = SqlHelper.LoadSQLStatement("License-GetRequestToSend.sql")
            };
            selectCommand.Parameters.AddWithValue("@DaysCycleCount", daysCycleCount);

            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                SqlHelper.LicenseFillFromReader(entity, thisReader);
            }
            else
            {
                throw new Exception("No licenses for requests to be sent on");
            }
            thisReader.Close();
            return entity;
        }

    }
}
