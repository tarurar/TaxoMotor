// <copyright file="UtilityEntityServices.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-10-28 12:11:33Z</date>
// <auto-generated>
//   Generated with SharePoint Software Factory 4.1
// </auto-generated>
// ReSharper disable CheckNamespace
namespace TM.SP.BCSModels.Utility
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.BusinessData.SystemSpecific;
    using Utils;

    // Base class to share connection string retrieval for all entities
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public class UtilityService : IContextProperty
    {
        public Microsoft.BusinessData.Runtime.IExecutionContext ExecutionContext
        {
            get;
            set;
        }

        public Microsoft.BusinessData.MetadataModel.ILobSystemInstance LobSystemInstance
        {
            get;
            set;
        }

        public Microsoft.BusinessData.MetadataModel.IMethodInstance MethodInstance
        {
            get;
            set;
        }

        protected SqlConnection GetSqlConnection()
        {
            var secureStoreAppId = BCS.GetLobSystemProperty(LobSystemInstance, "SecureStoreAppId");

            var cBuilder = new SqlConnectionStringBuilder
            {
                DataSource     = BCS.GetLobSystemProperty(LobSystemInstance, "DBServerName"),
                InitialCatalog = BCS.GetLobSystemProperty(LobSystemInstance, "DBName"),
                UserID         = Security.GetSecureStoreUserNameCredential(secureStoreAppId),
                Password       = Security.GetSecureStorePasswordCredential(secureStoreAppId)
            };

            return new SqlConnection(cBuilder.ConnectionString);
        }
    }
    /// <summary>
    /// All the methods for retrieving, updating and deleting data are implemented in this class file.
    /// The samples below show the finder and specific finder method for Entity1.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SPSF", "4.1")]
    public partial class RequestCounterEntityService : UtilityService
    {
        public RequestCounter ReadRequestCounterItem(Int32 id)
        {
            var entity = new RequestCounter();
            var thisConn = GetSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                CommandText =
                    "SELECT [Id] , [Title] , [Year] , [ServiceCode] , [CounterValue] FROM [dbo].[RequestCounter] WHERE [Id] = @Id"
            };
            selectCommand.Parameters.AddWithValue("@Id", id);

            selectCommand.Connection = thisConn;
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                entity.Id           = (Int32)thisReader["Id"];
                entity.Title        = (Int32)thisReader["Title"];
                entity.Year         = (Int32)thisReader["Year"];
                entity.ServiceCode  = (String)thisReader["ServiceCode"];
                entity.CounterValue = (Int32)thisReader["CounterValue"];
            }
            else
            {
                throw new Exception("Data not found");
            }
            thisReader.Close();
            return (entity);
        }

        public IList<RequestCounter> ReadRequestCounterList()
        {
            var allEntities = new List<RequestCounter>();

            var thisConn = GetSqlConnection();
            thisConn.Open();
            var selectCommand = new SqlCommand
            {
                Connection = thisConn,
                CommandText =
                    "SELECT [Id] , [Title] , [Year] , [ServiceCode] , [CounterValue] FROM [dbo].[RequestCounter]"
            };
            var thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while (thisReader.Read())
            {
                var entity = new RequestCounter
                {
                    Id           = (Int32) thisReader["Id"],
                    Title        = (Int32) thisReader["Title"],
                    Year         = (Int32) thisReader["Year"],
                    ServiceCode  = (String) thisReader["ServiceCode"],
                    CounterValue = (Int32) thisReader["CounterValue"]
                };

                allEntities.Add(entity);
            }
            thisReader.Close();
            return allEntities;
        }

        public RequestCounter CreateRequestCounter(RequestCounter newentity)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = GetSqlConnection();
                thisConn.Open();

                var createCommand = new SqlCommand
                {
                    Connection = thisConn,
                    CommandText =
                        "INSERT INTO [dbo].[RequestCounter] ([Year] , [ServiceCode] , [CounterValue]) VALUES (@Year , @ServiceCode , @CounterValue) SELECT [Id] , [Title] , [Year] , [ServiceCode] , [CounterValue] FROM [dbo].[RequestCounter] WHERE [Id] = SCOPE_IDENTITY()"
                };
                createCommand.Parameters.AddWithValue("@Id", newentity.Id);
                createCommand.Parameters.AddWithValue("@Year", newentity.Year);
                createCommand.Parameters.AddWithValue("@ServiceCode", newentity.ServiceCode);
                createCommand.Parameters.AddWithValue("@CounterValue", newentity.CounterValue);


                var thisReader = createCommand.ExecuteReader(CommandBehavior.CloseConnection);
                RequestCounter createdEntity;
                if (thisReader.Read())
                {
                    createdEntity = new RequestCounter
                    {
                        Id           = (Int32) thisReader["Id"],
                        Title        = (Int32) thisReader["Title"],
                        Year         = (Int32) thisReader["Year"],
                        ServiceCode  = (String) thisReader["ServiceCode"],
                        CounterValue = (Int32) thisReader["CounterValue"]
                    };
                }
                else
                {
                    throw new Exception("Data not found");
                }
                return createdEntity;
            }
            finally
            {
                if (thisConn != null) thisConn.Dispose();
            }
        }

        public void DeleteRequestCounter(Int32 id)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = GetSqlConnection();
                thisConn.Open();

                var deleteCommand = new SqlCommand
                {
                    Connection = thisConn,
                    CommandText = "DELETE FROM [dbo].[RequestCounter] WHERE [Id] = @Id"
                };
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();
            }
            finally
            {
                if (thisConn != null) thisConn.Dispose();
            }

        }


        public void UpdateRequestCounter(RequestCounter updateRequestCounter)
        {
            SqlConnection thisConn = null;
            try
            {
                thisConn = GetSqlConnection();
                thisConn.Open();

                var updateCommand = new SqlCommand
                {
                    Connection = thisConn,
                    CommandText =
                        "UPDATE [dbo].[RequestCounter] SET [Year] = @Year , [ServiceCode] = @ServiceCode , [CounterValue] = @CounterValue WHERE [Id] = @Id"
                };

                //add new field values
                updateCommand.Parameters.AddWithValue("@Year", updateRequestCounter.Year);
                updateCommand.Parameters.AddWithValue("@ServiceCode", updateRequestCounter.ServiceCode);
                updateCommand.Parameters.AddWithValue("@CounterValue", updateRequestCounter.CounterValue);

                updateCommand.Parameters.AddWithValue("@Id", updateRequestCounter.Id);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                if (thisConn != null) thisConn.Dispose();
            }
        }
    }
}

