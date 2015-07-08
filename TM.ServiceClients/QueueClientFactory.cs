namespace TM.ServiceClients
{
    public sealed class QueueClientFactory
    {
        public static MessageQueue.IDataService GetInstance(string endpoint)
        {
            var binding = new System.ServiceModel.BasicHttpBinding();
            var address = new System.ServiceModel.EndpointAddress(endpoint);

            return new MessageQueue.DataServiceClient(binding, address);
        }
    }
}
