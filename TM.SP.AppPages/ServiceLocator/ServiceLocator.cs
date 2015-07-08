using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using TM.ServiceClients;
using TM.SP.AppPages.Communication;
using TM.Utils;

// ReSharper disable once CheckNamespace
namespace TM.SP.AppPages
{
    internal class ServiceLocator: IServiceLocator
    {
        private static IServiceLocator _instance;
        private static readonly object Lock = new Object();
        private readonly IDictionary<object, object> _services;

        private ServiceLocator()
        {
            _services = new Dictionary<object, object>();
            BuildServiceMap();
        }

        private void BuildServiceMap()
        {
            var ctx = SPContext.Current;

            _services.Add(typeof (IQueueMessageBuilder), new Lazy<IQueueMessageBuilder>(() => new QueueMessageBuilder()));

            // SharePoint context dependent services
            if (ctx != null)
            {
                var endpoint = Config.GetConfigValueOrDefault<string>(ctx.Web, "MessageQueueServiceUrl");

                _services.Add(typeof (ServiceClients.MessageQueue.IDataService),
                    new Lazy<ServiceClients.MessageQueue.IDataService>(
                        () => QueueClientFactory.GetInstance(endpoint)));
            }
        }

        public static IServiceLocator Instance
        {
            get
            {
                lock (Lock)
                {
                    return _instance ?? (_instance = new ServiceLocator());
                }
            }
        }
        public T GetService<T>()
        {
            try
            {
                return ((Lazy<T>)_services[typeof (T)]).Value;
            }
            catch (KeyNotFoundException)
            {
                throw new Exception("The requested service is not registered");
            }
        }
    }
}
