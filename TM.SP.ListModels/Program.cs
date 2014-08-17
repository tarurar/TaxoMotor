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

                #region make content types default

                Utils.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestStateBookList.Url,
                    ContentTypeModels.TmIncomeRequestState.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestStateInternalBookList.Url,
                    ContentTypeModels.TmIncomeRequestStateInternal.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmDenyReasonBookList.Url,
                    ContentTypeModels.TmDenyReason.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmGovServiceSubTypeBookList.Url,
                    ContentTypeModels.TmGovServiceSubType.Name);
                Utils.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestList.Url,
                    ContentTypeModels.TmIncomeRequest.Name);
                
                #endregion

                #region add bcs fields to list TmIncomeRequestList without spmeta2
                // check bdc features activated
                if (!Utils.CheckFeatureActvation(ctx, new Guid(Consts.BcsCoordinateV5ListsFeatureId)))
                    throw new Exception(String.Format("Feature with id = {0} must be activated",
                        Consts.BcsCoordinateV5ListsFeatureId));
                // refresh allLists collection after models deployment
                allLists = Utils.GetWebLists(ctx);
                List incomeRequestList = Utils.GetList(allLists, ListModels.TmIncomeRequestList.Url);

                Utils.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestAccountBcsLookupXml,
                    AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToDefaultContentType);
                Utils.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestContactBcsLookupXml,
                    AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToDefaultContentType);
                Utils.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestTrusteeBcsLookupXml,
                    AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToDefaultContentType);

                #endregion

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
