using System;

namespace TM.Services.CoordinateV5
{
    public static partial class ServiceImplementation
    {
        public static void Acknowledgement(ErrorMessage request)
        {
            var queueUrl = Properties.Settings.Default.InputMessageQueueUrl;

            var binding   = new System.ServiceModel.BasicHttpBinding();
            var address   = new System.ServiceModel.EndpointAddress(queueUrl);
            var client = new CoordinateV5Client(binding, address);
            client.Acknowledgement(request.ServiceHeader, request.Error);
        }
    }
}