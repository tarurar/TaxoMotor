using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SPMeta2.Enumerations;
using SPMeta2.Extensions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Behaviours;
using Microsoft.SharePoint.Client;
using SPMeta2.Syntax.Default.Modern;
using TM.SP.DataModel.Definitions;
using TM.SP.DataModel.Helpers;

namespace TM.SP.DataModel
{
    public static class AllModels
    {
        #region methods

        public static ModelNode GetTaxoMotorSiteModel(ClientContext ctx)
        {
            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
                .WithFields(fields => fields
                    .AddField(Fields.TmServiceCode)
                    .AddField(Fields.TmUsageScopeInteger)
                    .AddField(Fields.TmRegNumber)
                    .AddField(Fields.TmSingleNumber)
                    .AddField(Fields.TmRegistrationDate, f => f.OnCreated((FieldDefinition def, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmComment)
                    .AddField(Fields.TmInstanceCounter)
                    .AddField(Fields.TmRequestedDocumentPrice)
                    .AddField(Fields.TmOutputFactDate, f => f.OnCreated((FieldDefinition def, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmOutputTargetDate, f => f.OnCreated((FieldDefinition def, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmPrepareFactDate, f => f.OnCreated((FieldDefinition def, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmPrepareTargetDate, f => f.OnCreated((FieldDefinition def, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmRefuseDate)
                    .AddField(Fields.TmOutputDate, f => f.OnCreated((FieldDefinition def, Field spField) => spField.MakeDateOnly()))
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
                    .AddField(Fields.TmTaxiOwner)
                    .AddField(Fields.TmTaxiBodyType)
                    .AddField(Fields.TmTaxiStatus, f => f.OnCreated((FieldDefinition fieldDef, Field spField) =>
                        spField.MakeChoices(new String[] { "В работе", "Решено положительно", "Отказано", "Не получено" })))
                    .AddField(Fields.TmTaxiDenyComment)
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
                    .AddField(Fields.TmConfigurationCategory)
                    .AddField(Fields.TmConfigurationValue)
                    .AddField(Fields.TmConfigurationDescr)
                    .AddField(Fields.TmAnswerReceived)
                    .AddField(Fields.TmCentralDocStoreUrl)
                    .AddField(Fields.TmAttachDocSubType)
                    .AddField(Fields.TmAttachValidityPeriod)
                    .AddField(Fields.TmAttachListCount)
                    .AddField(Fields.TmAttachCopyCount)
                    .AddField(Fields.TmAttachDivisionCode)
                    .AddField(Fields.TmIdentityDocTypeComment)
                    .AddField(Fields.TmAttachSingleStrDocName)
                    .AddField(Fields.TmBlankNo)
                    .AddField(Fields.TmBlankSeries)
                    .AddField(Fields.TmOrganizationName)
                    .AddField(Fields.TmOrgOgrn)
                    .AddField(Fields.TmOrgInn)
                    .AddField(Fields.TmLicenseStatus, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeChoices(new String[] {"Оригинал", "Дубль", "Пристановлено", "Аннулировано", "С изменениями"})))
                    .AddField(Fields.TmOrgLfb)
                    .AddField(Fields.TmJuridicalAddress)
                    .AddField(Fields.TmPhoneNumber)
                    .AddField(Fields.TmAddContactData)
                    .AddField(Fields.TmJuridicalPersonAbbreviation)
                    .AddField(Fields.TmLicenseOutputDate, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmLicenseTillDate, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmLicenseTillSuspensionDate, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeDateOnly()))
                    .AddField(Fields.TmLicenseCancellationReason)
                    .AddField(Fields.TmLicenseChangeReason)
                    .AddField(Fields.TmLicenseSuspensionReason)
                    .AddField(Fields.TmLicenseInvalidReason)
                    .AddField(Fields.TmLicenseExternalId)
                    .AddField(Fields.TmIncomeRequestTaxiModels, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestTaxiBrands, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestTaxiStateNumbers, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestTaxiYears, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestTaxiLastToDates, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestDeclarantNames, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestDeclarantAddresses, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestDeclarantINNs, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestTrusteeNames, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestTrusteeAddresses, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestTrusteeINNs, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestIdentityDocs, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeFillInChoice()))
                    .AddField(Fields.TmIncomeRequestSystemUpdateAvailable, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeDefaultValue("FALSE")))
                    .AddField(Fields.TmIncomeRequestSysUpdAvailText, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) =>
                        {
                            FieldCalculated field = spField as FieldCalculated;
                            if (field != null)
                                field.Formula = "=IF([Обновление обращения разрешено], \"1\", \"0\")";
                        }))
                    .AddField(Fields.TmMessageId, f => f.OnCreated(
                        (FieldDefinition fieldDef, Field spField) => spField.MakeHidden(false)))
                    .AddField(Fields.TmIncomeRequestForm,
                        f => f.OnCreated((FieldDefinition fieldDef, Field spField) =>
                            spField.MakeChoices(new String[] { "Портал госуслуг", "Очный визит" })))
                )
                .WithContentTypes(
                    ctList => ctList
                        .AddContentType(ContentTypes.TmIncomeRequestState, ct => ct
                            .AddContentTypeFieldLink(Fields.TmServiceCode)
                            .AddContentTypeFieldLink(Fields.TmIncomeRequestSystemUpdateAvailable)
                            .AddContentTypeFieldLink(Fields.TmIncomeRequestSysUpdAvailText))
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
                        .AddContentType(ContentTypes.TmAttachDoc,
                            ct => ct.AddContentTypeFieldLink(Fields.TmCentralDocStoreUrl))
                        .AddContentType(ContentTypes.TmConfigurationEntry, ct => ct
                            .AddContentTypeFieldLink(Fields.TmConfigurationCategory)
                            .AddContentTypeFieldLink(Fields.TmConfigurationValue)
                            .AddContentTypeFieldLink(Fields.TmConfigurationDescr))
                        .AddContentType(ContentTypes.TmIdentityDocumentType, ct => ct
                            .AddContentTypeFieldLink(Fields.TmServiceCode)
                            .AddContentTypeFieldLink(Fields.TmIdentityDocTypeComment))
                        .AddContentType(ContentTypes.TmLicense, ct => ct
                            .AddContentTypeFieldLink(Fields.TmRegNumber)
                            .AddContentTypeFieldLink(Fields.TmBlankSeries)
                            .AddContentTypeFieldLink(Fields.TmBlankNo)
                            .AddContentTypeFieldLink(Fields.TmOrganizationName)
                            .AddContentTypeFieldLink(Fields.TmOrgOgrn)
                            .AddContentTypeFieldLink(Fields.TmOrgInn)
                            .AddContentTypeFieldLink(Fields.TmLicenseStatus)
                            .AddContentTypeFieldLink(Fields.TmOrgLfb)
                            .AddContentTypeFieldLink(Fields.TmJuridicalAddress)
                            .AddContentTypeFieldLink(Fields.TmPhoneNumber)
                            .AddContentTypeFieldLink(Fields.TmAddContactData)
                            .AddContentTypeFieldLink(Fields.TmJuridicalPersonAbbreviation)
                            .AddContentTypeFieldLink(Fields.TmTaxiBrand)
                            .AddContentTypeFieldLink(Fields.TmTaxiModel)
                            .AddContentTypeFieldLink(Fields.TmTaxiYear)
                            .AddContentTypeFieldLink(Fields.TmTaxiStateNumber)
                            .AddContentTypeFieldLink(Fields.TmLicenseOutputDate)
                            .AddContentTypeFieldLink(Fields.TmLicenseTillDate)
                            .AddContentTypeFieldLink(Fields.TmLicenseTillSuspensionDate)
                            .AddContentTypeFieldLink(Fields.TmLicenseCancellationReason)
                            .AddContentTypeFieldLink(Fields.TmLicenseSuspensionReason)
                            .AddContentTypeFieldLink(Fields.TmLicenseChangeReason)
                            .AddContentTypeFieldLink(Fields.TmLicenseInvalidReason)
                            .AddContentTypeFieldLink(Fields.TmLicenseExternalId))
                );

            return model;
        }

        public static ModelNode GetTaxoMotorWebModel(ClientContext ctx)
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                .WithLists(
                    lists => lists
                        .AddList(Lists.TmIncomeRequestStateBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmIncomeRequestState))
                        .AddList(Lists.TmIncomeRequestStateInternalBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmIncomeRequestStateInternal))
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
                        .AddList(Lists.TmConfigurationList,
                            l => l.AddContentTypeLink(ContentTypes.TmConfigurationEntry))
                        .AddList(Lists.TmAttachLib,
                            l => l.AddContentTypeLink(ContentTypes.TmAttachDoc))
                        .AddList(Lists.TmIdentityDocumentTypeBookList,
                            l => l.AddContentTypeLink(ContentTypes.TmIdentityDocumentType))
                        .AddList(Lists.TmProjectScripts)
                        .AddList(Lists.TmLicenseList, l =>
                        {
                            l.AddContentTypeLink(ContentTypes.TmLicense);
                            l.OnCreated((ListDefinition def, List spList) => spList.MakeFolderCreationAvailable(true));
                        })
                );

            return model;
        }

        public static ModelNode GetTaxoMotorSiteDependentModel(ClientContext ctx)
        { 
            var model = SPMeta2Model.NewSiteModel(new SiteDefinition() { RequireSelfProcessing = false })
                .WithContentTypes(ctList => ctList
                    .AddContentType(ContentTypes.TmIncomeRequest, ct => ct
                        .AddContentTypeFieldLink(Fields.TmRegNumber)
                        .AddContentTypeFieldLink(Fields.TmSingleNumber)
                        .AddContentTypeFieldLink(Fields.TmRegistrationDate)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestForm)
                        .AddContentTypeFieldLink(Fields.TmComment)
                        .AddContentTypeFieldLink(Fields.TmInstanceCounter)
                        .AddContentTypeFieldLink(Fields.TmRequestedDocumentPrice)
                        .AddContentTypeFieldLink(Fields.TmOutputFactDate)
                        .AddContentTypeFieldLink(Fields.TmOutputTargetDate)
                        .AddContentTypeFieldLink(Fields.TmPrepareFactDate)
                        .AddContentTypeFieldLink(Fields.TmPrepareTargetDate)
                        .AddContentTypeFieldLink(Fields.TmRefuseDate)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTaxiModels)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTaxiBrands)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTaxiStateNumbers)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTaxiYears)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTaxiLastToDates)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestDeclarantNames)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestDeclarantAddresses)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestDeclarantINNs)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTrusteeNames)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTrusteeAddresses)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestTrusteeINNs)
                        .AddContentTypeFieldLink(Fields.TmIncomeRequestIdentityDocs)
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
                    ));

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
                        .AddContentTypeFieldLink(Fields.TmOutputDate)
                        .AddContentTypeFieldLink(Fields.TmErrorDescription)
                        .AddContentTypeFieldLink(Fields.TmMessageId)
                        .AddContentTypeFieldLink(Fields.TmAnswerReceived)
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
                        .AddContentTypeFieldLink(Fields.TmTaxiBlankNo)
                        .AddContentTypeFieldLink(Fields.TmTaxiInfoOld)
                        .AddContentTypeFieldLink(Fields.TmTaxiPrevLicenseNumber)
                        .AddContentTypeFieldLink(Fields.TmTaxiPrevLicenseDate)
                        .AddContentTypeFieldLink(Fields.TmTaxiStsDetails)
                        .AddContentTypeFieldLink(Fields.TmTaxiOwner)
                        .AddContentTypeFieldLink(Fields.TmTaxiBodyType)
                        .AddContentTypeFieldLink(Fields.TmTaxiStatus)
                        .AddContentTypeFieldLink(Fields.TmTaxiDenyComment)
                        .AddContentTypeFieldLink(Fields.TmMessageId)
                     )
                     .AddContentType(ContentTypes.TmAttach, ct => ct
                        .AddContentTypeFieldLink(Fields.TmAttachType)
                        .AddContentTypeFieldLink(Fields.TmAttachDocNumber)
                        .AddContentTypeFieldLink(Fields.TmAttachDocDate)
                        .AddContentTypeFieldLink(Fields.TmAttachDocSerie)
                        .AddContentTypeFieldLink(Fields.TmAttachWhoSigned)
                        .AddContentTypeFieldLink(Fields.TmAttachDocSubType)
                        .AddContentTypeFieldLink(Fields.TmAttachValidityPeriod)
                        .AddContentTypeFieldLink(Fields.TmAttachListCount)
                        .AddContentTypeFieldLink(Fields.TmAttachCopyCount)
                        .AddContentTypeFieldLink(Fields.TmAttachDivisionCode)
                        .AddContentTypeFieldLink(Fields.TmAttachSingleStrDocName)
                        .AddContentTypeFieldLink(Fields.TmMessageId)
                     )
                 );

            return model;
        }

        public static ModelNode GetTaxoMotorIncomeRequestWebDependentModel(ClientContext ctx)
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

        public static ModelNode GetTaxoMotorWebPagesModel(ClientContext ctx)
        {
            var model = SPMeta2Model.NewWebModel(new WebDefinition() { RequireSelfProcessing = false })
                .WithLists(
                    lists => lists
                        .AddList(Lists.TmProjectSitePages, list =>
                        {
                            list.AddWebPartPage(Pages.IncomeRequestSearch, page =>
                            {
                                if (!WebHelpers.CheckFeatureActivation(ctx,
                                        new Guid(ModelConsts.SPListViewFilter20FeatureId), FeatureScope.Site))
                                    throw new Exception(String.Format("Feature with id = {0} must be activated",
                                        ModelConsts.SPListViewFilter20FeatureId));
                                string wpXml = String.Empty;
                                // adding SPListViewFilter web part
                                WebParts.SPListViewFilter.WebpartXmlTemplate = WebPartHelpers.SetWebPartV3PropertyNode("Title", WebParts.SPListViewFilter.Title, WebParts.SPListViewFilter.WebpartXmlTemplate);
                                page.AddWebPart(WebParts.SPListViewFilter);
                                // adding XsltListViewWebPart for income requests to be filtered
                                IEnumerable<List> allLists = WebHelpers.GetWebLists(ctx);
                                List incomeRequestList     = ListHelpers.GetList(allLists, Lists.TmIncomeRequestList.Url);
                                
                                wpXml = WebPartHelpers.SetWebPartV3PropertyNode("ListId", incomeRequestList.Id.ToString("D"), WebParts.IncomeRequestListView.WebpartXmlTemplate);
                                wpXml = WebPartHelpers.SetWebPartV3PropertyNode("ListName", incomeRequestList.Id.ToString("B"), wpXml);
                                wpXml = WebPartHelpers.SetWebPartV3PropertyNode("Title", WebParts.IncomeRequestListView.Title, wpXml);
                                WebParts.IncomeRequestListView.WebpartXmlTemplate = wpXml;
                                page.AddWebPart(WebParts.IncomeRequestListView);
                            });
                        })
                );

            return model;
        }

        #endregion
    }
}
