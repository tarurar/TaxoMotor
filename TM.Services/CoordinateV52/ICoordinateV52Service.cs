using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TM.Services.CoordinateV52
{
    [XmlSerializerFormat]
    [ServiceContract(Namespace = Namespace.ServiceNamespace, ProtectionLevel = ProtectionLevel.Sign)]
    public interface ICoordinateV52Service
    {
        [System.ServiceModel.OperationContractAttribute(ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "GetRequestList", ReplyAction = Namespace.ServiceNamespace + "GetRequestListResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        TM.Services.CoordinateV52.GetRequestListOutMessage GetRequestList(TM.Services.CoordinateV52.GetRequestListInMessage request);

        [System.ServiceModel.OperationContractAttribute(ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "GetRequests", ReplyAction = Namespace.ServiceNamespace + "GetRequestsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        TM.Services.CoordinateV52.GetRequestsOutMessage GetRequests(TM.Services.CoordinateV52.GetRequestsInMessage request);

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "SendRequest")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendRequest(TM.Services.CoordinateV52.CoordinateMessage request);

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "SendTask")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendTask(TM.Services.CoordinateV52.CoordinateTaskMessage request);

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "SetFilesAndStatus")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SetFilesAndStatus(TM.Services.CoordinateV52.CoordinateStatusMessage request);

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "Acknowledgement")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void Acknowledgement(TM.Services.CoordinateV52.ErrorMessage request);

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "SendRequests")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendRequests(TM.Services.CoordinateV52.SendRequestsMessage request);

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "SendTasks")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendTasks(TM.Services.CoordinateV52.SendTasksMessage request);

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, ProtectionLevel = System.Net.Security.ProtectionLevel.Sign, Action = Namespace.ServiceNamespace + "SetFilesAndStatuses")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SetFilesAndStatuses(TM.Services.CoordinateV52.SetFilesAndStatusesMessage request);
    }
}
