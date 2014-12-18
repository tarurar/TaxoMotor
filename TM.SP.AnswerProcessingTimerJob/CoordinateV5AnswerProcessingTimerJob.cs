using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using CamlexNET;

using TM.Utils;
using TM.Services.CoordinateV5;
using MessageQueueService = TM.ServiceClients.MessageQueue;
using CoordinateV5File = TM.Services.CoordinateV5.File;

namespace TM.SP.AnswerProcessingTimerJob
{
    public class CoordinateV5AnswerProcessingTimerJob : SPJobDefinition
    {
        #region [resource strings]

        private static readonly string FeatureId = "{6f650624-0887-40ef-bc4b-e8e2318fb867}";

        public static readonly string TaxiListsFeatureId = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        #endregion

        #region [methods]
        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }
        public CoordinateV5AnswerProcessingTimerJob()
        {}

        public CoordinateV5AnswerProcessingTimerJob(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            Title = GetFeatureLocalizedResource("JobTitle");
        }

        public CoordinateV5AnswerProcessingTimerJob(string jobName, SPWebApplication webapp): base(jobName, webapp, null, SPJobLockType.Job)
        {
            Title = GetFeatureLocalizedResource("JobTitle");
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                var webApp = Parent as SPWebApplication;
                if (webApp != null)
                {
                    foreach (SPSite siteCollection in webApp.Sites)
                    {
                        SPWeb web = siteCollection.RootWeb;

                        if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                            web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                        {

                            SPListItemCollection requestList = GetOutcomeRequests(web, 20);
                            foreach (SPListItem request in requestList)
                            {
                                request["Tm_LastProcessDate"] = DateTime.Now;
                                request.Update();

                                string customAttributes;
                                string resultCode;
                                CoordinateV5File[] files = GetAnswerForOutcomeRequest(web, request, out customAttributes, out resultCode);
                                if (files.Any())
                                {
                                    UpdateOutcomeRequestWithFiles(request, files);
                                    request["Tm_AnswerReceived"] = true;
                                    request.Update();
                                }
                                if (!String.IsNullOrEmpty(customAttributes))
                                {
                                    request["Tm_XmlValue"] = customAttributes;
                                    request["Tm_AnswerReceived"] = true;
                                    request.Update();
                                }
                                if (!String.IsNullOrEmpty(resultCode))
                                {
                                    request["Tm_ResultCode"] = resultCode;
                                    request["Tm_AnswerReceived"] = true;
                                    request.Update();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
	        {
                throw new Exception(String.Format(GetFeatureLocalizedResource("AnswerProcessGeneralErrorFmt"), ex.Message));
	        }
        }

        private void UpdateOutcomeRequestWithFiles(SPListItem request, CoordinateV5File[] answer)
        {
            foreach (CoordinateV5File file in answer)
            {
                var docNumber = request.Attachments.Count + 1;
                var fileExt = Path.GetExtension(file.FileName);
                var filename = String.Format("document{0}{1}", docNumber, fileExt);

                request.Attachments.Add(filename, file.FileContent);
            }
        }

        protected virtual MessageQueueService.DataServiceClient GetServiceClientInstance(SPWeb web)
        {
            SPListItem confItem = Config.GetConfigItem(web, "MessageQueueServiceUrl");
            var binding = new System.ServiceModel.BasicHttpBinding 
            {
                MaxReceivedMessageSize = 65536000,
                                        
            };
            binding.ReaderQuotas.MaxStringContentLength = 81920000;

            var address = new System.ServiceModel.EndpointAddress(Config.GetConfigValue(confItem).ToString());

            return new MessageQueueService.DataServiceClient(binding, address);
        }

        private CoordinateV5File[] GetAnswerForOutcomeRequest(SPWeb web, SPListItem request, out string customAttributesXml, out string resultCode)
        {
            var svcClient = GetServiceClientInstance(web);
            List<CoordinateV5File> retVal = new List<CoordinateV5File>();
            MessageQueueService.Message[] messageList =
                svcClient.GetMessageList(new Guid(request["Tm_MessageId"].ToString()), 1);
            customAttributesXml = String.Empty;
            resultCode = String.Empty;

            foreach (MessageQueueService.Message message in messageList)
            {
                var serializer = new XmlSerializer(typeof(CoordinateStatusMessage));
                CoordinateStatusMessage csMessage;
                using (TextReader reader = new StringReader(message.MessageText))
                {
                    csMessage = (CoordinateStatusMessage)serializer.Deserialize(reader);
                }

                if (csMessage.StatusMessage.Documents != null && csMessage.StatusMessage.Documents.Any())
                {
                    var fileList = csMessage.StatusMessage.Documents.SelectMany(x => x.DocFiles);
                    retVal.AddRange(fileList);

                    var firstDoc = csMessage.StatusMessage.Documents.First();
                    if (firstDoc.CustomAttributes != null)
                    {
                        customAttributesXml = firstDoc.CustomAttributes.OuterXml;
                    }
                }

                if (csMessage.StatusMessage.Result != null)
                {
                    resultCode = csMessage.StatusMessage.Result.ResultCode;
                }
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

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                x => (bool)x["Tm_AnswerReceived"] == false,
                x => (DateTime)x["Created"] > DateTime.Now.AddDays(-15)
            };

            SPListItemCollection items = list.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).OrderBy(x => x["Tm_LastProcessDate"] as Camlex.Asc).ToString(),
                ViewAttributes = "Scope='RecursiveAll'",
                RowLimit = count
            });

            return items;
        }

        #endregion
    }
}
