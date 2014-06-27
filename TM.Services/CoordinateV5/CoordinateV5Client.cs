namespace TM.Services.CoordinateV5
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICoordinateV5ServiceChannel : ICoordinateV5Service, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CoordinateV5Client : System.ServiceModel.ClientBase<ICoordinateV5Service>, ICoordinateV5Service
    {

        public CoordinateV5Client()
        {
        }

        public CoordinateV5Client(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public CoordinateV5Client(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CoordinateV5Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CoordinateV5Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public GetRequestListOutMessage GetRequestList(GetRequestListInMessage request)
        {
            return base.Channel.GetRequestList(request);
        }

        public GetRequestsOutMessage GetRequests(GetRequestsInMessage request)
        {
            return base.Channel.GetRequests(request);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ICoordinateV5Service.SendRequest(CoordinateMessage request)
        {
            base.Channel.SendRequest(request);
        }

        public void SendRequest(Headers ServiceHeader, CoordinateData Message)
        {
            CoordinateMessage inValue = new CoordinateMessage();
            inValue.ServiceHeader = ServiceHeader;
            inValue.Message = Message;
            ((ICoordinateV5Service)(this)).SendRequest(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ICoordinateV5Service.SendTask(CoordinateTaskMessage request)
        {
            base.Channel.SendTask(request);
        }

        public void SendTask(Headers ServiceHeader, CoordinateTaskData TaskMessage)
        {
            CoordinateTaskMessage inValue = new CoordinateTaskMessage();
            inValue.ServiceHeader = ServiceHeader;
            inValue.TaskMessage = TaskMessage;
            ((ICoordinateV5Service)(this)).SendTask(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ICoordinateV5Service.SetFilesAndStatus(CoordinateStatusMessage request)
        {
            base.Channel.SetFilesAndStatus(request);
        }

        public void SetFilesAndStatus(Headers ServiceHeader, CoordinateStatusData StatusMessage)
        {
            CoordinateStatusMessage inValue = new CoordinateStatusMessage();
            inValue.ServiceHeader = ServiceHeader;
            inValue.StatusMessage = StatusMessage;
            ((ICoordinateV5Service)(this)).SetFilesAndStatus(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ICoordinateV5Service.Acknowledgement(ErrorMessage request)
        {
            base.Channel.Acknowledgement(request);
        }

        public void Acknowledgement(Headers ServiceHeader, ErrorMessageData Error)
        {
            ErrorMessage inValue = new ErrorMessage();
            inValue.ServiceHeader = ServiceHeader;
            inValue.Error = Error;
            ((ICoordinateV5Service)(this)).Acknowledgement(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ICoordinateV5Service.SendRequests(SendRequestsMessage request)
        {
            base.Channel.SendRequests(request);
        }

        public void SendRequests(Headers ServiceHeader, CoordinateData[] RequestsMessage)
        {
            SendRequestsMessage inValue = new SendRequestsMessage();
            inValue.ServiceHeader = ServiceHeader;
            inValue.RequestsMessage = RequestsMessage;
            ((ICoordinateV5Service)(this)).SendRequests(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ICoordinateV5Service.SendTasks(SendTasksMessage request)
        {
            base.Channel.SendTasks(request);
        }

        public void SendTasks(Headers ServiceHeader, CoordinateTaskData[] TasksMessage)
        {
            SendTasksMessage inValue = new SendTasksMessage();
            inValue.ServiceHeader = ServiceHeader;
            inValue.TasksMessage = TasksMessage;
            ((ICoordinateV5Service)(this)).SendTasks(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ICoordinateV5Service.SetFilesAndStatuses(SetFilesAndStatusesMessage request)
        {
            base.Channel.SetFilesAndStatuses(request);
        }

        public void SetFilesAndStatuses(Headers ServiceHeader, SetFilesAndStatusesData[] StatusesMessage)
        {
            SetFilesAndStatusesMessage inValue = new SetFilesAndStatusesMessage();
            inValue.ServiceHeader = ServiceHeader;
            inValue.StatusesMessage = StatusesMessage;
            ((ICoordinateV5Service)(this)).SetFilesAndStatuses(inValue);
        }
    }
}
