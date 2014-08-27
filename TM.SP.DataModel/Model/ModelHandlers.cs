using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Behaviours;
using TM.SP.DataModel.Consts;
using PlumsailFields = TM.SP.DataModel.Plumsail.Fields;

namespace TM.SP.DataModel
{
    public static class ModelHandlers
    {
        #region methods

        public static void MakeContentTypesDefault(ClientContext ctx)
        {
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmIncomeRequestStateBookList.Url,
                    ContentTypes.TmIncomeRequestState.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmIncomeRequestStateInternalBookList.Url,
                ContentTypes.TmIncomeRequestStateInternal.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmDenyReasonBookList.Url,
                ContentTypes.TmDenyReason.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmGovServiceSubTypeBookList.Url,
                ContentTypes.TmGovServiceSubType.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmOutcomeRequestTypeBookList.Url,
                ContentTypes.TmOutcomeRequestType.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmIncomeRequestList.Url,
                new string[]
                {
                    ContentTypes.TmNewIncomeRequest.Name, 
                    ContentTypes.TmDuplicateIncomeRequest.Name,
                    ContentTypes.TmRenewIncomeRequest.Name, 
                    ContentTypes.TmCancelIncomeRequest.Name
                });
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmOutcomeRequestStateList.Url,
                ContentTypes.TmOutcomeRequestState.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmTaxiList.Url,
                ContentTypes.TmTaxi.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmIncomeRequestAttachList.Url,
                ContentTypes.TmAttach.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmPossessionReasonBookList.Url,
                ContentTypes.TmPossessionReason.Name);
            ListHelpers.MakeContentTypeDefault(ctx, Lists.TmCancellationReasonBookList.Url,
                ContentTypes.TmCancellationReason.Name);
        }

        public static void CreateBcsFields(ClientContext ctx)
        {
            if (!WebHelpers.CheckFeatureActivation(ctx, new Guid(BcsModelConsts.CV5ListsFeatureId), FeatureScope.Web))
                throw new Exception(String.Format("Feature with id = {0} must be activated",
                    BcsModelConsts.CV5ListsFeatureId));

            IEnumerable<List> allLists = WebHelpers.GetWebLists(ctx);
            List incomeRequestList = ListHelpers.GetList(allLists, Lists.TmIncomeRequestList.Url);

            ListHelpers.AddFieldAsXmlToList(incomeRequestList, Fields.TmRequestAccountBcsLookupXml,
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(incomeRequestList, Fields.TmRequestContactBcsLookupXml,
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(incomeRequestList, Fields.TmRequestTrusteeBcsLookupXml,
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
        }

        public static void CreatePlumsailFields(ClientContext ctx)
        {
            if (!WebHelpers.CheckFeatureActivation(ctx, new Guid(PlumsailConsts.CrossSiteLookupFeatureId), FeatureScope.Site))
                throw new Exception(String.Format("Feature with id = {0} must be activated",
                    PlumsailConsts.CrossSiteLookupFeatureId));

            Guid webId = WebHelpers.GetWebId(ctx);
            IEnumerable<List> allLists = WebHelpers.GetWebLists(ctx);
            List taxiList = ListHelpers.GetList(allLists, Lists.TmTaxiList.Url);
            List outcomeRequestStateList = ListHelpers.GetList(allLists, Lists.TmOutcomeRequestStateList.Url);
            List incomeRequestAttachList = ListHelpers.GetList(allLists, Lists.TmIncomeRequestAttachList.Url);
            List incomeRequestList = ListHelpers.GetList(allLists, Lists.TmIncomeRequestList.Url);

            ListHelpers.AddFieldAsXmlToList(taxiList, PlumsailFields.TmIncomeRequestLookupXml.ToXml(),
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(outcomeRequestStateList, PlumsailFields.TmIncomeRequestLookupXml.ToXml(),
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(incomeRequestAttachList, PlumsailFields.TmIncomeRequestLookupXml.ToXml(),
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(incomeRequestList, PlumsailFields.TmIncomeRequestStateLookupXml.ToXml(),
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(incomeRequestList, PlumsailFields.TmIncomeRequestStateInternalLookupXml.ToXml(),
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
        }

        #endregion
    }
}
