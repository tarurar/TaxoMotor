using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net.Security;

namespace TM.Services.CoordinateV5
{
    [XmlSerializerFormat]
    [ServiceContract(Namespace = Namespace.ServiceNamespace, ProtectionLevel = ProtectionLevel.Sign)]
    public interface ICoordinateV5Service
    {
        [OperationContract(Action = Namespace.ServiceNamespace + "GetRequestList",
            ReplyAction = Namespace.ServiceNamespace + "GetRequestListResponse")]
        GetRequestListOutMessage GetRequestList(GetRequestListInMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "GetRequests",
        ReplyAction = Namespace.ServiceNamespace + "GetRequestsResponse")]
        GetRequestsOutMessage GetRequests(GetRequestsInMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "SendRequest",
            /*ReplyAction = Namespace.ServiceNamespace + "SendRequestResponse",*/ IsOneWay = true)]
        void SendRequest(CoordinateMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "SendTask",
            /*ReplyAction = Namespace.ServiceNamespace + "SendTaskResponse",*/ IsOneWay = true)]
        void SendTask(CoordinateTaskMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "SetFilesAndStatus",
            /*ReplyAction = Namespace.ServiceNamespace + "SetFilesAndStatusResponse",*/ IsOneWay = true)]
        void SetFilesAndStatus(CoordinateStatusMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "Acknowledgement",
            /*ReplyAction = Namespace.ServiceNamespace + "AcknowledgementResponse",*/ IsOneWay = true)]
        void Acknowledgement(ErrorMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "SendRequests",
            /*ReplyAction = Namespace.ServiceNamespace + "SendRequestResponse",*/ IsOneWay = true)]
        void SendRequests(SendRequestsMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "SendTasks",
            /*ReplyAction = Namespace.ServiceNamespace + "SendTaskResponse",*/ IsOneWay = true)]
        void SendTasks(SendTasksMessage request);

        [OperationContract(Action = Namespace.ServiceNamespace + "SetFilesAndStatuses",
            /*ReplyAction = Namespace.ServiceNamespace + "SetFilesAndStatusResponse",*/ IsOneWay = true)]
        void SetFilesAndStatuses(SetFilesAndStatusesMessage request);
    }
}
