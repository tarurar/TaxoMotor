using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Extensions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Behaviours;
using Microsoft.SharePoint.Client;
using SPMeta2.Syntax.Default.Modern;

namespace TM.SP.DataModel
{
    public static class AllModels
    {
        #region methods

        public static ModelNode GetTaxoMotorSiteModel()
        {
            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
                .WithFields(fields => fields
                    .AddField(Fields.TmServiceCode)
                    .AddField(Fields.TmUsageScopeInteger)
                    .AddField(Fields.TmRegNumber)
                    .AddField(Fields.TmSingleNumber)
                    .AddField(Fields.TmRegistrationDate)
                    .AddField(Fields.TmComment)
                    .AddField(Fields.TmInstanceCounter)
                    .AddField(Fields.TmRequestedDocumentPrice)
                    .AddField(Fields.TmOutputFactDate)
                    .AddField(Fields.TmOutputTargetDate)
                    .AddField(Fields.TmPrepareFactDate)
                    .AddField(Fields.TmPrepareTargetDate)
                    .AddField(Fields.TmRefuseDate)
                    .AddField(Fields.TmOutputDate)
                    .AddField(Fields.TmErrorDescription)
                    .AddField(Fields.TmTaxiBrand)
                    .AddField(Fields.TmTaxiModel)
                    .AddField(Fields.TmTaxiYear)
                    .AddField(Fields.TmTaxiLastToDate)
                    .AddField(Fields.TmTaxiStateNumber)
                    .AddField(Fields.TmTaxiLeasingContractDetails)
                    .AddField(Fields.TmTaxiBodyYellow)
                    .AddField(Fields.TmTaxiBodyColor)
                    .AddField(Fields.TmTaxiBodyColor2)
                    .AddField(Fields.TmTaxiStateNumberYellow)
                    .AddField(Fields.TmTaxiTaxometer)
                    .AddField(Fields.TmTaxiGps)
                    .AddField(Fields.TmTaxiPrevStateNumber)
                    .AddField(Fields.TmTaxiDecision)
                    .AddField(Fields.TmTaxiBlankNo)
                    .AddField(Fields.TmTaxiInfoOld)
                    .AddField(Fields.TmTaxiPrevLicenseNumber)
                    .AddField(Fields.TmTaxiPrevLicenseDate)
                    .AddField(Fields.TmTaxiStsDetails)
                    .AddField(Fields.TmAttachType)
                    .AddField(Fields.TmAttachDocNumber)
                    .AddField(Fields.TmAttachDocDate)
                    .AddField(Fields.TmAttachDocSerie)
                    .AddField(Fields.TmAttachWhoSigned)
                    .AddField(Fields.TmRenewalReason_AddressCompany)
                    .AddField(Fields.TmRenewalReason_AddressPerson)
                    .AddField(Fields.TmRenewalReason_IdentityCard)
                    .AddField(Fields.TmRenewalReason_NameCompany)
                    .AddField(Fields.TmRenewalReason_NamePerson)
                    .AddField(Fields.TmRenewalReason_ReorgCompany)
                    .AddField(Fields.TmRenewalReason_StateNumber)
                    .AddField(Fields.TmCancellationReasonOther)
                    .AddField(Fields.TmMessageId, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeHidden(false)))
                    .AddField(Fields.TmIncomeRequestForm,
                        f => f.OnCreated((FieldDefinition fieldDef, Field spField) =>
                            spField.MakeChoices(new String[] { "Портал госуслуг", "Очный визит" })))
                )
                .WithContentTypes(
                    ctList => ctList
                        .AddContentType(ContentTypes.TmIncomeRequestState,
                            ct => ct.AddContentTypeFieldLink(Fields.TmServiceCode))
                        .AddContentType(ContentTypes.TmIncomeRequestStateInternal,
                            ct => ct.AddContentTypeFieldLink(Fields.TmServiceCode))
                        .AddContentType(ContentTypes.TmDenyReason, ct => ct
                            .AddContentTypeFieldLink(Fields.TmServiceCode)
                            .AddContentTypeFieldLink(Fields.TmUsageScopeInteger))
                        .AddContentType(ContentTypes.TmGovServiceSubType,
                            ct => ct.AddContentTypeFieldLink(Fields.TmServiceCode))
                        .AddContentType(ContentTypes.TmOutcomeRequestType,
                            ct => ct.AddContentTypeFieldLink(Fields.TmServiceCode))
                        .AddContentType(ContentTypes.TmPossessionReason,
                            ct => ct.AddContentTypeFieldLink(Fields.TmServiceCode))
                        .AddContentType(ContentTypes.TmCancellationReason,
                            ct => ct.AddContentTypeFieldLink(Fields.TmServiceCode))
                );

