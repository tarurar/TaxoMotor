using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using TM.SP.BCSModels.CoordinateV5;

namespace TM.SP.BCSModels.CoordinateV5
{
    public partial class RequestAccountDataService : CoordinateV5Service
    {
        public IList<RequestAccountData> GetItemsByIdList(string RequestIdList)
        {
            SqlConnection thisConn = null;
            List<RequestAccountData> allEntities = new List<RequestAccountData>();
            if (String.IsNullOrEmpty(RequestIdList))
                return allEntities;

            thisConn = getSqlConnection();
            thisConn.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.Connection = thisConn;
            selectCommand.CommandText = @"SELECT R.[ID]
                                              ,R.[TITLE]
                                              ,R.[DECLARANTREQUESTACCOUNT]
	                                          ,RA.[FULLNAME]
	                                          ,RA.[OGRN]
                                          FROM [DBO].[REQUEST] R
                                          LEFT JOIN [DBO].[REQUESTACCOUNT] RA ON R.[DECLARANTREQUESTACCOUNT] = RA.[ID]
                                          WHERE R.[ID] IN (" + RequestIdList + @")";
            SqlDataReader thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while (thisReader.Read())
            {
                RequestAccountData entity = new RequestAccountData();

                entity.Id = (System.Int32)thisReader["Id"];
                entity.Title = (thisReader["Title"] == DBNull.Value) ? null : thisReader["Title"].ToString();
                entity.DeclarantAccountFullName = (thisReader["FullName"] == DBNull.Value) ? null : thisReader["FullName"].ToString();
                entity.Ogrn = (thisReader["Ogrn"] == DBNull.Value) ? null : thisReader["Ogrn"].ToString();
                entity.DeclarantAccountId = thisReader["DeclarantRequestAccount"] as System.Nullable<System.Int32>;

                allEntities.Add(entity);
            }
            thisReader.Close();
            return allEntities;
        }
    }
}
