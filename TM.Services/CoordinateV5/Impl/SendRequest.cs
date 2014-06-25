using System;
using System.Linq;
using System.Data.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml.Linq;
using TM.DatabaseModel;

namespace TM.Services.CoordinateV5
{
    public static partial class ServiceImplementation
    {
        public static void SendRequest(CoordinateMessage request)
        {
            try
            {
                TM_DatabaseDataContext ctx = new TM_DatabaseDataContext(DatabaseFactory.CreateConnection());
                IncomingRequestXML newRequest = new IncomingRequestXML()
                {
                    InDate = DateTime.Now,
                    RequestBody = new XElement(request.ToString()),
                    Source = "CoordinateV5Service"
                };
                ctx.IncomingRequestXMLs.InsertOnSubmit(newRequest);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while trying to save data. ExDetails: " + ex.Message);
            }
        }
    }

}