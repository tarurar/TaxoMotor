using System;
using System.ServiceModel.Description;

namespace WebClientGIBDD
{
    public class SoapServiceBehavior : IEndpointBehavior
    {
        private EventHandler<WriteLogData> WriteLogAction;
        public SoapServiceBehavior(EventHandler<WriteLogData> action)
        {
            WriteLogAction = action;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            var insp = new LogMessageInspector();
            insp.WriteLogEvent += WriteLogAction;
            clientRuntime.MessageInspectors.Add(insp);
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint){}

    }
}