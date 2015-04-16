using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using TM.DatabaseModel;
using TM.Utils;

namespace TM.Services.CoordinateV52
{
    public static partial class ServiceImplementation
    {
        public static void SendRequest(CoordinateMessage request)
        {
            using (TM_DatabaseDataContext ctx = new TM_DatabaseDataContext(DatabaseFactory.CreateConnection()))
            {
                ctx.CommandTimeout = DatabaseFactory.GetCommandTimeout();

                IncomingRequestXML newRequest = new IncomingRequestXML()
                {
                    InDate = DateTime.Now,
                    RequestBody = request.ToXElement<CoordinateMessage>(),
                    Source = "CoordinateV52Service"
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
                        var worker = new AcknowledgementWorker(request.ServiceHeader.Invert(), "ASGUFV52");
                        new Thread(worker.Work) { IsBackground = false }.Start();
                    }
                }
            }
        }
    }
}