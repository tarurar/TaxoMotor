using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TM.Services.CoordinateV52
{
    public static partial class ServiceImplementation
    {
        public static void Acknowledgement(ErrorMessage request)
        {
            var client = new CoordinateV52Client("InputQueueV52");
            try
            {
                client.Acknowledgement(request.ServiceHeader, request.Error);
            }
            finally
            {
                client.Close();
            }
        }
    }
}