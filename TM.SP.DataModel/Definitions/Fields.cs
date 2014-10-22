using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

using TM.ClientUtils.Bcs;

namespace TM.SP.DataModel
{
    public static class Fields
    {
        #region [Общие поля]
        /// <summary>
        /// Код сущности, используется практически во всех справочниках
        /// </summary>
        public static FieldDefinition TmServiceCode = new FieldDefinition()
        {
            Id           = new Guid("{698FA5A4-0DEA-41C2-8703-ABA7663ED01E}"),
            Title        = "Код",
            InternalName = "Tm_ServiceCode",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };
        /// <summary>
        /// GUID входящего запроса с обращением
        /// </summary>
        public static FieldDefinition TmMessageId = new FieldDefinition()
        {
            Id           = new Guid("{81ED1C39-71F7-4A39-A6F4-F1E9E8672FD1}"),
            Title        = "MessageId",
            InternalName = "Tm_MessageId",
            FieldType    = BuiltInFieldTypes.Guid,
            Group        = ModelConsts.ColumnsDefaultGroup
        };
        /// <summary>
        /// Lookup на обращение (BCS список)
        /// </summary>
        public static XElement TmRequestAccountBcsLookupXml = new XElement("Field",
            new XAttribute("Type"           , "BusinessData"),
            new XAttribute("Name"           , "Tm_RequestAccountBCSLookup"),
            new XAttribute("StaticName"     , "Tm_RequestAccountBCSLookup"),
            new XAttribute("DisplayName"    , "Заявитель ЮЛ"),
            new XAttribute("Required"       , "FALSE"),
            new XAttribute("ID"             , "{5C8E8BBB-6670-4ABF-84C1-F5C529BDDB75}"),
            new XAttribute("SystemInstance" , BcsModelConsts.CV5SystemName),
            new XAttribute("EntityNamespace", BcsModelConsts.CV5EntityNamespace),
            new XAttribute("EntityName"     , BcsModelConsts.CV5RequestAccountEntityName),
            new XAttribute("BdcField"       , "Title"),
            new XAttribute("Version"        , "1")
        );

        #endregion

        #region [Причина отказа]

        public static FieldDefinition TmUsageScopeInteger = new FieldDefinition()
        {
            Id           = new Guid("{A0D93632-BCF8-4BB3-99B4-9B553F41E6D9}"),
            Title        = "Область применения (код)",
            InternalName = "Tm_UsageScopeInteger",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        =  ModelConsts.ColumnsDefaultGroup
        };

        #endregion

        #region [Обращение - Общие поля]