            return model;
        }

        public static ModelNode GetTaxoMotorWebModel()
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                .WithLists(
                    lists => lists
                        .AddList(Lists.TmIncomeRequestStateBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmIncomeRequestState))
                            .OnProvisioned<List>(context =>
                            {
                                var clientContext = context.Object.Context;
                                var list = context.Object;

                                clientContext.Load(list, inc => inc.Id);
                                clientContext.ExecuteQuery();
                                Plumsail.Fields.TmIncomeRequestStateLookupXml.ListId = list.Id;
                            })
                        .AddList(Lists.TmIncomeRequestStateInternalBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmIncomeRequestStateInternal))
                            .OnProvisioned<List>(context =>
                            {
                                var clientContext = context.Object.Context;
                                var list = context.Object;

                                clientContext.Load(list, inc => inc.Id);
                                clientContext.ExecuteQuery();
                                Plumsail.Fields.TmIncomeRequestStateInternalLookupXml.ListId = list.Id;
                            })
                        .AddList(Lists.TmDenyReasonBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmDenyReason))
                        .AddList(Lists.TmGovServiceSubTypeBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmGovServiceSubType))
                        .AddList(Lists.TmOutcomeRequestTypeBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmOutcomeRequestType))
                        .AddList(Lists.TmPossessionReasonBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmPossessionReason))
                        .AddList(Lists.TmCancellationReasonBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmCancellationReason))
                );

            return model;
        }

        public static ModelNode GetTaxoMotorSiteDependentModel(ClientContext ctx)
        {
            Guid webId = WebHelpers.GetWebId(ctx);
            IEnumerable<List> allLists = WebHelpers.GetWebLists(ctx);

            List incomeRequestStateBookList = ListHelpers.GetList(allLists, Lists.TmIncomeRequestStateBookList.Url);
            List incomeRequestStateInternalBookList = ListHelpers.GetList(allLists,
                Lists.TmIncomeRequestStateInternalBookList.Url);
            List denyReasonBookList = ListHelpers.GetList(allLists, Lists.TmDenyReasonBookList.Url);
            List govServiceSubTypeBookList = ListHelpers.GetList(allLists, Lists.TmGovServiceSubTypeBookList.Url);
            List outcomeRequestTypeBookList = ListHelpers.GetList(allLists, Lists.TmOutcomeRequestTypeBookList.Url);
            List possessionReasonBookList = ListHelpers.GetList(allLists, Lists.TmPossessionReasonBookList.Url);
            List cancellationReasonBookList = ListHelpers.GetList(allLists, Lists.TmCancellationReasonBookList.Url);

            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
                .WithFields(fields => fields
                    .AddField(Fields.TmDenyReasonLookup, field => field.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                            spField.MakeLookupConnectionToList(webId, denyReasonBookList.Id,
                                "LinkTitle")))
                    .AddField(Fields.TmRequestedDocument, field => field.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                            spField.MakeLookupConnectionToList(webId, govServiceSubTypeBookList.Id,
                                "LinkTitle")))
                    .AddField(Fields.TmOutputRequestTypeLookup, field => field.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                            spField.MakeLookupConnectionToList(webId, outcomeRequestTypeBookList.Id,
                                "LinkTitle")))
                    .AddField(Fields.TmPossessionReasonLookup, field => field.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                            spField.MakeLookupConnectionToList(webId, possessionReasonBookList.Id,
                                "LinkTitle")))
                    .AddField(Fields.TmCancellationReasonLookup, field => field.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                            spField.MakeLookupConnectionToList(webId, cancellationReasonBookList.Id,
                                "LinkTitle")))
                )
                .WithContentTypes(ctList => ctList
                    .AddContentType(ContentTypes.TmIncomeRequest, ct => ct
                        .AddContentTypeFieldLink(Fields.TmRegNumber)
                        .AddContentTypeFieldLink(Fields.TmSingleNumber)
                        .AddContentTypeFieldLink(Fields.TmRegistrationDate)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestForm)
                        .AddContentTypeFieldLink(Fields.TmDenyReasonLookup)
                        .AddContentTypeFieldLink(Fields.TmComment)
                        .AddContentTypeFieldLink(Fields.TmRequestedDocument)
                        .AddContentTypeFieldLink(Fields.TmInstanceCounter)
                        .AddContentTypeFieldLink(Fields.TmRequestedDocumentPrice)
                        .AddContentTypeFieldLink(Fields.TmOutputFactDate)
                        .AddContentTypeFieldLink(Fields.TmOutputTargetDate)
                        .AddContentTypeFieldLink(Fields.TmPrepareFactDate)
                        .AddContentTypeFieldLink(Fields.TmPrepareTargetDate)
                        .AddContentTypeFieldLink(Fields.TmRefuseDate)
                        .AddContentTypeFieldLink(Fields.TmMessageId)
                    )
                    .AddContentType(ContentTypes.TmNewIncomeRequest)
                    .AddContentType(ContentTypes.TmDuplicateIncomeRequest)
                    .AddContentType(ContentTypes.TmRenewIncomeRequest, ct => ct
                        .AddContentTypeFieldLink(Fields.TmRenewalReason_AddressCompany)
                        .AddContentTypeFieldLink(Fields.TmRenewalReason_AddressPerson)
                        .AddContentTypeFieldLink(Fields.TmRenewalReason_IdentityCard)
                        .AddContentTypeFieldLink(Fields.TmRenewalReason_NameCompany)
                        .AddContentTypeFieldLink(Fields.TmRenewalReason_NamePerson)
                        .AddContentTypeFieldLink(Fields.TmRenewalReason_ReorgCompany)
                        .AddContentTypeFieldLink(Fields.TmRenewalReason_StateNumber)
                        )
                    .AddContentType(ContentTypes.TmCancelIncomeRequest, ct => ct
                        .AddContentTypeFieldLink(Fields.TmCancellationReasonLookup)
                        .AddContentTypeFieldLink(Fields.TmCancellationReasonOther)
                        )
                    );

            return model;
        }

        public static ModelNode GetTaxoMotorWebDependentModel()
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() {RequireSelfProcessing = false})
                .WithLists(lists => lists
                    .AddList(Lists.TmIncomeRequestList, l => l
                        .AddContentTypeLink(ContentTypes.TmNewIncomeRequest)
                        .AddContentTypeLink(ContentTypes.TmDuplicateIncomeRequest)
                        .AddContentTypeLink(ContentTypes.TmCancelIncomeRequest)
                        .AddContentTypeLink(ContentTypes.TmRenewIncomeRequest)
                    )
                    .OnProvisioned<List>(context =>
                    {
                        var clientContext = context.Object.Context;
                        var list = context.Object;

                        clientContext.Load(list, inc => inc.Id);
                        clientContext.ExecuteQuery();
                        Plumsail.Fields.TmIncomeRequestLookupXml.ListId = list.Id;
                    }));

            return model;
        }

        public static ModelNode GetTaxoMotorIncomeRequestSiteDependentModel(ClientContext ctx)
        {
            Guid webId = WebHelpers.GetWebId(ctx);
            var allLists = WebHelpers.GetWebLists(ctx);
            List incomeRequestList = ListHelpers.GetList(allLists, Lists.TmIncomeRequestList.Url);

            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
                 .WithContentTypes(ctList => ctList
                    .AddContentType(ContentTypes.TmOutcomeRequestState, ct => ct
                        .AddContentTypeFieldLink(Fields.TmOutputRequestTypeLookup)
                        .AddContentTypeFieldLink(Fields.TmOutputDate)
                        .AddContentTypeFieldLink(Fields.TmErrorDescription)
                        .AddContentTypeFieldLink(Fields.TmMessageId)
                     )
                     .AddContentType(ContentTypes.TmTaxi, ct => ct
                        .AddContentTypeFieldLink(Fields.TmTaxiBrand)
                        .AddContentTypeFieldLink(Fields.TmTaxiModel)
                        .AddContentTypeFieldLink(Fields.TmTaxiYear)
                        .AddContentTypeFieldLink(Fields.TmTaxiLastToDate)
                        .AddContentTypeFieldLink(Fields.TmTaxiStateNumber)
                        .AddContentTypeFieldLink(Fields.TmTaxiLeasingContractDetails)
                        .AddContentTypeFieldLink(Fields.TmTaxiBodyYellow)
                        .AddContentTypeFieldLink(Fields.TmTaxiBodyColor)
                        .AddContentTypeFieldLink(Fields.TmTaxiBodyColor2)
                        .AddContentTypeFieldLink(Fields.TmTaxiStateNumberYellow)
                        .AddContentTypeFieldLink(Fields.TmTaxiTaxometer)
                        .AddContentTypeFieldLink(Fields.TmTaxiGps)
                        .AddContentTypeFieldLink(Fields.TmTaxiPrevStateNumber)
                        .AddContentTypeFieldLink(Fields.TmTaxiDecision)
                        .AddContentTypeFieldLink(Fields.TmDenyReasonLookup)
                        .AddContentTypeFieldLink(Fields.TmTaxiBlankNo)
                        .AddContentTypeFieldLink(Fields.TmTaxiInfoOld)
                        .AddContentTypeFieldLink(Fields.TmTaxiPrevLicenseNumber)
                        .AddContentTypeFieldLink(Fields.TmTaxiPrevLicenseDate)
                        .AddContentTypeFieldLink(Fields.TmTaxiStsDetails)
                        .AddContentTypeFieldLink(Fields.TmPossessionReasonLookup)
                        .AddContentTypeFieldLink(Fields.TmMessageId)
                     )
                     .AddContentType(ContentTypes.TmAttach, ct => ct
                        .AddContentTypeFieldLink(Fields.TmAttachType)
                        .AddContentTypeFieldLink(Fields.TmAttachDocNumber)
                        .AddContentTypeFieldLink(Fields.TmAttachDocDate)
                        .AddContentTypeFieldLink(Fields.TmAttachDocSerie)
                        .AddContentTypeFieldLink(Fields.TmAttachWhoSigned)
                        .AddContentTypeFieldLink(Fields.TmMessageId)
                     )
                 );

            return model;
        }

        public static ModelNode GetTaxoMotorIncomeRequestWebDependentModel()
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                .WithLists(lists => lists
                    .AddList(Lists.TmOutcomeRequestStateList, l =>
                        l.AddContentTypeLink(ContentTypes.TmOutcomeRequestState))
                    .AddList(Lists.TmTaxiList, l =>
                        l.AddContentTypeLink(ContentTypes.TmTaxi))
                    .AddList(Lists.TmIncomeRequestAttachList, l =>
                        l.AddContentTypeLink(ContentTypes.TmAttach))
                );

            return model;
        }

        #endregion
    }
}
