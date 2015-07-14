using System;
using System.Linq;
using Microsoft.SharePoint;
using TM.ServiceClients;
using TM.SP.AppPages.Communication;
using TM.SP.AppPages.Tracker;
using TM.SP.BCSModels.Taxi;
using TM.Utils;
using System.Collections.Generic;

namespace TM.SP.AppPages.Strategies
{
    public class LicenseRequestAutoSendStrategy: LicenseBaseStrategy
    {
        protected SPWeb Web;
        private readonly ServiceClients.MessageQueue.IDataService _queueClient;
        private readonly IQueueMessageBuilder _queueMessageBuilder;
        private string _queueServiceGuid;
        private string _queueServiceEndpoint;

        private void ReadConfiguration(SPWeb web)
        {
            _queueServiceGuid = Config.GetConfigValueOrDefault<string>(web, "BR2ServiceGuid");
            _queueServiceEndpoint = Config.GetConfigValueOrDefault<string>(web, "MessageQueueServiceUrl");
        }

        private static IRequestAccountData GetAccountData(SPListItem license)
        {
            return new RequestAccountData
            {
                Ogrn = license.TryGetValue<string>("Tm_OrgOgrn"),
                Inn  = license.TryGetValue<string>("Tm_OrgInn")
            };
        }

        private bool DoSendMessage<T>(ICoordinateMessageBuilder<T> internalBuilder, ITrackingContext<SPListItem> trackContext, OutcomeRequest type)
        {
            var buildOptions = new QueueMessageBuildOptions
            {
                Date        = DateTime.Now,
                Method      = 2,
                ServiceGuid = new Guid(_queueServiceGuid)
            };

            var message = _queueMessageBuilder.Build(internalBuilder, _queueClient, buildOptions);
            var sent = _queueClient.AddMessage(message);
            if (sent)
            {
                var tracker = new RequestTracker(trackContext,
                    new OutcomeRequestTrackingData {Id = message.RequestId, Type = type});
                tracker.Track();
            }

            return sent;
        }

        private static void DoHandleActions(IEnumerable<Func<bool>> actions)
        {
            var allDone = actions.All(action =>
            {
                bool done;
                try
                {
                    done = action.Invoke();
                }
                catch (Exception)
                {
                    done = false;
                }

                return done;
            });

            if (!allDone)
            {
                throw new Exception("There were errors while trying to send requests");
            }
        }

        public LicenseRequestAutoSendStrategy(SPWeb web)
        {
            Web = web;
            ReadConfiguration(Web);

            _queueClient = QueueClientFactory.GetInstance(_queueServiceEndpoint);
            _queueMessageBuilder = ServiceLocator.Instance.GetService<IQueueMessageBuilder>();
        }

        public override void Handle(License element)
        {
            var spItem = LicenseHelper.GetSharePointItemFromBusinessItem(Web, element);
            var accountData = GetAccountData(spItem);
            var isJuridical = element.Lfb.Trim() != "91";

            var trackingContext = new LicenseTrackingContext(spItem);
            var egrBuilder = isJuridical
                ? new CoordinateV5EgrulMessageBuilder(spItem, accountData)
                : new CoordinateV5EgripMessageBuilder(spItem, accountData);
            DoHandleActions(new List<Func<bool>>
            {
                () => 
                    DoSendMessage(new CoordinateV5PtsMessageBuilder(spItem), trackingContext, OutcomeRequest.Pts),
                () =>
                    DoSendMessage(new CoordinateV5PenaltyMessageBuilder(spItem), trackingContext, OutcomeRequest.Penalty),
                () =>
                    DoSendMessage(egrBuilder, trackingContext, isJuridical ? OutcomeRequest.Egrul : OutcomeRequest.Egrip)
            });
        }
    }
}
