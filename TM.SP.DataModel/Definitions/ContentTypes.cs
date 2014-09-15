using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace TM.SP.DataModel
{
    public static class ContentTypes
    {
        #region properties

        public static ContentTypeDefinition TmIncomeRequestState = new ContentTypeDefinition()
        {
            Id                  = new Guid("{6CB4A60E-EBD0-4B3B-A91E-4608345C5E75}"),
            Description         = "Состояние обращения",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_IncomeRequestState"
        };

        public static ContentTypeDefinition TmIncomeRequestStateInternal = new ContentTypeDefinition()
        {
            Id                  = new Guid("{3564B675-F783-4756-8F52-CA8211319C48}"),
            Description         = "Внутренний статус обращения",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_IncomeRequestStateInternal"
        };

        public static ContentTypeDefinition TmDenyReason = new ContentTypeDefinition()
        {
            Id                  = new Guid("{6BC8BFCE-F7E1-4FBD-9264-1D2DBA277CF3}"),
            Description         = "Причина отказа",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_DenyReason"
        };

        public static ContentTypeDefinition TmPossessionReason = new ContentTypeDefinition()
        {
            Id                  = new Guid("{FCEEB6A9-6C9A-41C6-B413-3A8D6B363BAD}"),
            Description         = "Основание владения",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_PossessionReason"
        };

        public static ContentTypeDefinition TmGovServiceSubType = new ContentTypeDefinition()
        {
            Id                  = new Guid("{F59F313E-2D74-4DF0-AE78-140DE2334B50}"),
            Description         = "Тип запрашиваемого документа, подтип государственной услуги",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_GovServiceSubType"
        };

        public static ContentTypeDefinition TmCancellationReason = new ContentTypeDefinition()
        {
            Id                  = new Guid("{B96F909E-3372-4AA5-B3C0-0122D98F503D}"),
            Description         = "Причина аннулирования",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_CancellationReason"
        };

        public static ContentTypeDefinition TmIncomeRequest = new ContentTypeDefinition()
        {
            Id                  = new Guid("{84EDFCD7-5A7D-407C-B154-926E988C79D4}"),
            Description         = "Обращение",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_IncomeRequest"
        };

        public static ContentTypeDefinition TmNewIncomeRequest = new ContentTypeDefinition()
        {
            Id                  = new Guid("{7B255652-59C9-47D3-A3B0-DB6675C49045}"),
            Description         = "Новое",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = TmIncomeRequest.GetContentTypeId(),
            Name                = "Tm_NewIncomeRequest"
        };

        public static ContentTypeDefinition TmRenewIncomeRequest = new ContentTypeDefinition()
        {
            Id                  = new Guid("{5E0FFF91-4AF9-4BDE-83E5-815B85FB2620}"),
            Description         = "Переоформление",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = TmIncomeRequest.GetContentTypeId(),
            Name                = "Tm_RenewIncomeRequest"
        };

        public static ContentTypeDefinition TmCancelIncomeRequest = new ContentTypeDefinition()
        {
            Id                  = new Guid("{ADC40231-CE2E-4704-9E6F-7AD37025993F}"),
            Description         = "Аннулирование",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = TmIncomeRequest.GetContentTypeId(),
            Name                = "Tm_CancelIncomeRequest"
        };

        public static ContentTypeDefinition TmDuplicateIncomeRequest = new ContentTypeDefinition()
        {
            Id                  = new Guid("{465A29AC-9188-498A-8D1B-E36FA8C93F3F}"),
            Description         = "Выдача дубликата",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = TmIncomeRequest.GetContentTypeId(),
            Name                = "Tm_DuplicateIncomeRequest"
        };

        public static ContentTypeDefinition TmOutcomeRequestType = new ContentTypeDefinition()
        {
            Id                  = new Guid("{11599BE2-7066-449F-9A2A-138FCADB35FF}"),
            Description         = "Тип межведомственного запроса",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_OutcomeRequestType"
        };

        public static ContentTypeDefinition TmOutcomeRequestState = new ContentTypeDefinition()
        {
            Id                  = new Guid("{0A9D8165-1B79-48E5-9F81-588F9AA06E39}"),
            Description         = "Состояние межведомственного запроса",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_OutcomeRequestState"
        };

        public static ContentTypeDefinition TmTaxi = new ContentTypeDefinition()
        {
            Id                  = new Guid("{1F986B2F-EF60-442A-94BE-02010E33CCFC}"),
            Description         = "Транспортное средство",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_Taxi"
        };

        public static ContentTypeDefinition TmAttach = new ContentTypeDefinition()
        {
            Id                  = new Guid("{2AC0EF4F-A67D-432A-AD10-8A350A38DD6E}"),
            Description         = "Документ вложение",
            Group               = ModelConsts.ContentTypeDefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_Attach"
        };

        public static ContentTypeDefinition TmConfigurationEntry = new ContentTypeDefinition()
        {
            Id                  = new Guid("{E5ABD31B-14BD-49B2-A655-8ADCE3002FE3}"),
            Description         = "Элемент конфигурации",
            Group               = ModelConsts.ServiceContentTypeGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name                = "Tm_ConfigurationEntry"
        };

        public static ContentTypeDefinition TmAttachDoc = new ContentTypeDefinition()
        {
            Id                  = new Guid("{E9A1FF30-4430-4E84-AD5C-D1555D3446EF}"),
            Description         = "Документ вложение",
            Group               = ModelConsts.ServiceContentTypeGroup,
            ParentContentTypeId = BuiltInContentTypeId.Document,
            Name                = "Tm_AttachDoc"
        };

        #endregion
    }
}
