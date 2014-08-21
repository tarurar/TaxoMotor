using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Behaviours;
using Microsoft.SharePoint.Client;

namespace TM.SP.DataModel
{
    public static class AllModels
    {
        #region methods

        public static ModelNode GetTaxoMotorSiteModel()
        {
            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
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
                    .AddField(FieldModels.TmTaxiBlankNo)
                    .AddField(FieldModels.TmTaxiInfoOld)
                    .AddField(FieldModels.TmTaxiPrevLicenseNumber)
                    .AddField(FieldModels.TmTaxiPrevLicenseDate)
                    .AddField(FieldModels.TmTaxiStsDetails)
                    .AddField(FieldModels.TmAttachType)
                    .AddField(FieldModels.TmAttachDocNumber)
                    .AddField(FieldModels.TmAttachDocDate)
                    .AddField(FieldModels.TmAttachDocSerie)
                    .AddField(FieldModels.TmAttachWhoSigned)
                    .AddField(FieldModels.TmRenewalReason_AddressCompany)
                    .AddField(FieldModels.TmRenewalReason_AddressPerson)
                    .AddField(FieldModels.TmRenewalReason_IdentityCard)
                    .AddField(FieldModels.TmRenewalReason_NameCompany)
                    .AddField(FieldModels.TmRenewalReason_NamePerson)
                    .AddField(FieldModels.TmRenewalReason_ReorgCompany)
                    .AddField(FieldModels.TmRenewalReason_StateNumber)
                    .AddField(FieldModels.TmCancellationReasonOther)
                    .AddField(FieldModels.TmMessageId, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeHidden(false)))
                    .AddField(FieldModels.TmIncomeRequestForm,
                        f => f.OnCreated((FieldDefinition fieldDef, Field spField) =>
                            spField.MakeChoices(new String[] { "Портал госуслуг", "Очный визит" })))
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
                        .AddContentType(ContentTypeModels.TmPossessionReason,
                            ct => ct.AddContentTypeFieldLink(FieldModels.TmServiceCode))
                        .AddContentType(ContentTypeModels.TmCancellationReason,
                            ct => ct.AddContentTypeFieldLink(FieldModels.TmServiceCode))
                );

            return model;
        }

        public static ModelNode GetTaxoMotorWebModel()
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
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
                        .AddList(ListModels.TmPossessionReasonBookList,
                            l => l.AddContentTypeLink(ContentTypeModels.TmPossessionReason))
                        .AddList(ListModels.TmCancellationReasonBookList,
                            l => l.AddContentTypeLink(ContentTypeModels.TmCancellationReason))
                );

            return model;
        }

        public static ModelNode GetTaxoMotorSiteDependentModel(ClientContext ctx)
        {
            Guid webId = WebHelpers.GetWebId(ctx);
            IEnumerable<List> allLists = WebHelpers.GetWebLists(ctx);

            List incomeRequestStateBookList = ListHelpers.GetList(allLists, ListModels.TmIncomeRequestStateBookList.Url);
            List incomeRequestStateInternalBookList = ListHelpers.GetList(allLists,
                ListModels.TmIncomeRequestStateInternalBookList.Url);
            List denyReasonBookList = ListHelpers.GetList(allLists, ListModels.TmDenyReasonBookList.Url);
            List govServiceSubTypeBookList = ListHelpers.GetList(allLists, ListModels.TmGovServiceSubTypeBookList.Url);
            List outcomeRequestTypeBookList = ListHelpers.GetList(allLists, ListModels.TmOutcomeRequestTypeBookList.Url);
            List possessionReasonBookList = ListHelpers.GetList(allLists, ListModels.TmPossessionReasonBookList.Url);
            List cancellationReasonBookList = ListHelpers.GetList(allLists, ListModels.TmCancellationReasonBookList.Url);

            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
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
                    .AddField(FieldModels.TmPossessionReasonLookup, field => field.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                            spField.MakeLookupConnectionToList(webId, possessionReasonBookList.Id,
                                "LinkTitle")))
                    .AddField(FieldModels.TmCancellationReasonLookup, field => field.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                            spField.MakeLookupConnectionToList(webId, cancellationReasonBookList.Id,
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
                    )
                    .AddContentType(ContentTypeModels.TmNewIncomeRequest)
                    .AddContentType(ContentTypeModels.TmDuplicateIncomeRequest)
                    .AddContentType(ContentTypeModels.TmRenewIncomeRequest, ct => ct
                        .AddContentTypeFieldLink(FieldModels.TmRenewalReason_AddressCompany)
                        .AddContentTypeFieldLink(FieldModels.TmRenewalReason_AddressPerson)
                        .AddContentTypeFieldLink(FieldModels.TmRenewalReason_IdentityCard)
                        .AddContentTypeFieldLink(FieldModels.TmRenewalReason_NameCompany)
                        .AddContentTypeFieldLink(FieldModels.TmRenewalReason_NamePerson)
                        .AddContentTypeFieldLink(FieldModels.TmRenewalReason_ReorgCompany)
                        .AddContentTypeFieldLink(FieldModels.TmRenewalReason_StateNumber)
                        )
                    .AddContentType(ContentTypeModels.TmCancelIncomeRequest, ct => ct
                        .AddContentTypeFieldLink(FieldModels.TmCancellationReasonLookup)
                        .AddContentTypeFieldLink(FieldModels.TmCancellationReasonOther)
                        )
                    );

            return model;
        }

        public static ModelNode GetTaxoMotorWebDependentModel()
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                .WithLists(lists => lists
                    .AddList(ListModels.TmIncomeRequestList, l => l
                        .AddContentTypeLink(ContentTypeModels.TmNewIncomeRequest)
                        .AddContentTypeLink(ContentTypeModels.TmDuplicateIncomeRequest)
                        .AddContentTypeLink(ContentTypeModels.TmCancelIncomeRequest)
                        .AddContentTypeLink(ContentTypeModels.TmRenewIncomeRequest)
                ));

            return model;
        }

        public static ModelNode GetTaxoMotorIncomeRequestSiteDependentModel(ClientContext ctx)
        {
            Guid webId = WebHelpers.GetWebId(ctx);
            var allLists = WebHelpers.GetWebLists(ctx);
            List incomeRequestList = ListHelpers.GetList(allLists, ListModels.TmIncomeRequestList.Url);

            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
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
                        .AddContentTypeFieldLink(FieldModels.TmDenyReasonLookup)
                        .AddContentTypeFieldLink(FieldModels.TmTaxiBlankNo)
                        .AddContentTypeFieldLink(FieldModels.TmTaxiInfoOld)
                        .AddContentTypeFieldLink(FieldModels.TmTaxiPrevLicenseNumber)
                        .AddContentTypeFieldLink(FieldModels.TmTaxiPrevLicenseDate)
                        .AddContentTypeFieldLink(FieldModels.TmTaxiStsDetails)
                        .AddContentTypeFieldLink(FieldModels.TmPossessionReasonLookup)
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

            return model;
        }

        public static ModelNode GetTaxoMotorIncomeRequestWebDependentModel()
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                .WithLists(lists => lists
                    .AddList(ListModels.TmOutcomeRequestStateList, l =>
                        l.AddContentTypeLink(ContentTypeModels.TmOutcomeRequestState))
                    .AddList(ListModels.TmTaxiList, l =>
                        l.AddContentTypeLink(ContentTypeModels.TmTaxi))
                    .AddList(ListModels.TmIncomeRequestAttachList, l =>
                        l.AddContentTypeLink(ContentTypeModels.TmAttach))
                );

            return model;
        }

        #endregion
    }
}
