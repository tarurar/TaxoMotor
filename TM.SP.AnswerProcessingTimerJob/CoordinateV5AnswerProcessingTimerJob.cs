using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using CamlexNET;

using TM.Utils;
using TM.ServiceClients;
using TM.Services.CoordinateV5;
using MessageQueueService = TM.ServiceClients.MessageQueue;
using CoordinateV5File = TM.Services.CoordinateV5.File;

namespace TM.SP.AnswerProcessingTimerJob
{
    public class CoordinateV5AnswerProcessingTimerJob : SPJobDefinition
    {
        #region [resource strings]

        private static readonly string FeatureId = "{6f650624-0887-40ef-bc4b-e8e2318fb867}";

        #endregion

        #region [methods]
        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }
        public CoordinateV5AnswerProcessingTimerJob() : base() {}

        public CoordinateV5AnswerProcessingTimerJob(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public CoordinateV5AnswerProcessingTimerJob(string jobName, SPWebApplication webapp): base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public override void Execute(Guid targetInstanceId)
        {
            try 
	        {
                SPWebApplication webApp = this.Parent as SPWebApplication;
                foreach (SPSite siteCollection in webApp.Sites)
                {
                    SPWeb web = siteCollection.RootWeb;
                    SPListItemCollection requestList = GetOutcomeRequests(web, 5);
                    foreach (SPListItem request in requestList)
                    {
                        CoordinateV5File[] answer = GetAnswerForOutcomeRequest(web, request);
                        if (answer.Count() > 0)
                            UpdateOutcomeRequestWithAnswer(request, answer);
                    }
                }
	        }
	        catch (Exception ex)
	        {
                throw new Exception(String.Format(GetFeatureLocalizedResource("AnswerProcessGeneralErrorFmt"), ex.Message));
	        }
        }

        private void UpdateOutcomeRequestWithAnswer(SPListItem request, CoordinateV5File[] answer)
        {
            foreach (CoordinateV5File file in answer)
            {
                request.Attachments.Add(file.FileName, file.FileContent);
            }
            request["Tm_AnswerReceived"] = true;
            request.Update();
        }

        protected virtual MessageQueueService.DataServiceClient GetServiceClientInstance(SPWeb web)
        {
            SPListItem confItem = Config.GetConfigItem(web, "MessageQueueServiceUrl");
            var binding = new System.ServiceModel.BasicHttpBinding();
            var address = new System.ServiceModel.EndpointAddress(Config.GetConfigValue(confItem).ToString());

            return new MessageQueueService.DataServiceClient(binding, address);
        }

        private CoordinateV5File[] GetAnswerForOutcomeRequest(SPWeb web, SPListItem request)
        {
            var svcClient = GetServiceClientInstance(web);
            List<CoordinateV5File> retVal = new List<CoordinateV5File>();
            MessageQueueService.Message[] messageList = 
                svcClient.GetMessageList(new Guid(request["MessageId"].ToString()), 1);

            foreach (MessageQueueService.Message message in messageList)
            {
                var serializer = new XmlSerializer(typeof(CoordinateStatusMessage));
                CoordinateStatusMessage csMessage = null;
                using (TextReader reader = new StringReader(message.MessageText))
                {
                    csMessage = (CoordinateStatusMessage)serializer.Deserialize(reader);
                }

                var fileList = csMessage.StatusMessage.Documents.SelectMany(x => x.DocFiles);
                retVal.AddRange(fileList);
            }

            return retVal.ToArray();
            
        }
        /// <summary>
        /// Getting not answered outcome requests
        /// </summary>
        /// <param name="web"></param>
        /// <param name="count">Number of returning items, 0 - no limits</param>
        /// <returns></returns>
        private SPListItemCollection GetOutcomeRequests(SPWeb web, byte count)
        {
            SPList list = web.GetListOrBreak("Lists/OutcomeRequestStateList");

            SPListItemCollection items = list.GetItems(new SPQuery()
            {
                Query = Camlex.Query().Where(x => (bool)x["Tm_AnswerReceived"] == false).ToString(),
                RowLimit = count
            });

            return items;
        }

        #endregion
    }
}
