using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace TM.SP.BCSModels.CoordinateV5
{
    public partial class RequestAccountEntityService
    {
        public RequestAccount GetLatest()
        {
            SqlConnection thisConn = null;
            RequestAccount entity = null;

            entity = new RequestAccount();
            thisConn = getSqlConnection();
            thisConn.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "SELECT TOP 1 [Title] , [FullName] , [Name] , [BrandName] , [Ogrn] , [OgrnAuthority] , [OgrnNum] , [OgrnDate] , [Inn] , [InnAuthority] , [InnNum] , [InnDate] , [Kpp] , [Okpo] , [OrgFormCode] , [Okved] , [Okfs] , [BankName] , [BankBik] , [CorrAccount] , [SetAccount] , [Phone] , [Fax] , [EMail] , [WebSite] , [Id] , [MessageId] , [PostalAddress] , [FactAddress] , [RequestContact], [SingleStrPostalAddress], [SingleStrFactAddress] FROM [dbo].[RequestAccount] ORDER BY [Id] DESC";

            selectCommand.Connection = thisConn;
            SqlDataReader thisReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (thisReader.Read())
            {
                entity.Title = (thisReader["Title"] == DBNull.Value) ? null : thisReader["Title"].ToString();
                entity.FullName = (thisReader["FullName"] == DBNull.Value) ? null : thisReader["FullName"].ToString();
                entity.Name = (thisReader["Name"] == DBNull.Value) ? null : thisReader["Name"].ToString();
                entity.BrandName = (thisReader["BrandName"] == DBNull.Value) ? null : thisReader["BrandName"].ToString();
                entity.Ogrn = (thisReader["Ogrn"] == DBNull.Value) ? null : thisReader["Ogrn"].ToString();
                entity.OgrnAuthority = (thisReader["OgrnAuthority"] == DBNull.Value) ? null : thisReader["OgrnAuthority"].ToString();
                entity.OgrnNum = (thisReader["OgrnNum"] == DBNull.Value) ? null : thisReader["OgrnNum"].ToString();
                entity.OgrnDate = thisReader["OgrnDate"] as System.Nullable<System.DateTime>;
                entity.Inn = (thisReader["Inn"] == DBNull.Value) ? null : thisReader["Inn"].ToString();
                entity.InnAuthority = (thisReader["InnAuthority"] == DBNull.Value) ? null : thisReader["InnAuthority"].ToString();
                entity.InnNum = (thisReader["InnNum"] == DBNull.Value) ? null : thisReader["InnNum"].ToString();
                entity.InnDate = thisReader["InnDate"] as System.Nullable<System.DateTime>;
                entity.Kpp = (thisReader["Kpp"] == DBNull.Value) ? null : thisReader["Kpp"].ToString();
                entity.Okpo = (thisReader["Okpo"] == DBNull.Value) ? null : thisReader["Okpo"].ToString();
                entity.OrgFormCode = (thisReader["OrgFormCode"] == DBNull.Value) ? null : thisReader["OrgFormCode"].ToString();
                entity.Okved = (thisReader["Okved"] == DBNull.Value) ? null : thisReader["Okved"].ToString();
                entity.Okfs = (thisReader["Okfs"] == DBNull.Value) ? null : thisReader["Okfs"].ToString();
                entity.BankName = (thisReader["BankName"] == DBNull.Value) ? null : thisReader["BankName"].ToString();
                entity.BankBik = (thisReader["BankBik"] == DBNull.Value) ? null : thisReader["BankBik"].ToString();
                entity.CorrAccount = (thisReader["CorrAccount"] == DBNull.Value) ? null : thisReader["CorrAccount"].ToString();
                entity.SetAccount = (thisReader["SetAccount"] == DBNull.Value) ? null : thisReader["SetAccount"].ToString();
                entity.Phone = (thisReader["Phone"] == DBNull.Value) ? null : thisReader["Phone"].ToString();
                entity.Fax = (thisReader["Fax"] == DBNull.Value) ? null : thisReader["Fax"].ToString();
                entity.EMail = (thisReader["EMail"] == DBNull.Value) ? null : thisReader["EMail"].ToString();
                entity.WebSite = (thisReader["WebSite"] == DBNull.Value) ? null : thisReader["WebSite"].ToString();
                entity.Id = (System.Int32)thisReader["Id"];
                entity.MessageId = (System.String)thisReader["MessageId"];
                entity.PostalAddress = thisReader["PostalAddress"] as System.Nullable<System.Int32>;
                entity.FactAddress = thisReader["FactAddress"] as System.Nullable<System.Int32>;
                entity.RequestContact = thisReader["RequestContact"] as System.Nullable<System.Int32>;
                entity.SingleStrPostalAddress = (thisReader["SingleStrPostalAddress"] == DBNull.Value) ? null : thisReader["SingleStrPostalAddress"].ToString();
                entity.SingleStrFactAddress = (thisReader["SingleStrFactAddress"] == DBNull.Value) ? null : thisReader["SingleStrFactAddress"].ToString();
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
