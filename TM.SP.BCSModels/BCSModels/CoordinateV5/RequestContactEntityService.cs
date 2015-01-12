using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;

namespace TM.SP.BCSModels.CoordinateV5
{
    public partial class RequestContactEntityService
    {
        public RequestContact GetLatest()
        {
            SqlConnection thisConn = null;
            RequestContact entity = null;

            entity = new RequestContact();
            thisConn = getSqlConnection();
            thisConn.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "SELECT TOP 1 [Id] , [Title] , [LastName] , [FirstName] , [MiddleName] , [Gender] , [BirthDate] , [Snils] , [Inn] , [MobilePhone] , [WorkPhone] , [HomePhone] , [EMail] , [Nation] , [Citizenship] , [CitizenshipType] , [JobTitle] , [OMSNum] , [OMSDate] , [OMSCompany] , [OMSValidityPeriod] , [IsiId] , [Id_Auto] , [MessageId] , [RegAddress] , [FactAddress] , [BirthAddress], [SingleStrRegAddress], [SingleStrFactAddress], [SingleStrBirthAddress] FROM [dbo].[RequestContact] ORDER BY [Id_Auto] DESC";

            selectCommand.Connection = thisConn;
            SqlDataReader thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                entity.Id = (thisReader["Id"] == DBNull.Value) ? null : thisReader["Id"].ToString();
                entity.Title = (System.String)thisReader["Title"];
                entity.LastName = (thisReader["LastName"] == DBNull.Value) ? null : thisReader["LastName"].ToString();
                entity.FirstName = (thisReader["FirstName"] == DBNull.Value) ? null : thisReader["FirstName"].ToString();
                entity.MiddleName = (thisReader["MiddleName"] == DBNull.Value) ? null : thisReader["MiddleName"].ToString();
                entity.Gender = (thisReader["Gender"] == DBNull.Value) ? null : thisReader["Gender"].ToString();
                entity.BirthDate = thisReader["BirthDate"] as System.Nullable<System.DateTime>;
                entity.Snils = (thisReader["Snils"] == DBNull.Value) ? null : thisReader["Snils"].ToString();
                entity.Inn = (thisReader["Inn"] == DBNull.Value) ? null : thisReader["Inn"].ToString();
                entity.MobilePhone = (thisReader["MobilePhone"] == DBNull.Value) ? null : thisReader["MobilePhone"].ToString();
                entity.WorkPhone = (thisReader["WorkPhone"] == DBNull.Value) ? null : thisReader["WorkPhone"].ToString();
                entity.HomePhone = (thisReader["HomePhone"] == DBNull.Value) ? null : thisReader["HomePhone"].ToString();
                entity.EMail = (thisReader["EMail"] == DBNull.Value) ? null : thisReader["EMail"].ToString();
                entity.Nation = (thisReader["Nation"] == DBNull.Value) ? null : thisReader["Nation"].ToString();
                entity.Citizenship = (thisReader["Citizenship"] == DBNull.Value) ? null : thisReader["Citizenship"].ToString();
                entity.CitizenshipType = (thisReader["CitizenshipType"] == DBNull.Value) ? null : thisReader["CitizenshipType"].ToString();
                entity.JobTitle = (thisReader["JobTitle"] == DBNull.Value) ? null : thisReader["JobTitle"].ToString();
                entity.OMSNum = (thisReader["OMSNum"] == DBNull.Value) ? null : thisReader["OMSNum"].ToString();
                entity.OMSDate = thisReader["OMSDate"] as System.Nullable<System.DateTime>;
                entity.OMSCompany = (thisReader["OMSCompany"] == DBNull.Value) ? null : thisReader["OMSCompany"].ToString();
                entity.OMSValidityPeriod = (thisReader["OMSValidityPeriod"] == DBNull.Value) ? null : thisReader["OMSValidityPeriod"].ToString();
                entity.IsiId = (thisReader["IsiId"] == DBNull.Value) ? null : thisReader["IsiId"].ToString();
                entity.Id_Auto = (System.Int32)thisReader["Id_Auto"];
                entity.MessageId = (System.String)thisReader["MessageId"];
                entity.RegAddress = thisReader["RegAddress"] as System.Nullable<System.Int32>;
                entity.FactAddress = thisReader["FactAddress"] as System.Nullable<System.Int32>;
                entity.BirthAddress = thisReader["BirthAddress"] as System.Nullable<System.Int32>;
                entity.SingleStrRegAddress = (thisReader["SingleStrRegAddress"] == DBNull.Value) ? null : thisReader["SingleStrRegAddress"].ToString();
                entity.SingleStrFactAddress = (thisReader["SingleStrFactAddress"] == DBNull.Value) ? null : thisReader["SingleStrFactAddress"].ToString();
                entity.SingleStrBirthAddress = (thisReader["SingleStrBirthAddress"] == DBNull.Value) ? null : thisReader["SingleStrBirthAddress"].ToString();
            }
            else
            {
                throw new Exception("Data not found");
            }
            thisReader.Close();
            return (entity);
        }
    }
}
