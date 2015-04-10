using System;

namespace TM.Services.CoordinateV5
{
    public static partial class ServiceImplementation
    {
        public static void Acknowledgement(ErrorMessage request)
        {
            var client = new CoordinateV5Client("InputQueue");
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