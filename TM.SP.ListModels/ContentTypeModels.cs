using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

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

        #endregion
    }
}
