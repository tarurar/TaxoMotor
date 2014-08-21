using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace TM.SP.DataModel
{
    public static class ModelHandlers
    {
        #region methods

        public static void MakeContentTypesDefault(ClientContext ctx)
        {
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestStateBookList.Url,
                    ContentTypeModels.TmIncomeRequestState.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestStateInternalBookList.Url,
                ContentTypeModels.TmIncomeRequestStateInternal.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmDenyReasonBookList.Url,
                ContentTypeModels.TmDenyReason.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmGovServiceSubTypeBookList.Url,
                ContentTypeModels.TmGovServiceSubType.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmOutcomeRequestTypeBookList.Url,
                ContentTypeModels.TmOutcomeRequestType.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestList.Url,
                ContentTypeModels.TmNewIncomeRequest.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmOutcomeRequestStateList.Url,
                ContentTypeModels.TmOutcomeRequestState.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmTaxiList.Url,
                ContentTypeModels.TmTaxi.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmIncomeRequestAttachList.Url,
                ContentTypeModels.TmAttach.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmPossessionReasonBookList.Url,
                ContentTypeModels.TmPossessionReason.Name);
            ListHelpers.MakeContentTypeDefault(ctx, ListModels.TmCancellationReasonBookList.Url,
                ContentTypeModels.TmCancellationReason.Name);
        }

        public static void CreateBcsFields(ClientContext ctx)
        {
            if (!WebHelpers.CheckFeatureActivation(ctx, new Guid(BcsModelConsts.CV5ListsFeatureId)))
                throw new Exception(String.Format("Feature with id = {0} must be activated",
                    BcsModelConsts.CV5ListsFeatureId));

            IEnumerable<List> allLists = WebHelpers.GetWebLists(ctx);
            List incomeRequestList = ListHelpers.GetList(allLists, ListModels.TmIncomeRequestList.Url);

            ListHelpers.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestAccountBcsLookupXml,
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestContactBcsLookupXml,
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
            ListHelpers.AddFieldAsXmlToList(incomeRequestList, FieldModels.TmRequestTrusteeBcsLookupXml,
                AddFieldOptions.AddFieldInternalNameHint | AddFieldOptions.AddToAllContentTypes);
        }

        #endregion
    }
}
