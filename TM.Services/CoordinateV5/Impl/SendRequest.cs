using System;
using System.IO;
using System.Linq;
using System.Data.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TM.DatabaseModel;

namespace TM.Services.CoordinateV5
{
    public static partial class ServiceImplementation
    {
        public static void SendRequest(CoordinateMessage request)
        {
            TM_DatabaseDataContext ctx = new TM_DatabaseDataContext(DatabaseFactory.CreateConnection());
            XmlSerializer sXML = new XmlSerializer(typeof(CoordinateMessage));
            StringWriter str = new StringWriter();
            sXML.Serialize(XmlWriter.Create(str), request);

            IncomingRequestXML newRequest = new IncomingRequestXML()
            {
                InDate = DateTime.Now,
                RequestBody = new XElement(str.ToString()),
                Source = "CoordinateV5Service"
            };
            ctx.IncomingRequestXMLs.InsertOnSubmit(newRequest);
            ctx.SubmitChanges();
        }
    }

}