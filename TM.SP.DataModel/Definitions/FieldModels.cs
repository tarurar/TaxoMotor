using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace TM.SP.DataModel
{
    public static class FieldModels
    {
        #region properties

        public static FieldDefinition TmServiceCode = new FieldDefinition()
        {
            Id = new Guid("{698FA5A4-0DEA-41C2-8703-ABA7663ED01E}"),
            Title = "Код",
            InternalName = "Tm_ServiceCode",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmUsageScopeInteger = new FieldDefinition()
        {
            Id = new Guid("{A0D93632-BCF8-4BB3-99B4-9B553F41E6D9}"),
            Title = "Область применения (код)",
            InternalName = "Tm_UsageScopeInteger",
            FieldType = BuiltInFieldTypes.Integer,
            Group =  ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRegNumber = new FieldDefinition()
        {
            Id = new Guid("{CD4C9A50-D719-44AA-9458-A267A0F53B69}"),
            Title = "Регистрационный номер",
            InternalName = "Tm_RegNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmSingleNumber = new FieldDefinition()
        {
            Id = new Guid("{3EC39AEB-1885-4BF8-835E-27D73A5F0C3A}"),
            Title = "Единый номер",
            InternalName = "Tm_SingleNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmIncomeRequestStateLookup = new FieldDefinition()
        {
            Id = new Guid("{DC820D1F-F672-48AE-890E-6B784422E9A9}"),
            Title = "Состояние обращения",
            InternalName = "Tm_IncomeRequestStateLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmIncomeRequestStateInternalLookup = new FieldDefinition()
        {
            Id = new Guid("{136E87D0-C4AA-4710-9E54-5BE31EFE6BCB}"),
            Title = "Внутренний статус обращения",
            InternalName = "Tm_IncomeRequestStateInternalLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRegistrationDate = new FieldDefinition()
        {
            Id = new Guid("{A9069860-C16D-435D-AAAF-2A73457323EB}"),
            Title = "Дата регистрации",
            InternalName = "Tm_RegistrationDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmIncomeRequestForm = new FieldDefinition()
        {
            Id = new Guid("{F2D73050-B448-42E0-A379-EFEE8440382C}"),
            Title = "Форма обращения",
            InternalName = "Tm_IncomeRequestForm",
            FieldType = BuiltInFieldTypes.Choice,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmDenyReasonLookup = new FieldDefinition()
        {
            Id = new Guid("{C6837DD1-0075-4223-BDC3-DEEE57334491}"),
            Title = "Причина отказа",
            InternalName = "Tm_DenyReasonLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmComment = new FieldDefinition()
        {
            Id = new Guid("{1A89ECD3-C1AE-4BF7-A3D5-DE319FC325CD}"),
            Title = "Примечание",
            InternalName = "Tm_Comment",
            FieldType = BuiltInFieldTypes.Note,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRequestedDocument = new FieldDefinition()
        {
            Id = new Guid("{83ED0D5C-C1D6-42BC-B922-855D3B4E22A7}"),
            Title = "Запрашиваемый документ",
            InternalName = "Tm_RequestedDocument",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmInstanceCounter = new FieldDefinition()
        {
            Id = new Guid("{3BDCF1C0-E4E5-4746-938F-E7E48BFFB9F2}"),
            Title = "Количество экземпляров",
            InternalName = "Tm_InstanceCounter",
            FieldType = BuiltInFieldTypes.Integer,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRequestedDocumentPrice = new FieldDefinition()
        {
            Id = new Guid("{915FBFA7-8BF4-4E15-8551-822272973B00}"),
            Title = "Стоимость документа",
            Description = "Стоимость государственной услуги по выдаче соответствующего документа",
            InternalName = "Tm_RequestedDocumentPrice",
            FieldType = BuiltInFieldTypes.Currency,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmPrepareTargetDate = new FieldDefinition()
        {
            Id = new Guid("{9DB8891C-FA1F-454F-A8AE-72F1BBF0947E}"),
            Title = "Плановый срок подготовки",
            InternalName = "Tm_PrepareTargetDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOutputTargetDate = new FieldDefinition()
        {
            Id = new Guid("{2A360234-965E-48C9-82E7-9C2C7EBC9B7B}"),
            Title = "Плановый срок выдачи",
            InternalName = "Tm_OutputTargetDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmPrepareFactDate = new FieldDefinition()
        {
            Id = new Guid("{8BB6996D-75F0-4F9A-96BF-780C66E0AAA2}"),
            Title = "Дата фактической подготовки",
            InternalName = "Tm_PrepareFactDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOutputFactDate = new FieldDefinition()
        {
            Id = new Guid("{D5F291C7-DF29-4946-8BC2-AF6B98634F25}"),
            Title = "Дата фактической выдачи",
            InternalName = "Tm_OutputFactDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRefuseDate = new FieldDefinition()
        {
            Id = new Guid("{BBA808D0-0946-47A4-AE61-4C2D7AE7E0F4}"),
            Title = "Дата отказа",
            InternalName = "Tm_RefuseDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmMessageId = new FieldDefinition()
        {
            Id = new Guid("{81ED1C39-71F7-4A39-A6F4-F1E9E8672FD1}"),
            Title = "MessageId",
            InternalName = "Tm_MessageId",
            FieldType = BuiltInFieldTypes.Guid,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static XElement TmRequestAccountBcsLookupXml = new XElement("Field",
            new XAttribute("Type", "BusinessData"),
            new XAttribute("Name", "Tm_RequestAccountBCSLookup"),
            new XAttribute("StaticName", "Tm_RequestAccountBCSLookup"),
            new XAttribute("DisplayName", "Заявитель ЮЛ"),
            new XAttribute("Required", "FALSE"),
            new XAttribute("ID", "{5C8E8BBB-6670-4ABF-84C1-F5C529BDDB75}"),
            new XAttribute("SystemInstance", BcsModelConsts.CV5SystemName),
            new XAttribute("EntityNamespace", BcsModelConsts.CV5EntityNamespace),
            new XAttribute("EntityName", BcsModelConsts.CV5RequestAccountEntityName),
            new XAttribute("BdcField", "Title"),
            new XAttribute("Version", "1")
        );

        public static XElement TmRequestContactBcsLookupXml = new XElement("Field",
            new XAttribute("Type", "BusinessData"),
            new XAttribute("Name", "Tm_RequestContactBCSLookup"),
            new XAttribute("StaticName", "Tm_RequestContactBCSLookup"),
            new XAttribute("DisplayName", "Заявитель ФЛ"),
            new XAttribute("Required", "FALSE"),
            new XAttribute("ID", "{83BFA335-F62A-4CD0-BC3C-A314C871CD86}"),
            new XAttribute("SystemInstance", BcsModelConsts.CV5SystemName),
            new XAttribute("EntityNamespace", BcsModelConsts.CV5EntityNamespace),
            new XAttribute("EntityName", BcsModelConsts.CV5RequestContactEntityName),
            new XAttribute("BdcField", "Title"),
            new XAttribute("Version", "1")
        );

        public static XElement TmRequestTrusteeBcsLookupXml = new XElement("Field",
            new XAttribute("Type", "BusinessData"),
            new XAttribute("Name", "Tm_RequestTrusteeBcsLookup"),
            new XAttribute("StaticName", "Tm_RequestTrusteeBcsLookup"),
            new XAttribute("DisplayName", "Доверенное лицо"),
            new XAttribute("Required", "FALSE"),
            new XAttribute("ID", "{15AFC5D5-B6C3-468D-9452-02044362BC72}"),
            new XAttribute("SystemInstance", BcsModelConsts.CV5SystemName),
            new XAttribute("EntityNamespace", BcsModelConsts.CV5EntityNamespace),
            new XAttribute("EntityName", BcsModelConsts.CV5RequestContactEntityName),
            new XAttribute("BdcField", "Title"),
            new XAttribute("Version", "1")
        );

        public static FieldDefinition TmOutputRequestTypeLookup = new FieldDefinition()
        {
            Id = new Guid("{C6CBD012-0605-4A82-8432-18D787DB8E2A}"),
            Title = "Тип запроса",
            InternalName = "Tm_OutputRequestTypeLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOutputDate = new FieldDefinition()
        {
            Id = new Guid("{5B704A5C-6D14-420B-87FB-F12CE37A57E3}"),
            Title = "Дата отправки",
            InternalName = "Tm_OutputDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmErrorDescription = new FieldDefinition()
        {
            Id = new Guid("{B1BF90A8-3363-42E3-AC84-9D08A75C82FD}"),
            Title = "Ошибка",
            InternalName = "Tm_ErrorDescription",
            FieldType = BuiltInFieldTypes.Note,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmIncomeRequestLookup = new FieldDefinition()
        {
            Id = new Guid("{5388EA4A-9E56-4782-9786-E8249CC2BFDD}"),
            Title = "Обращение",
            InternalName = "Tm_IncomeRequestLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBrand = new FieldDefinition()
        {
            Id = new Guid("{DE4AF847-5CAC-44E8-AABD-91D171029CEE}"),
            Title = "Марка",
            InternalName = "Tm_TaxiBrand",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiModel = new FieldDefinition()
        {
            Id = new Guid("{43D77EDA-DAED-4CEC-AF22-2C77AAD2570C}"),
            Title = "Модель",
            InternalName = "Tm_TaxiModel",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiYear = new FieldDefinition()
        {
            Id = new Guid("{C401E37A-6B02-44FF-A1F8-8F49FB10E775}"),
            Title = "Год выпуска",
            InternalName = "Tm_TaxiYear",
            FieldType = BuiltInFieldTypes.Integer,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiLastToDate = new FieldDefinition()
        {
            Id = new Guid("{08AB9C28-C84B-466A-85DF-CC9459D84B94}"),
            Title = "Дата ТО",
            InternalName = "Tm_TaxiLastToDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiStateNumber = new FieldDefinition()
        {
            Id = new Guid("{5CFC136A-A1DF-4FE3-8FCD-8C1EFD4CC5D2}"),
            Title = "Гос. номер",
            InternalName = "Tm_TaxiStateNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiPrevStateNumber = new FieldDefinition()
        {
            Id = new Guid("{F3A1ECDF-2D32-4CB9-9836-9F64926C4247}"),
            Title = "Предыдущий гос. номер",
            InternalName = "Tm_TaxiPrevStateNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiLeasingContractDetails = new FieldDefinition()
        {
            Id = new Guid("{9FE88B9B-3700-4F38-BA29-78C42CA61D14}"),
            Title = "Договор лизинга",
            InternalName = "Tm_LeasingContractDetails",
            FieldType = BuiltInFieldTypes.Note,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBodyYellow = new FieldDefinition()
        {
            Id = new Guid("{393287C1-2411-4B63-AB39-A868B47A23DC}"),
            Title = "Цвет кузова желтый",
            InternalName = "Tm_TaxiBodyYellow",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBodyColor = new FieldDefinition()
        {
            Id = new Guid("{FF8BE731-BC7B-475F-BC82-7C9EC7373B14}"),
            Title = "Цвет кузова",
            InternalName = "Tm_TaxiBodyColor",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBodyColor2 = new FieldDefinition()
        {
            Id = new Guid("{D8B3E2E5-0DBA-49EC-B206-7BD5D68B4098}"),
            Title = "Цвет не из списка",
            InternalName = "Tm_TaxiBodyColor2",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiStateNumberYellow = new FieldDefinition()
        {
            Id = new Guid("{12FEA6BA-8108-4C52-99F0-9C436E31D830}"),
            Title = "Желтый номер",
            InternalName = "Tm_TaxiStateNumberYellow",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiTaxometer = new FieldDefinition()
        {
            Id = new Guid("{7F815DCB-C977-42F3-B05B-FA9008EAB240}"),
            Title = "Таксометр",
            InternalName = "Tm_TaxiTaxometer",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiGps = new FieldDefinition()
        {
            Id = new Guid("{4D02796C-C8E5-4EC6-A797-B2593946EC2B}"),
            Title = "GPS/Глонасс",
            InternalName = "Tm_TaxiGps",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiDecision = new FieldDefinition()
        {
            Id = new Guid("{75A5609D-216E-40E0-80CF-CAD60601F8DC}"),
            Title = "Решение",
            InternalName = "Tm_TaxiDecision",
            FieldType = BuiltInFieldTypes.Integer,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBlankNo = new FieldDefinition()
        {
            Id = new Guid("{A931C164-9C2E-4CF3-BC72-E0D7D8C6709B}"),
            Title = "Номер бланка",
            InternalName = "Tm_TaxiBlankNo",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiPrevLicenseNumber = new FieldDefinition()
        {
            Id = new Guid("{11C7D20E-3AC3-4274-8D75-E74D562CEB80}"),
            Title = "Номер ранее выд. разрешения",
            InternalName = "Tm_TaxiPrevLicenseNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiPrevLicenseDate = new FieldDefinition()
        {
            Id = new Guid("{A7027823-AF1A-477C-82D1-191403E896D2}"),
            Title = "Дата выдачи ранее выд. разрешения",
            InternalName = "Tm_TaxiPrevLicenseDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiInfoOld = new FieldDefinition()
        {
            Id = new Guid("{4D3D4F0C-FA52-46EF-9B05-F34F70918941}"),
            Title = "Сведения о ТС по старому разрешению",
            InternalName = "Tm_TaxiInfoOld",
            FieldType = BuiltInFieldTypes.Note,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiStsDetails = new FieldDefinition()
        {
            Id = new Guid("{B5A821CB-C4D3-4F2A-B429-957D3D4AA78E}"),
            Title = "Реквизиты СТС",
            InternalName = "Tm_TaxiStsDetails",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachType = new FieldDefinition()
        {
            Id = new Guid("{6D8F99C9-4FFE-4457-AA6F-51A11287D988}"),
            Title = "Вид документа",
            InternalName = "Tm_AttachType",
            FieldType = BuiltInFieldTypes.Integer,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDocNumber = new FieldDefinition()
        {
            Id = new Guid("{B5A836DB-6BF4-409C-851D-E45519D8A146}"),
            Title = "Номер документа",
            InternalName = "Tm_AttachDocNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDocDate = new FieldDefinition()
        {
            Id = new Guid("{16287AA5-5740-4C88-97D7-CE988F9D8000}"),
            Title = "Дата документа",
            InternalName = "Tm_AttachDocDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDocSerie = new FieldDefinition()
        {
            Id = new Guid("{4FE75132-E040-4F54-8526-0AC72044D504}"),
            Title = "Серия документа",
            InternalName = "Tm_AttachDocSerie",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachWhoSigned = new FieldDefinition()
        {
            Id = new Guid("{AE01C815-7204-4D60-89AA-78646A7412D9}"),
            Title = "Кто подписал",
            InternalName = "Tm_AttachWhoSigned",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmPossessionReasonLookup = new FieldDefinition()
        {
            Id = new Guid("{BFA645C4-7FEC-417D-B67A-13C2B9DA6268}"),
            Title = "Основание владения",
            InternalName = "Tm_PossessionReasonLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRenewalReason_StateNumber = new FieldDefinition()
        {
            Id = new Guid("{5D0392C3-1B3C-4EE9-8859-B62DCA477DDB}"),
            Title = "Изменение гос. рег-го знака",
            InternalName = "Tm_RenewalReason_StateNumber",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRenewalReason_AddressCompany = new FieldDefinition()
        {
            Id = new Guid("{8FDC55E8-1569-4B76-BD74-6CB32E6335CB}"),
            Title = "Изменение адреса ЮЛ",
            InternalName = "Tm_RenewalReason_AddressCompany",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRenewalReason_IdentityCard = new FieldDefinition()
        {
            Id = new Guid("{E431F5F0-5E4E-4C18-8E43-77DF7B87137E}"),
            Title = "Изменение данных документа удост. личность ИП",
            InternalName = "Tm_RenewalReason_IdentityCard",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRenewalReason_AddressPerson = new FieldDefinition()
        {
            Id = new Guid("{7008E18A-127B-452B-BE68-B6796D96DB08}"),
            Title = "Изменение адреса регистрации по месту жит. ИП",
            InternalName = "Tm_RenewalReason_AddressPerson",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRenewalReason_NameCompany = new FieldDefinition()
        {
            Id = new Guid("{5CE81552-2411-4937-B1AE-C3CA2CE6B005}"),
            Title = "Изменение наименования ЮЛ",
            InternalName = "Tm_RenewalReason_NameCompany",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRenewalReason_ReorgCompany = new FieldDefinition()
        {
            Id = new Guid("{3EA8641A-190A-45E2-8015-0778991E9532}"),
            Title = "Реорганизация ЮЛ",
            InternalName = "Tm_RenewalReason_ReorgCompany",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRenewalReason_NamePerson = new FieldDefinition()
        {
            Id = new Guid("{5AEC0DA6-F0E9-4C62-BB71-5D5A9D5F91D4}"),
            Title = "Изменение ФИО ИП ",
            InternalName = "Tm_RenewalReason_NamePerson",
            FieldType = BuiltInFieldTypes.Boolean,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmCancellationReasonLookup = new FieldDefinition()
        {
            Id = new Guid("{E72D2328-1A5D-443C-8C6D-A354C6F6FC86}"),
            Title = "Причина аннулирования",
            InternalName = "Tm_CancellationReasonLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmCancellationReasonOther = new FieldDefinition()
        {
            Id = new Guid("{4D5425EA-6A45-4459-814B-03091DCFF1EC}"),
            Title = "Иное основание",
            InternalName = "Tm_CancellationReasonOther",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        #endregion
    }
}