        public static FieldDefinition TmRegNumber = new FieldDefinition()
        {
            Id           = new Guid("{CD4C9A50-D719-44AA-9458-A267A0F53B69}"),
            Title        = "Регистрационный номер",
            InternalName = "Tm_RegNumber",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmSingleNumber = new FieldDefinition()
        {
            Id           = new Guid("{3EC39AEB-1885-4BF8-835E-27D73A5F0C3A}"),
            Title        = "Единый номер",
            InternalName = "Tm_SingleNumber",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRegistrationDate = new FieldDefinition()
        {
            Id           = new Guid("{A9069860-C16D-435D-AAAF-2A73457323EB}"),
            Title        = "Дата регистрации",
            InternalName = "Tm_RegistrationDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmIncomeRequestForm = new FieldDefinition()
        {
            Id           = new Guid("{F2D73050-B448-42E0-A379-EFEE8440382C}"),
            Title        = "Форма обращения",
            InternalName = "Tm_IncomeRequestForm",
            FieldType    = BuiltInFieldTypes.Choice,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmComment = new FieldDefinition()
        {
            Id           = new Guid("{1A89ECD3-C1AE-4BF7-A3D5-DE319FC325CD}"),
            Title        = "Примечание",
            InternalName = "Tm_Comment",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmInstanceCounter = new FieldDefinition()
        {
            Id           = new Guid("{3BDCF1C0-E4E5-4746-938F-E7E48BFFB9F2}"),
            Title        = "Количество экземпляров",
            InternalName = "Tm_InstanceCounter",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRequestedDocumentPrice = new FieldDefinition()
        {
            Id           = new Guid("{915FBFA7-8BF4-4E15-8551-822272973B00}"),
            Title        = "Стоимость документа",
            Description  = "Стоимость государственной услуги по выдаче соответствующего документа",
            InternalName = "Tm_RequestedDocumentPrice",
            FieldType    = BuiltInFieldTypes.Currency,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmPrepareTargetDate = new FieldDefinition()
        {
            Id           = new Guid("{9DB8891C-FA1F-454F-A8AE-72F1BBF0947E}"),
            Title        = "Плановый срок подготовки",
            InternalName = "Tm_PrepareTargetDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOutputTargetDate = new FieldDefinition()
        {
            Id           = new Guid("{2A360234-965E-48C9-82E7-9C2C7EBC9B7B}"),
            Title        = "Плановый срок выдачи",
            InternalName = "Tm_OutputTargetDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmPrepareFactDate = new FieldDefinition()
        {
            Id           = new Guid("{8BB6996D-75F0-4F9A-96BF-780C66E0AAA2}"),
            Title        = "Дата фактической подготовки",
            InternalName = "Tm_PrepareFactDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOutputFactDate = new FieldDefinition()
        {
            Id           = new Guid("{D5F291C7-DF29-4946-8BC2-AF6B98634F25}"),
            Title        = "Дата фактической выдачи",
            InternalName = "Tm_OutputFactDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmRefuseDate = new FieldDefinition()
        {
            Id           = new Guid("{BBA808D0-0946-47A4-AE61-4C2D7AE7E0F4}"),
            Title        = "Дата отказа",
            InternalName = "Tm_RefuseDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };
        /// <summary>
        /// Ссылка на физическое лицо (BCS список)
        /// </summary>
        public static XElement TmRequestContactBcsLookupXml = new XElement("Field",
            new XAttribute("Type"           , "BusinessData"),
            new XAttribute("Name"           , "Tm_RequestContactBCSLookup"),
            new XAttribute("StaticName"     , "Tm_RequestContactBCSLookup"),
            new XAttribute("DisplayName"    , "Заявитель ФЛ"),
            new XAttribute("Required"       , "FALSE"),
            new XAttribute("ID"             , "{83BFA335-F62A-4CD0-BC3C-A314C871CD86}"),
            new XAttribute("SystemInstance" , BcsModelConsts.CV5SystemName),
            new XAttribute("EntityNamespace", BcsModelConsts.CV5EntityNamespace),
            new XAttribute("EntityName"     , BcsModelConsts.CV5RequestContactEntityName),
            new XAttribute("BdcField"       , "Title"),
            new XAttribute("Version"        , "1")
        );
        /// <summary>
        /// Ссылка на доверенное лицо (BCS список)
        /// </summary>
        public static XElement TmRequestTrusteeBcsLookupXml = new XElement("Field",
            new XAttribute("Type"           , "BusinessData"),
            new XAttribute("Name"           , "Tm_RequestTrusteeBcsLookup"),
            new XAttribute("StaticName"     , "Tm_RequestTrusteeBcsLookup"),
            new XAttribute("DisplayName"    , "Доверенное лицо"),
            new XAttribute("Required"       , "FALSE"),
            new XAttribute("ID"             , "{15AFC5D5-B6C3-468D-9452-02044362BC72}"),
            new XAttribute("SystemInstance" , BcsModelConsts.CV5SystemName),
            new XAttribute("EntityNamespace", BcsModelConsts.CV5EntityNamespace),
            new XAttribute("EntityName"     , BcsModelConsts.CV5RequestContactEntityName),
            new XAttribute("BdcField"       , "Title"),
            new XAttribute("Version"        , "1")
        );

        #endregion

        #region [Обращение - Вычислимые поля для поиска]

        public static FieldDefinition TmIncomeRequestTaxiModels = new FieldDefinition()
        {
            Id           = new Guid("{9868A537-2060-423E-A80E-BDF63B4E4C71}"),
            Title        = "Модель ТС",
            InternalName = "Tm_IncomeRequestTaxiModels",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestTaxiBrands = new FieldDefinition()
        {
            Id           = new Guid("{072E7AD2-AA6E-4E9D-8521-FE4F0AF7F59A}"),
            Title        = "Марка ТС",
            InternalName = "Tm_IncomeRequestTaxiBrands",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestTaxiStateNumbers = new FieldDefinition()
        {
            Id           = new Guid("{F0C20389-2D59-42EC-959D-72A787504F10}"),
            Title        = "Гос. номер ТС",
            InternalName = "Tm_IncomeRequestTaxiStateNumbers",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestTaxiYears = new FieldDefinition()
        {
            Id           = new Guid("{1E1E1BA1-9629-4751-9C52-88C8D5597587}"),
            Title        = "Год выпуска ТС",
            InternalName = "Tm_IncomeRequestTaxiYears",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestTaxiLastToDates = new FieldDefinition()
        {
            Id           = new Guid("{9B31F82C-BAAD-4D31-8972-082155C8D055}"),
            Title        = "Дата последнего ТО ТС",
            InternalName = "Tm_IncomeRequestTaxiLastToDates",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestDeclarantNames = new FieldDefinition()
        {
            Id           = new Guid("{C0C5A54F-D1F3-4759-B126-77ED3B23C683}"),
            Title        = "Заявитель",
            InternalName = "Tm_IncomeRequestDeclarantNames",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestDeclarantAddresses = new FieldDefinition()
        {
            Id           = new Guid("{BA8B57AE-8B69-4563-AFDC-D708219BFE24}"),
            Title        = "Адрес заявителя",
            InternalName = "Tm_IncomeRequestDeclarantAddress",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestDeclarantINNs = new FieldDefinition()
        {
            Id           = new Guid("{36769DEA-7B5C-417A-8750-D451FDD97DE1}"),
            Title        = "ИНН заявителя",
            InternalName = "Tm_IncomeRequestDeclarantINNs",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestTrusteeNames = new FieldDefinition()
        {
            Id           = new Guid("{2280CE8E-BEE9-43E0-8693-AB615C13D49B}"),
            Title        = "ФИО представителя",
            InternalName = "Tm_IncomeRequestTrusteeNames",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestTrusteeAddresses = new FieldDefinition()
        {
            Id           = new Guid("{2D610FDA-FC5F-4A4E-AB32-08413C42F251}"),
            Title        = "Адрес представителя",
            InternalName = "Tm_IncomeRequestTrusteeAddresses",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestTrusteeINNs = new FieldDefinition()
        {
            Id           = new Guid("{08F25938-B3BB-4E71-8745-E119C99398E1}"),
            Title        = "ИНН представителя",
            InternalName = "Tm_IncomeRequestTrusteeINNs",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        public static FieldDefinition TmIncomeRequestIdentityDocs = new FieldDefinition()
        {
            Id           = new Guid("{6E7BDDAA-7877-45F3-B111-31A1B5445EA4}"),
            Title        = "Документ удост. личность",
            InternalName = "Tm_IncomeRequestIdentityDocs",
            FieldType    = BuiltInFieldTypes.MultiChoice,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        #endregion

        #region [Обращение - Переоформление разрешения]

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

        #endregion

        #region [Обращение - Аннулирование разрешения]

        public static FieldDefinition TmCancellationReasonOther = new FieldDefinition()
        {
            Id = new Guid("{4D5425EA-6A45-4459-814B-03091DCFF1EC}"),
            Title = "Иное основание",
            InternalName = "Tm_CancellationReasonOther",
            FieldType = BuiltInFieldTypes.Text,
            Group = ModelConsts.ColumnsDefaultGroup
        };

        #endregion

        #region [Состояние межведомственного запроса]

        public static FieldDefinition TmOutputDate = new FieldDefinition()
        {
            Id           = new Guid("{5B704A5C-6D14-420B-87FB-F12CE37A57E3}"),
            Title        = "Дата отправки",
            InternalName = "Tm_OutputDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmErrorDescription = new FieldDefinition()
        {
            Id           = new Guid("{B1BF90A8-3363-42E3-AC84-9D08A75C82FD}"),
            Title        = "Ошибка",
            InternalName = "Tm_ErrorDescription",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAnswerReceived = new FieldDefinition()
        {
            Id           = new Guid("{AAA21EAC-CD45-4ABC-A818-D6F9AC252389}"),
            Title        = "Ответ получен",
            InternalName = "Tm_AnswerReceived",
            FieldType    = BuiltInFieldTypes.Boolean,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        #endregion

        #region [Транспортное средство]

        public static FieldDefinition TmTaxiBrand = new FieldDefinition()
        {
            Id           = new Guid("{DE4AF847-5CAC-44E8-AABD-91D171029CEE}"),
            Title        = "Марка",
            InternalName = "Tm_TaxiBrand",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiModel = new FieldDefinition()
        {
            Id           = new Guid("{43D77EDA-DAED-4CEC-AF22-2C77AAD2570C}"),
            Title        = "Модель",
            InternalName = "Tm_TaxiModel",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiYear = new FieldDefinition()
        {
            Id           = new Guid("{C401E37A-6B02-44FF-A1F8-8F49FB10E775}"),
            Title        = "Год выпуска",
            InternalName = "Tm_TaxiYear",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiLastToDate = new FieldDefinition()
        {
            Id           = new Guid("{08AB9C28-C84B-466A-85DF-CC9459D84B94}"),
            Title        = "Дата ТО",
            InternalName = "Tm_TaxiLastToDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiStateNumber = new FieldDefinition()
        {
            Id           = new Guid("{5CFC136A-A1DF-4FE3-8FCD-8C1EFD4CC5D2}"),
            Title        = "Гос. номер",
            InternalName = "Tm_TaxiStateNumber",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiPrevStateNumber = new FieldDefinition()
        {
            Id           = new Guid("{F3A1ECDF-2D32-4CB9-9836-9F64926C4247}"),
            Title        = "Предыдущий гос. номер",
            InternalName = "Tm_TaxiPrevStateNumber",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiLeasingContractDetails = new FieldDefinition()
        {
            Id           = new Guid("{9FE88B9B-3700-4F38-BA29-78C42CA61D14}"),
            Title        = "Договор лизинга",
            InternalName = "Tm_LeasingContractDetails",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBodyYellow = new FieldDefinition()
        {
            Id           = new Guid("{393287C1-2411-4B63-AB39-A868B47A23DC}"),
            Title        = "Цвет кузова желтый",
            InternalName = "Tm_TaxiBodyYellow",
            FieldType    = BuiltInFieldTypes.Boolean,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBodyColor = new FieldDefinition()
        {
            Id           = new Guid("{FF8BE731-BC7B-475F-BC82-7C9EC7373B14}"),
            Title        = "Цвет кузова",
            InternalName = "Tm_TaxiBodyColor",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBodyColor2 = new FieldDefinition()
        {
            Id           = new Guid("{D8B3E2E5-0DBA-49EC-B206-7BD5D68B4098}"),
            Title        = "Цвет не из списка",
            InternalName = "Tm_TaxiBodyColor2",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiStateNumberYellow = new FieldDefinition()
        {
            Id           = new Guid("{12FEA6BA-8108-4C52-99F0-9C436E31D830}"),
            Title        = "Желтый номер",
            InternalName = "Tm_TaxiStateNumberYellow",
            FieldType    = BuiltInFieldTypes.Boolean,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiTaxometer = new FieldDefinition()
        {
            Id           = new Guid("{7F815DCB-C977-42F3-B05B-FA9008EAB240}"),
            Title        = "Таксометр",
            InternalName = "Tm_TaxiTaxometer",
            FieldType    = BuiltInFieldTypes.Boolean,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiGps = new FieldDefinition()
        {
            Id           = new Guid("{4D02796C-C8E5-4EC6-A797-B2593946EC2B}"),
            Title        = "GPS/Глонасс",
            InternalName = "Tm_TaxiGps",
            FieldType    = BuiltInFieldTypes.Boolean,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiDecision = new FieldDefinition()
        {
            Id           = new Guid("{75A5609D-216E-40E0-80CF-CAD60601F8DC}"),
            Title        = "Решение",
            InternalName = "Tm_TaxiDecision",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBlankNo = new FieldDefinition()
        {
            Id           = new Guid("{A931C164-9C2E-4CF3-BC72-E0D7D8C6709B}"),
            Title        = "Номер бланка",
            InternalName = "Tm_TaxiBlankNo",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiPrevLicenseNumber = new FieldDefinition()
        {
            Id           = new Guid("{11C7D20E-3AC3-4274-8D75-E74D562CEB80}"),
            Title        = "Номер ранее выд. разрешения",
            InternalName = "Tm_TaxiPrevLicenseNumber",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiPrevLicenseDate = new FieldDefinition()
        {
            Id           = new Guid("{A7027823-AF1A-477C-82D1-191403E896D2}"),
            Title        = "Дата выдачи ранее выд. разрешения",
            InternalName = "Tm_TaxiPrevLicenseDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiInfoOld = new FieldDefinition()
        {
            Id           = new Guid("{4D3D4F0C-FA52-46EF-9B05-F34F70918941}"),
            Title        = "Сведения о ТС по старому разрешению",
            InternalName = "Tm_TaxiInfoOld",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiStsDetails = new FieldDefinition()
        {
            Id           = new Guid("{B5A821CB-C4D3-4F2A-B429-957D3D4AA78E}"),
            Title        = "Реквизиты СТС",
            InternalName = "Tm_TaxiStsDetails",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiOwner = new FieldDefinition()
        {
            Id           = new Guid("{684231CA-C50B-4267-9938-4F82F5333B2F}"),
            Title        = "Владелец",
            InternalName = "Tm_TaxiOwner",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiBodyType = new FieldDefinition()
        {
            Id           = new Guid("{CBEBACBE-8CB0-48D5-A076-96B38C3D6741}"),
            Title        = "Тип кузова",
            InternalName = "Tm_TaxiBodyType",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiStatus = new FieldDefinition()
        {
            Id           = new Guid("{EC0F7911-4803-44A1-BD53-E1358A1583B7}"),
            Title        = "Статус",
            InternalName = "Tm_TaxiStatus",
            FieldType    = BuiltInFieldTypes.Choice,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmTaxiDenyComment = new FieldDefinition()
        {
            Id           = new Guid("{E1FA4BEB-2D50-4C5E-B9E8-4E85F3D0343A}"),
            Title        = "Комментарий к отказу",
            InternalName = "Tm_TaxiDenyComment",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        #endregion

        #region [Документ обращения]

        public static FieldDefinition TmAttachType = new FieldDefinition()
        {
            Id           = new Guid("{6D8F99C9-4FFE-4457-AA6F-51A11287D988}"),
            Title        = "Вид документа",
            InternalName = "Tm_AttachType",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDocSubType = new FieldDefinition()
        {
            Id           = new Guid("{4758A95E-A9B0-40B5-B6A6-A0A3E9AAF9A0}"),
            Title        = "Подтип документа",
            InternalName = "Tm_AttachSubType",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachValidityPeriod = new FieldDefinition()
        {
            Id           = new Guid("{F010124C-9032-452A-96C1-EC203020A8E5}"),
            Title        = "Срок действия",
            InternalName = "Tm_AttachValidityPeriod",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachListCount = new FieldDefinition()
        {
            Id           = new Guid("{910339C1-6BA1-490D-8E9D-543F8D55F179}"),
            Title        = "Кол-во листов",
            InternalName = "Tm_AttachListCount",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachCopyCount = new FieldDefinition()
        {
            Id           = new Guid("{8C54AD22-BABA-49A1-A890-6DA26F706EBE}"),
            Title        = "Кол-во экземпляров",
            InternalName = "Tm_AttachCopyCount",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDivisionCode = new FieldDefinition()
        {
            Id           = new Guid("{8C202C79-9465-41DA-A975-92DA0FCC8CCF}"),
            Title        = "Код подразделения",
            InternalName = "Tm_AttachDivisionCode",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDocNumber = new FieldDefinition()
        {
            Id           = new Guid("{B5A836DB-6BF4-409C-851D-E45519D8A146}"),
            Title        = "Номер документа",
            InternalName = "Tm_AttachDocNumber",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDocDate = new FieldDefinition()
        {
            Id           = new Guid("{16287AA5-5740-4C88-97D7-CE988F9D8000}"),
            Title        = "Дата документа",
            InternalName = "Tm_AttachDocDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachDocSerie = new FieldDefinition()
        {
            Id           = new Guid("{4FE75132-E040-4F54-8526-0AC72044D504}"),
            Title        = "Серия документа",
            InternalName = "Tm_AttachDocSerie",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAttachWhoSigned = new FieldDefinition()
        {
            Id           = new Guid("{AE01C815-7204-4D60-89AA-78646A7412D9}"),
            Title        = "Кто подписал",
            InternalName = "Tm_AttachWhoSigned",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };


        public static FieldDefinition TmAttachSingleStrDocName = new FieldDefinition()
        {
            Id           = new Guid("{51AB412A-761D-4116-AD42-B38F08FC02DE}"),
            Title        = "Строка наименования документа",
            InternalName = "Tm_AttachSingleStrDocName",
            Description  = "Поле для хранения наименования документа. Используется для поиска, вычисляется в eventreceiver'ах",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsCalcGroup
        };
        #endregion

        #region [Список конфигураций системы]

        public static FieldDefinition TmConfigurationCategory = new FieldDefinition()
        {
            Id           = new Guid("{C99E5E6C-234C-4C99-9D68-9089B84C53BA}"),
            Title        = "Категория",
            InternalName = "Tm_ConfigurationCategory",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ServiceColumnsGroup
        };

        public static FieldDefinition TmConfigurationValue = new FieldDefinition()
        {
            Id           = new Guid("{5EDE105C-9028-4003-A32C-D848878172B0}"),
            Title        = "Значение параметра",
            InternalName = "Tm_ConfigurationValue",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ServiceColumnsGroup
        };

        public static FieldDefinition TmConfigurationDescr = new FieldDefinition()
        {
            Id           = new Guid("{B361690F-3427-43E0-B41D-43EAE2AB9CC5}"),
            Title        = "Описание",
            InternalName = "Tm_ConfigurationDescr",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ServiceColumnsGroup
        };

        #endregion

        #region [Вложение для документа обращения]

        public static FieldDefinition TmCentralDocStoreUrl = new FieldDefinition()
        {
            Id           = new Guid("{86B3E450-C9D8-4656-935E-4F99FADBB242}"),
            Title        = "Ссылка ЦХЭД",
            InternalName = "Tm_CentralDocStoreUrl",
            FieldType    = BuiltInFieldTypes.URL,
            Group        = ModelConsts.ServiceColumnsGroup
        };

        #endregion

        #region [Виды документов, удост. личность]

        public static FieldDefinition TmIdentityDocTypeComment = new FieldDefinition()
        {
            Id           = new Guid("{A9DE7C34-63C4-4677-80CA-0A0DF36B9C38}"),
            Title        = "Комментарий к виду ДокУдЛ",
            InternalName = "Tm_IdentityDocTypeComment",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        #endregion

        #region [Состояния обращений]

        public static FieldDefinition TmIncomeRequestSystemUpdateAvailable = new FieldDefinition()
        {
            Id           = new Guid("{511DEBBB-3B6F-4218-ABBE-B0AD961EAFDC}"),
            Title        = "Обновление обращения разрешено",
            InternalName = "Tm_IncomeRequestSysUpdateAvail",
            FieldType    = BuiltInFieldTypes.Boolean,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmIncomeRequestSysUpdAvailText = new FieldDefinition()
        {
            Id           = new Guid("{EC3A867E-5EE2-4C58-A1F0-CED6217289B4}"),
            Title        = "Обновление обращения разрешено (выч.)",
            InternalName = "Tm_IncomeRequestSysUpdAvailText",
            FieldType    = BuiltInFieldTypes.Calculated,
            Group        = ModelConsts.ColumnsCalcGroup
        };

        #endregion

        #region [Разрешение]

        public static FieldDefinition TmBlankSeries = new FieldDefinition()
        {
            Id           = new Guid("{BA2CD0A1-40AA-4118-B023-EC3B8E0D41BA}"),
            Title        = "Серия бланка",
            InternalName = "Tm_BlankSeries",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmBlankNo = new FieldDefinition()
        {
            Id           = new Guid("{543E9DB8-EC5F-466A-8BCF-324E86E65101}"),
            Title        = "Номер бланка",
            InternalName = "Tm_BlankNo",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOrganizationName = new FieldDefinition()
        {
            Id           = new Guid("{F45C91A8-0801-4CFC-9E74-6B47CA93A3A8}"),
            Title        = "Наименование организации",
            InternalName = "Tm_OrganizationName",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOrgOgrn = new FieldDefinition()
        {
            Id           = new Guid("{30A710D1-8ADA-4397-83D0-B276E7ADF870}"),
            Title        = "ОГРН",
            InternalName = "Tm_OrgOgrn",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOrgInn = new FieldDefinition()
        {
            Id           = new Guid("{E6620411-C231-46C5-9BCF-2D5BCCAC92F9}"),
            Title        = "ИНН",
            InternalName = "Tm_OrgInn",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmOrgLfb = new FieldDefinition()
        {
            Id           = new Guid("{D2E5C464-F2B7-48C4-9374-19B5E7F8FDEC}"),
            Title        = "Организационно-правовая форма",
            InternalName = "Tm_OrgLfb",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmJuridicalAddress = new FieldDefinition()
        {
            Id           = new Guid("{B6748CF9-A34D-4012-8459-C549388B709E}"),
            Title        = "Юридический адрес",
            InternalName = "Tm_JuridicalAddress",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmPhoneNumber = new FieldDefinition()
        {
            Id           = new Guid("{65C0073C-FA3D-4B25-AC9B-14607AC1AE5C}"),
            Title        = "Номер телефона",
            InternalName = "Tm_PhoneNumber",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmAddContactData = new FieldDefinition()
        {
            Id           = new Guid("{28A80B63-8D0A-433D-BD24-0D01962335D0}"),
            Title        = "Доп. контактные данные",
            InternalName = "Tm_AddContactData",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmJuridicalPersonAbbreviation = new FieldDefinition()
        {
            Id           = new Guid("{F2A95693-A71A-42D7-9856-06CCA567AF43}"),
            Title        = "Аббревиатура ЮЛ",
            InternalName = "Tm_JuridicalPersonAbbreviation",
            FieldType    = BuiltInFieldTypes.Text,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseOutputDate = new FieldDefinition()
        {
            Id           = new Guid("{71EB2275-9BE3-4CD0-81F6-AC45C1FE6ABF}"),
            Title        = "Дата выдачи",
            InternalName = "Tm_LicenseOutputDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseTillDate = new FieldDefinition()
        {
            Id           = new Guid("{78CF58D5-9825-4650-8B81-75A008889171}"),
            Title        = "Срок действия (до)",
            InternalName = "Tm_LicenseTillDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseTillSuspensionDate = new FieldDefinition()
        {
            Id           = new Guid("{07674A82-AFBF-4B9F-8760-F6676558037D}"),
            Title        = "Срок приостановки (до)",
            InternalName = "Tm_LicenseTillSuspensionDate",
            FieldType    = BuiltInFieldTypes.DateTime,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseCancellationReason = new FieldDefinition()
        {
            Id           = new Guid("{078DBA35-673A-4256-897F-954F479EF2B4}"),
            Title        = "Причина аннулирования",
            InternalName = "Tm_LicenseCancellationReason",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseSuspensionReason = new FieldDefinition()
        {
            Id           = new Guid("{EAF745C0-96B6-4D02-B54E-BC1210B459D6}"),
            Title        = "Причина приостановки",
            InternalName = "Tm_LicenseSuspensionReason",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseChangeReason = new FieldDefinition()
        {
            Id           = new Guid("{05E54335-D48A-4B20-8AF8-AF8E6F85550E}"),
            Title        = "Причина изменений",
            InternalName = "Tm_LicenseChangeReason",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseInvalidReason = new FieldDefinition()
        {
            Id           = new Guid("{669315FA-61D3-4F7F-A61D-2FD1D2788E84}"),
            Title        = "Причина признания недействительным",
            InternalName = "TmLicenseInvalidReason",
            FieldType    = BuiltInFieldTypes.Note,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static  FieldDefinition TmLicenseStatus = new FieldDefinition()
        {
            Id           = new Guid("{2E1509C3-D5AC-4181-8520-D48C2FAC91C9}"),
            Title        = "Статус",
            InternalName = "Tm_LicenseStatus",
            FieldType    = BuiltInFieldTypes.Choice,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        public static FieldDefinition TmLicenseExternalId = new FieldDefinition()
        {
            Id           = new Guid("{02D2608B-B4F1-4CDE-BB97-430FD93DE862}"),
            Title        = "Внешний ИД",
            InternalName = "Tm_LicenseExternalId",
            FieldType    = BuiltInFieldTypes.Integer,
            Group        = ModelConsts.ColumnsDefaultGroup
        };

        /// <summary>
        /// Ссылка на разрешение (BCS список)
        /// </summary>
        public static XElement TmLicenseAllViewBcsLookupXml = new XElement("Field",
            new XAttribute("Type", "BusinessData"),
            new XAttribute("Name", "Tm_LicenseAllViewBcsLookup"),
            new XAttribute("StaticName", "Tm_LicenseAllViewBcsLookup"),
            new XAttribute("DisplayName", "Ссылка на внешний список"),
            new XAttribute("BaseRenderingType", "Text"),
            new XAttribute("Required", "FALSE"),
            new XAttribute("ID", "{6EFF9171-6245-4C52-B5A1-9038E219D1BD}"),
            new XAttribute("SystemInstance", BcsModelConsts.TaxiV2SystemName),
            new XAttribute("EntityNamespace", BcsModelConsts.TaxiV2EntityNamespace),
            new XAttribute("EntityName", BcsModelConsts.TaxiV2LicenseAllEntityName),
            new XAttribute("BdcField", "Title"),
            new XAttribute("SecondaryFieldBdcNames", SecondaryFieldNamesHelper.Encode(new string[] { "IsLast" })),
            new XAttribute("SecondaryFieldWssNames", SecondaryFieldNamesHelper.Encode(new string[] { "LicenseAllView_IsLast" })),
            new XAttribute("SecondaryFieldsWssStaticNames", SecondaryFieldNamesHelper.Encode(new string[] { "LicenseAllView_IsLast" })),
            new XAttribute("Version", "1")
        );

        #endregion
    }
}
