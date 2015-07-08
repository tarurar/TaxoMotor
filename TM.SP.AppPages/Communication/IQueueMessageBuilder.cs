using TM.ServiceClients.MessageQueue;

namespace TM.SP.AppPages.Communication
{
    public interface IQueueMessageBuilder
    {
        Message Build<T>(ICoordinateMessageBuilder<T> internalMessageBuilder, IDataService queueClient,
            QueueMessageBuildOptions options);
    }
}
