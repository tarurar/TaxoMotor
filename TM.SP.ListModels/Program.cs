using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using CommandLine;
using NLog;
using SPMeta2.CSOM;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Behaviours;
using SPMeta2.Definitions;
using SPMeta2.Syntax.Default;

namespace TM.SP.ListModels
{
    class Options
    {
        [Option('s', "site", Required = true, HelpText = "URL адрес сайта")]
        public string siteUrl { get; set; }
        [Option('l', "winlogin", Required = true, HelpText = "Windows Account Login")]
        public string windowsLogin { get; set; }
        [Option('p', "winpassword", Required = true, HelpText = "Windows Account Password")]
        public string windowsPassword { get; set; }
        [Option('d', "windomain", Required = true, HelpText = "Windows Domain")]
        public string windowsDomain { get; set; }
    }
    class Program
    {
        private static readonly Logger Logger = LogManager.GetLogger("ExceptionLogger");
        private static readonly Logger cLogger = LogManager.GetLogger("ConsoleLogger");

        internal static bool CheckArguments(Options options)
        {
            bool retVal = true;

            if (String.IsNullOrEmpty(options.siteUrl))
            {
                cLogger.Info("Необходимо указать адрес сайта, параметр командной строки -s");
                retVal = false;
            }

            if (String.IsNullOrEmpty(options.windowsLogin))
            {
                cLogger.Info("Необходимо указать логин пользователя, имеющего административный доступ к сайту, параметр командной строки -l");
                retVal = false;
            }

            if (String.IsNullOrEmpty(options.windowsPassword))
            {
                cLogger.Info("Необходимо указать пароль пользователя, имеющего административный доступ к сайту, параметр командной строки -p");
                retVal = false;
            }

            if (String.IsNullOrEmpty(options.windowsDomain))
            {
                cLogger.Info("Необходимо указать домен учетной записи пользователя, параметр командной строки -d");
                retVal = false;
            }

            return retVal;
        }

