using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace TM.SP.ListModels
{
    public static class ContentTypeModels
    {
        #region consts

        private const string DefaultGroup = "TaxoMotor Common Content Types";

        #endregion

        #region properties

        public static ContentTypeDefinition TmIncomeRequestState = new ContentTypeDefinition()
        {
            Id = new Guid("{6CB4A60E-EBD0-4B3B-A91E-4608345C5E75}"),
            Description = "Состояние обращения",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_IncomeRequestState"
        };

        public static ContentTypeDefinition TmIncomeRequestStateInternal = new ContentTypeDefinition()
        {
            Id = new Guid("{3564B675-F783-4756-8F52-CA8211319C48}"),
            Description = "Внутренний статус обращения",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_IncomeRequestStateInternal"
        };

        public static ContentTypeDefinition TmDenyReason = new ContentTypeDefinition()
        {
            Id = new Guid("{6BC8BFCE-F7E1-4FBD-9264-1D2DBA277CF3}"),
            Description = "Причина отказа",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_DenyReason"
        };

        public static ContentTypeDefinition TmPossessionReason = new ContentTypeDefinition()
        {
            Id = new Guid("{FCEEB6A9-6C9A-41C6-B413-3A8D6B363BAD}"),
            Description = "Основание владения",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_PossessionReason"
        };

        public static ContentTypeDefinition TmGovServiceSubType = new ContentTypeDefinition()
        {
            Id = new Guid("{F59F313E-2D74-4DF0-AE78-140DE2334B50}"),
            Description = "Тип запрашиваемого документа, подтип государственной услуги",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_GovServiceSubType"
        };

        public static ContentTypeDefinition TmIncomeRequest = new ContentTypeDefinition()
        {
            Id = new Guid("{84EDFCD7-5A7D-407C-B154-926E988C79D4}"),
            Description = "Обращение",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_IncomeRequest"
        };

        public static ContentTypeDefinition TmOutcomeRequestType = new ContentTypeDefinition()
        {
            Id = new Guid("{11599BE2-7066-449F-9A2A-138FCADB35FF}"),
            Description = "Тип межведомственного запроса",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_OutcomeRequestType"
        };

        public static ContentTypeDefinition TmOutcomeRequestState = new ContentTypeDefinition()
        {
            Id = new Guid("{0A9D8165-1B79-48E5-9F81-588F9AA06E39}"),
            Description = "Состояние межведомственного запроса",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_OutcomeRequestState"
        };

        public static ContentTypeDefinition TmTaxi = new ContentTypeDefinition()
        {
            Id = new Guid("{1F986B2F-EF60-442A-94BE-02010E33CCFC}"),
            Description = "Транспортное средство",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_Taxi"
        };

        public static ContentTypeDefinition TmAttach = new ContentTypeDefinition()
        {
            Id = new Guid("{2AC0EF4F-A67D-432A-AD10-8A350A38DD6E}"),
            Description = "Документ вложение",
            Group = DefaultGroup,
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Name = "Tm_Attach"
        };

        #endregion
    }
}
