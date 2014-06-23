using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TM.Services.CoordinateV5
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICoordinateV5Service" in both code and config file together.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://asguf.mos.ru/rkis_gu/coordinate/v5/", ConfigurationName = "ICoordinateV5Service")]
    public interface ICoordinateV5Service
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/GetRequestList", ReplyAction = "http://asguf.mos.ru/rkis_gu/coordinate/v5/GetRequestListResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        GetRequestListOutMessage GetRequestList(GetRequestListInMessage request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/GetRequests", ReplyAction = "http://asguf.mos.ru/rkis_gu/coordinate/v5/GetRequestsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        GetRequestsOutMessage GetRequests(GetRequestsInMessage request);

        // CODEGEN: Контракт генерации сообщений с операцией SendRequest не является ни RPC, ни упакованным документом.
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/SendRequest")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendRequest(CoordinateMessage request);

        // CODEGEN: Контракт генерации сообщений с операцией SendTask не является ни RPC, ни упакованным документом.
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/SendTask")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendTask(CoordinateTaskMessage request);

        // CODEGEN: Контракт генерации сообщений с операцией SetFilesAndStatus не является ни RPC, ни упакованным документом.
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/SetFilesAndStatus")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SetFilesAndStatus(CoordinateStatusMessage request);

        // CODEGEN: Контракт генерации сообщений с именем упаковщика (ErrorMessage) сообщения ErrorMessage не соответствует значению по умолчанию (Acknowledgement).
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/Acknowledgement")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void Acknowledgement(ErrorMessage request);

        // CODEGEN: Контракт генерации сообщений с операцией SendRequests не является ни RPC, ни упакованным документом.
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/SendRequests")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendRequests(SendRequestsMessage request);

        // CODEGEN: Контракт генерации сообщений с операцией SendTasks не является ни RPC, ни упакованным документом.
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/SendTasks")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SendTasks(SendTasksMessage request);

        // CODEGEN: Контракт генерации сообщений с операцией SetFilesAndStatuses не является ни RPC, ни упакованным документом.
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://asguf.mos.ru/rkis_gu/coordinate/v5/SetFilesAndStatuses")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBase))]
        void SetFilesAndStatuses(SetFilesAndStatusesMessage request);
    }
}