        static void Deploy(Options options)
        {
            using (var ctx = new ClientContext(options.siteUrl))
            {
                ctx.ExecutingWebRequest +=
                    (sender, e) => e.WebRequestExecutor.RequestHeaders.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                ctx.Credentials = new NetworkCredential(options.windowsLogin, options.windowsPassword, options.windowsDomain);
                var pService = new CSOMProvisionService();

                #region base model deployment

                var rootModel = SPMeta2Model.NewSiteModel(new SiteDefinition() {RequireSelfProcessing = false})
                    .WithFields(fields => fields
                        .AddField(FieldModels.TmServiceCode)
                        .AddField(FieldModels.TmUsageScopeInteger)
                        .AddField(FieldModels.TmRegNumber)
                        .AddField(FieldModels.TmSingleNumber)
                        .AddField(FieldModels.TmRegistrationDate)
                        .AddField(FieldModels.TmComment)
                        .AddField(FieldModels.TmInstanceCounter)
                        .AddField(FieldModels.TmRequestedDocumentPrice)
                        .AddField(FieldModels.TmOutputFactDate)
                        .AddField(FieldModels.TmOutputTargetDate)
                        .AddField(FieldModels.TmPrepareFactDate)
                        .AddField(FieldModels.TmPrepareTargetDate)
                        .AddField(FieldModels.TmRefuseDate)
                        .AddField(FieldModels.TmOutputDate)
                        .AddField(FieldModels.TmErrorDescription)
                        .AddField(FieldModels.TmTaxiBrand)
                        .AddField(FieldModels.TmTaxiModel)
                        .AddField(FieldModels.TmTaxiYear)
                        .AddField(FieldModels.TmTaxiLastToDate)
                        .AddField(FieldModels.TmTaxiStateNumber)
                        .AddField(FieldModels.TmTaxiLeasingContractDetails)
                        .AddField(FieldModels.TmTaxiBodyYellow)
                        .AddField(FieldModels.TmTaxiBodyColor)
                        .AddField(FieldModels.TmTaxiBodyColor2)
                        .AddField(FieldModels.TmTaxiStateNumberYellow)
                        .AddField(FieldModels.TmTaxiTaxometer)
                        .AddField(FieldModels.TmTaxiGps)
                        .AddField(FieldModels.TmTaxiPrevStateNumber)
                        .AddField(FieldModels.TmTaxiDecision)
                        .AddField(FieldModels.TmTaxiChecked)
                        .AddField(FieldModels.TmTaxiBlankNo)
                        .AddField(FieldModels.TmTaxiLicenseNumber)
                        .AddField(FieldModels.TmTaxiInfoOld)
                        .AddField(FieldModels.TmTaxiPrevLicenseNumber)
                        .AddField(FieldModels.TmTaxiPrevLicenseDate)
                        .AddField(FieldModels.TmAttachType)
                        .AddField(FieldModels.TmAttachDocNumber)
                        .AddField(FieldModels.TmAttachDocDate)
                        .AddField(FieldModels.TmAttachDocSerie)
                        .AddField(FieldModels.TmAttachWhoSigned)
                        .AddField(FieldModels.TmMessageId, f => f.OnCreated(
                            (FieldDefinition fieldDef, Field spField) => spField.MakeHidden(false)))
                        .AddField(FieldModels.TmIncomeRequestForm,
                            f => f.OnCreated((FieldDefinition fieldDef, Field spField) =>
                                spField.MakeChoices(new String[] {"Портал госуслуг", "Очный визит"})))
                    )
                    .WithContentTypes(
                        ctList => ctList
                            .AddContentType(ContentTypeModels.TmIncomeRequestState,
                                ct => ct.AddContentTypeFieldLink(FieldModels.TmServiceCode))
                            .AddContentType(ContentTypeModels.TmIncomeRequestStateInternal,
                                ct => ct.AddContentTypeFieldLink(FieldModels.TmServiceCode))
                            .AddContentType(ContentTypeModels.TmDenyReason, ct => ct
                                .AddContentTypeFieldLink(FieldModels.TmServiceCode)
                                .AddContentTypeFieldLink(FieldModels.TmUsageScopeInteger))
                            .AddContentType(ContentTypeModels.TmGovServiceSubType,
                                ct => ct.AddContentTypeFieldLink(FieldModels.TmServiceCode))
                            .AddContentType(ContentTypeModels.TmOutcomeRequestType,
                                ct => ct.AddContentTypeFieldLink(FieldModels.TmServiceCode))
                    );

                var webModel = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                    .WithLists(
                        lists => lists
                            .AddList(ListModels.TmIncomeRequestStateBookList,
                                l => l.AddContentTypeLink(ContentTypeModels.TmIncomeRequestState))
                            .AddList(ListModels.TmIncomeRequestStateInternalBookList,
                                l => l.AddContentTypeLink(ContentTypeModels.TmIncomeRequestStateInternal))
                            .AddList(ListModels.TmDenyReasonBookList,
                                l => l.AddContentTypeLink(ContentTypeModels.TmDenyReason))
                            .AddList(ListModels.TmGovServiceSubTypeBookList,
                                l => l.AddContentTypeLink(ContentTypeModels.TmGovServiceSubType))
                            .AddList(ListModels.TmOutcomeRequestTypeBookList,
                                l => l.AddContentTypeLink(ContentTypeModels.TmOutcomeRequestType))
                    );

                pService.DeployModel(SiteModelHost.FromClientContext(ctx), rootModel);
                pService.DeployModel(WebModelHost.FromClientContext(ctx), webModel);

                #endregion

                #region dependent model elements deployment

                Guid webId = Utils.GetWebId(ctx);
                IEnumerable<List> allLists = Utils.GetWebLists(ctx);

                List incomeRequestStateBookList = Utils.GetList(allLists, ListModels.TmIncomeRequestStateBookList.Url);
                List incomeRequestStateInternalBookList = Utils.GetList(allLists,
                    ListModels.TmIncomeRequestStateInternalBookList.Url);
                List denyReasonBookList = Utils.GetList(allLists, ListModels.TmDenyReasonBookList.Url);
                List govServiceSubTypeBookList = Utils.GetList(allLists, ListModels.TmGovServiceSubTypeBookList.Url);
                List outcomeRequestTypeBookList = Utils.GetList(allLists, ListModels.TmOutcomeRequestTypeBookList.Url);

                var rootModelLookups = SPMeta2Model.NewSiteModel(new SiteDefinition() {RequireSelfProcessing = false})
                    .WithFields(fields => fields
                        .AddField(FieldModels.TmIncomeRequestStateLookup, field => field.OnCreated(
                            (FieldDefinition fieldDef, Field spField) =>
                                spField.MakeLookupConnectionToList(webId, incomeRequestStateBookList.Id,
                                    "LinkTitle")))
                        .AddField(FieldModels.TmIncomeRequestStateInternalLookup, field => field.OnCreated(
                            (FieldDefinition fieldDef, Field spField) =>
                                spField.MakeLookupConnectionToList(webId, incomeRequestStateInternalBookList.Id,
                                    "LinkTitle")))
                        .AddField(FieldModels.TmDenyReasonLookup, field => field.OnCreated(
                            (FieldDefinition fieldDef, Field spField) =>
                                spField.MakeLookupConnectionToList(webId, denyReasonBookList.Id,
                                    "LinkTitle")))
                        .AddField(FieldModels.TmRequestedDocument, field => field.OnCreated(
                            (FieldDefinition fieldDef, Field spField) =>
                                spField.MakeLookupConnectionToList(webId, govServiceSubTypeBookList.Id,
                                    "LinkTitle")))
                        .AddField(FieldModels.TmOutputRequestTypeLookup, field => field.OnCreated(
                            (FieldDefinition fieldDef, Field spField) =>
                                spField.MakeLookupConnectionToList(webId, outcomeRequestTypeBookList.Id,
                                    "LinkTitle")))
                    )
                    .WithContentTypes(ctList => ctList
                        .AddContentType(ContentTypeModels.TmIncomeRequest, ct => ct
                            .AddContentTypeFieldLink(FieldModels.TmRegNumber)
                            .AddContentTypeFieldLink(FieldModels.TmSingleNumber)
                            .AddContentTypeFieldLink(FieldModels.TmIncomeRequestStateLookup)
                            .AddContentTypeFieldLink(FieldModels.TmIncomeRequestStateInternalLookup)
                            .AddContentTypeFieldLink(FieldModels.TmRegistrationDate)
                            .AddContentTypeFieldLink(FieldModels.TmIncomeRequestForm)
                            .AddContentTypeFieldLink(FieldModels.TmDenyReasonLookup)
                            .AddContentTypeFieldLink(FieldModels.TmComment)
                            .AddContentTypeFieldLink(FieldModels.TmRequestedDocument)
                            .AddContentTypeFieldLink(FieldModels.TmInstanceCounter)
                            .AddContentTypeFieldLink(FieldModels.TmRequestedDocumentPrice)
                            .AddContentTypeFieldLink(FieldModels.TmOutputFactDate)
                            .AddContentTypeFieldLink(FieldModels.TmOutputTargetDate)
                            .AddContentTypeFieldLink(FieldModels.TmPrepareFactDate)
                            .AddContentTypeFieldLink(FieldModels.TmPrepareTargetDate)
                            .AddContentTypeFieldLink(FieldModels.TmRefuseDate)
                            .AddContentTypeFieldLink(FieldModels.TmMessageId)
                        ));

                var webModelLookupLists = SPMeta2Model.NewWebModel(new WebDefinition() {RequireSelfProcessing = false})
                    .WithLists(lists => lists
                        .AddList(ListModels.TmIncomeRequestList, l =>
                            l.AddContentTypeLink(ContentTypeModels.TmIncomeRequest)));

                pService.DeployModel(SiteModelHost.FromClientContext(ctx), rootModelLookups);
                pService.DeployModel(WebModelHost.FromClientContext(ctx), webModelLookupLists);

                #endregion

                #region dependent from TmIncomeRequest model elements deployment
                // refresh allLists collection after models deployment
                allLists = Utils.GetWebLists(ctx);
                List incomeRequestList = Utils.GetList(allLists, ListModels.TmIncomeRequestList.Url);

                var rootModelNestedLookups = SPMeta2Model.NewSiteModel(new SiteDefinition() {RequireSelfProcessing = false})
                    .WithFields(fields => fields
                        .AddField(FieldModels.TmIncomeRequestLookup, field => field.OnCreated(
                            (FieldDefinition fieldDef, Field spField) =>
                                spField.MakeLookupConnectionToList(webId, incomeRequestList.Id, "LinkTitle")))
                     )
                     .WithContentTypes(ctList => ctList
                        .AddContentType(ContentTypeModels.TmOutcomeRequestState, ct => ct
                            .AddContentTypeFieldLink(FieldModels.TmOutputRequestTypeLookup)
                            .AddContentTypeFieldLink(FieldModels.TmOutputDate)
                            .AddContentTypeFieldLink(FieldModels.TmErrorDescription)
                            .AddContentTypeFieldLink(FieldModels.TmMessageId)
                            .AddContentTypeFieldLink(FieldModels.TmIncomeRequestLookup)
                         )
                         .AddContentType(ContentTypeModels.TmTaxi, ct => ct
                            .AddContentTypeFieldLink(FieldModels.TmTaxiBrand)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiModel)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiYear)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiLastToDate)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiStateNumber)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiLeasingContractDetails)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiBodyYellow)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiBodyColor)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiBodyColor2)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiStateNumberYellow)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiTaxometer)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiGps)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiPrevStateNumber)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiDecision)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiChecked)
                            .AddContentTypeFieldLink(FieldModels.TmDenyReasonLookup)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiBlankNo)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiLicenseNumber)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiInfoOld)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiPrevLicenseNumber)
                            .AddContentTypeFieldLink(FieldModels.TmTaxiPrevLicenseDate)
                            .AddContentTypeFieldLink(FieldModels.TmMessageId)
                            .AddContentTypeFieldLink(FieldModels.TmIncomeRequestLookup)
                         )
                         .AddContentType(ContentTypeModels.TmAttach, ct => ct
                            .AddContentTypeFieldLink(FieldModels.TmAttachType)
                            .AddContentTypeFieldLink(FieldModels.TmAttachDocNumber)
                            .AddContentTypeFieldLink(FieldModels.TmAttachDocDate)
                            .AddContentTypeFieldLink(FieldModels.TmAttachDocSerie)
                            .AddContentTypeFieldLink(FieldModels.TmAttachWhoSigned)
                            .AddContentTypeFieldLink(FieldModels.TmMessageId)
                            .AddContentTypeFieldLink(FieldModels.TmIncomeRequestLookup)
                         )
                     );

                var webModelNestedLookupLists = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                    .WithLists(lists => lists
                        .AddList(ListModels.TmOutcomeRequestStateList, l =>
                            l.AddContentTypeLink(ContentTypeModels.TmOutcomeRequestState))
                        .AddList(ListModels.TmTaxiList, l =>
                            l.AddContentTypeLink(ContentTypeModels.TmTaxi))
                        .AddList(ListModels.TmIncomeRequestAttachList, l =>
                            l.AddContentTypeLink(ContentTypeModels.TmAttach))
                    );

                pService.DeployModel(SiteModelHost.FromClientContext(ctx), rootModelNestedLookups);
                pService.DeployModel(WebModelHost.FromClientContext(ctx), webModelNestedLookupLists);


                #endregion

                #region make content types default

                Utils.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestStateBookList.Url,
                    ContentTypeModels.TmIncomeRequestState.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestStateInternalBookList.Url,
                    ContentTypeModels.TmIncomeRequestStateInternal.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmDenyReasonBookList.Url,
                    ContentTypeModels.TmDenyReason.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmGovServiceSubTypeBookList.Url,
                    ContentTypeModels.TmGovServiceSubType.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmOutcomeRequestTypeBookList.Url,
                    ContentTypeModels.TmOutcomeRequestType.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestList.Url,
                    ContentTypeModels.TmIncomeRequest.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmOutcomeRequestStateList.Url,
                    ContentTypeModels.TmOutcomeRequestState.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmTaxiList.Url,
                    ContentTypeModels.TmTaxi.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestAttachList.Url,
                    ContentTypeModels.TmAttach.Name);

                #endregion

                #region add bcs fields to list TmIncomeRequestList without spmeta2
                // check bdc features activated
                if (!Utils.CheckFeatureActvation(ctx, new Guid(Consts.BcsCoordinateV5ListsFeatureId)))
                    throw new Exception(String.Format("Feature with id = {0} must be activated",
                        Consts.BcsCoordinateV5ListsFeatureId));
               
                Utils.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestAccountBcsLookupXml,
                    AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToDefaultContentType);
                Utils.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestContactBcsLookupXml,
                    AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToDefaultContentType);
                Utils.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestTrusteeBcsLookupXml,
                    AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToDefaultContentType);

                #endregion

            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Options options = new Options();
            CommandLine.Parser.Default.ParseArguments(args, options);

            if (!CheckArguments(options))
            {
                cLogger.Error("Есть ошибки в параметрах командной строки");
                Console.ReadKey();
                return;
            }

            try
            {
                Deploy(options);
            }
            catch (Exception ex)
            {
                cLogger.Error(String.Format("Ошибка при развертывании: {0}." + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace));
                Console.ReadKey();
                return;
            }

            cLogger.Info("Развертывание успешно завершено");
            Console.ReadKey();
        }
    }
}
