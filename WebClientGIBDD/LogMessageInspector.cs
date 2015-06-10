using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace WebClientGIBDD
{
    public class LogMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// Отправка сообщений в лог
        /// </summary>
        /// <param name="buffer">Буферезированное сообщениеЗапрос</param>
        /// <param name="stringBuilder">накопитель лога </param>
        private Message TraceMessage(MessageBuffer buffer, WebServiceClient.Direction direction)
        {
            Message msgCopy = buffer.CreateMessage();
            //var securityHeaderIndex = msgCopy.Headers.FindHeader("Security",
            //                                                     "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            //if (securityHeaderIndex > -1)
            //{
            //    var el = XElement.Load(msgCopy.Headers.GetReaderAtHeader(securityHeaderIndex).ReadSubtree());
            //    el.Descendants(
            //        "{http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd}Password")
            //      .First()
            //      .SetValue("******");
            //    msgCopy.Headers.Add(MessageHeader.CreateHeader("Security",
            //                                                   "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd",
            //                                                   el));
            //    msgCopy.Headers.RemoveAt(securityHeaderIndex);
            //}

            StringBuilder sb = new StringBuilder();
            using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(sb))
            {
                msgCopy.WriteMessage(xw);
                xw.Close();
            }

            if(WriteLogEvent != null)
                WriteLogEvent(this, new WriteLogData{Message = sb.ToString(), Direction= direction});

            return buffer.CreateMessage();
        }


        /// <summary>
        /// Разрешает проверку или изменение сообщения после получения ответного сообщения, но до передачи его обратно клиентскому приложению.
        /// </summary>
        /// <param name="reply">Сообщение, которое необходимо преобразовать в типы и врученное обратно клиентскому приложению.Запрос</param>
        /// <param name="correlationState">Данные состояния корреляции.Запрос</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            reply = TraceMessage(reply.CreateBufferedCopy(int.MaxValue), WebServiceClient.Direction.Receive);
        }

        /// <summary>
        /// Разрешает проверку или изменение сообщения до того, как сообщение запроса отправляется службе.
        /// </summary>
        /// <param name="request">Сообщение, которое нужно отправить службе.Запрос</param>
        /// <param name="channel">WCF канал объекта клиента.Запрос</param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request = TraceMessage(request.CreateBufferedCopy(int.MaxValue), WebServiceClient.Direction.Send);
            return null;
        }

        public event EventHandler<WriteLogData> WriteLogEvent;
    }

    public class WriteLogData : EventArgs
    {
        public string Message { get; set; }
        public WebServiceClient.Direction Direction { get; set; }
    }
}