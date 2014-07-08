using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Data.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text;
using TM.DatabaseModel;
using TM.Utils;

namespace TM.Services.CoordinateV5
{
    public static partial class ServiceImplementation
    {
        public static void SendRequest(CoordinateMessage request)
        {
            TM_DatabaseDataContext ctx = new TM_DatabaseDataContext(DatabaseFactory.CreateConnection());
            IncomingRequestXML newRequest = new IncomingRequestXML()
            {
                InDate      = DateTime.Now,
                RequestBody = request.ToXElement<CoordinateMessage>(),
                Source      = "CoordinateV5Service"
            };
            ctx.IncomingRequestXMLs.InsertOnSubmit(newRequest);

            var sendAck = false;
            try
            {
                ctx.SubmitChanges();
                sendAck = true;
            }
            catch (SqlException ex)
            {
                if (newRequest.hasDuplicateMessageError(ex))
                {
                    sendAck = true;
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (sendAck)
                {
                    var worker = new AcknowledgementWorker(request.ServiceHeader.Invert(), "ASGUF");
                    new Thread(worker.Work) {IsBackground = false}.Start();
                }
            }
        }
    }

}