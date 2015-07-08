using System;
using Microsoft.CSharp.RuntimeBinder;
using TM.ServiceClients.MessageQueue;
using TM.Utils;

namespace TM.SP.AppPages.Communication
{
    public class QueueMessageBuilder: IQueueMessageBuilder
    {
        public Message Build<T>(ICoordinateMessageBuilder<T> internalMessageBuilder, IDataService queueClient,
            QueueMessageBuildOptions options)
        {
            dynamic internalMessage = internalMessageBuilder.Build();

            string requestId;
            try
            {
                requestId = internalMessage.TaskMessage.Task.RequestId;
            }
            catch (RuntimeBinderException)
            {
                requestId = null;
            }
            
            return new Message
            {
                Service       = queueClient.GetService(options.ServiceGuid),
                MessageId     = new Guid(internalMessage.ServiceHeader.MessageId),
                MessageType   = 2,
                MessageMethod = options.Method,
                MessageDate   = options.Date,
                MessageText   = Extensions.ToXElement<T>(internalMessage).ToString(),
                RequestId     = requestId == null ? new Guid() : new Guid(requestId)
            };
        }
    }
}
