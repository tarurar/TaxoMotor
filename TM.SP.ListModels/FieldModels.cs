﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace TM.SP.ListModels
{
    public static class FieldModels
    {
        #region consts

        private const string DefaultGroup = "TaxoMotor Common Columns";

        #endregion

        #region properties

        public static FieldDefinition TmServiceCode = new FieldDefinition()
        {
            Id = new Guid("{698FA5A4-0DEA-41C2-8703-ABA7663ED01E}"),
            Title = "Код",
            InternalName = "Tm_ServiceCode",
            FieldType = BuiltInFieldTypes.Text,
            Group = DefaultGroup
        };

        public static FieldDefinition TmUsageScopeInteger = new FieldDefinition()
        {
            Id = new Guid("{A0D93632-BCF8-4BB3-99B4-9B553F41E6D9}"),
            Title = "Область применения (код)",
            InternalName = "Tm_UsageScopeInteger",
            FieldType = BuiltInFieldTypes.Integer,
            Group =  DefaultGroup
        };

        public static FieldDefinition TmRegNumber = new FieldDefinition()
        {
            Id = new Guid("{CD4C9A50-D719-44AA-9458-A267A0F53B69}"),
            Title = "Регистрационный номер",
            InternalName = "Tm_RegNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = DefaultGroup
        };

        public static FieldDefinition TmSingleNumber = new FieldDefinition()
        {
            Id = new Guid("{3EC39AEB-1885-4BF8-835E-27D73A5F0C3A}"),
            Title = "Единый номер",
            InternalName = "Tm_SingleNumber",
            FieldType = BuiltInFieldTypes.Text,
            Group = DefaultGroup
        };

        public static FieldDefinition TmIncomeRequestStateLookup = new FieldDefinition()
        {
            Id = new Guid("{DC820D1F-F672-48AE-890E-6B784422E9A9}"),
            Title = "Состояние обращения",
            InternalName = "Tm_IncomeRequestStateLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = DefaultGroup
        };

        public static FieldDefinition TmIncomeRequestStateInternalLookup = new FieldDefinition()
        {
            Id = new Guid("{136E87D0-C4AA-4710-9E54-5BE31EFE6BCB}"),
            Title = "Внутренний статус обращения",
            InternalName = "Tm_IncomeRequestStateInternalLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = DefaultGroup
        };

        public static FieldDefinition TmRegistrationDate = new FieldDefinition()
        {
            Id = new Guid("{A9069860-C16D-435D-AAAF-2A73457323EB}"),
            Title = "Дата регистрации",
            InternalName = "Tm_RegistrationDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = DefaultGroup
        };

        public static FieldDefinition TmIncomeRequestForm = new FieldDefinition()
        {
            Id = new Guid("{F2D73050-B448-42E0-A379-EFEE8440382C}"),
            Title = "Форма обращения",
            InternalName = "Tm_IncomeRequestForm",
            FieldType = BuiltInFieldTypes.Choice,
            Group = DefaultGroup
        };

        public static FieldDefinition TmDenyReasonLookup = new FieldDefinition()
        {
            Id = new Guid("{C6837DD1-0075-4223-BDC3-DEEE57334491}"),
            Title = "Причина отказа",
            InternalName = "Tm_DenyReasonLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = DefaultGroup
        };

        public static FieldDefinition TmComment = new FieldDefinition()
        {
            Id = new Guid("{1A89ECD3-C1AE-4BF7-A3D5-DE319FC325CD}"),
            Title = "Примечание",
            InternalName = "Tm_Comment",
            FieldType = BuiltInFieldTypes.Note,
            Group = DefaultGroup
        };

        public static FieldDefinition TmRequestedDocument = new FieldDefinition()
        {
            Id = new Guid("{83ED0D5C-C1D6-42BC-B922-855D3B4E22A7}"),
            Title = "Запрашиваемый документ",
            InternalName = "Tm_RequestedDocument",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = DefaultGroup
        };

        public static FieldDefinition TmInstanceCounter = new FieldDefinition()
        {
            Id = new Guid("{3BDCF1C0-E4E5-4746-938F-E7E48BFFB9F2}"),
            Title = "Количество экземпляров",
            InternalName = "Tm_InstanceCounter",
            FieldType = BuiltInFieldTypes.Integer,
            Group = DefaultGroup
        };

        public static FieldDefinition TmRequestedDocumentPrice = new FieldDefinition()
        {
            Id = new Guid("{915FBFA7-8BF4-4E15-8551-822272973B00}"),
            Title = "Стоимость документа",
            Description = "Стоимость государственной услуши по выдаче соответствующего документа",
            InternalName = "Tm_RequestedDocumentPrice",
            FieldType = BuiltInFieldTypes.Currency,
            Group = DefaultGroup
        };

        public static FieldDefinition TmPrepareTargetDate = new FieldDefinition()
        {
            Id = new Guid("{9DB8891C-FA1F-454F-A8AE-72F1BBF0947E}"),
            Title = "Плановый срок подготовки",
            InternalName = "Tm_PrepareTargetDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = DefaultGroup
        };

        public static FieldDefinition TmOutputTargetDate = new FieldDefinition()
        {
            Id = new Guid("{2A360234-965E-48C9-82E7-9C2C7EBC9B7B}"),
            Title = "Плановый срок выдачи",
            InternalName = "Tm_OutputTargetDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = DefaultGroup
        };

        public static FieldDefinition TmPrepareFactDate = new FieldDefinition()
        {
            Id = new Guid("{8BB6996D-75F0-4F9A-96BF-780C66E0AAA2}"),
            Title = "Дата фактической подготовки",
            InternalName = "Tm_PrepareFactDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = DefaultGroup
        };

        public static FieldDefinition TmOutputFactDate = new FieldDefinition()
        {
            Id = new Guid("{D5F291C7-DF29-4946-8BC2-AF6B98634F25}"),
            Title = "Дата фактической выдачи",
            InternalName = "Tm_OutputFactDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = DefaultGroup
        };

        public static FieldDefinition TmRefuseDate = new FieldDefinition()
        {
            Id = new Guid("{BBA808D0-0946-47A4-AE61-4C2D7AE7E0F4}"),
            Title = "Дата отказа",
            InternalName = "Tm_RefuseDate",
            FieldType = BuiltInFieldTypes.DateTime,
            Group = DefaultGroup
        };

        public static FieldDefinition TmMessageId = new FieldDefinition()
        {
            Id = new Guid("{81ED1C39-71F7-4A39-A6F4-F1E9E8672FD1}"),
            Title = "MessageId",
            InternalName = "Tm_MessageId",
            FieldType = BuiltInFieldTypes.Guid,
            Group = DefaultGroup
        };

        public static XElement TmRequestAccountBcsLookupXml = new XElement("Field",
            new XAttribute("Type", "BusinessData"),
            new XAttribute("Name", "Tm_RequestAccountBCSLookup"),
            new XAttribute("StaticName", "Tm_RequestAccountBCSLookup"),
            new XAttribute("DisplayName", "Заявитель ЮЛ"),
            new XAttribute("Required", "FALSE"),
            new XAttribute("ID", "{5C8E8BBB-6670-4ABF-84C1-F5C529BDDB75}"),
            new XAttribute("SystemInstance", Consts.BcsCoordinateV5SystemName),
            new XAttribute("EntityNamespace", Consts.BcsCoordinateV5EntityNamespace),
            new XAttribute("EntityName", Consts.BcsRequestAccountEntityName),
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
            new XAttribute("SystemInstance", Consts.BcsCoordinateV5SystemName),
            new XAttribute("EntityNamespace", Consts.BcsCoordinateV5EntityNamespace),
            new XAttribute("EntityName", Consts.BcsRequestContactEntityName),
            new XAttribute("BdcField", "Title"),
            new XAttribute("Version", "1")
        );

        public static XElement TmRequestTrusteeBcsLookupXml = new XElement("Field",
            new XAttribute("Type", "BusinessData"),
            new XAttribute("Name", "Tm_RequestTrusteeBcsLookupXml"),
            new XAttribute("StaticName", "Tm_RequestTrusteeBcsLookupXml"),
            new XAttribute("DisplayName", "Доверенное лицо"),
            new XAttribute("Required", "FALSE"),
            new XAttribute("ID", "{15AFC5D5-B6C3-468D-9452-02044362BC72}"),
            new XAttribute("SystemInstance", Consts.BcsCoordinateV5SystemName),
            new XAttribute("EntityNamespace", Consts.BcsCoordinateV5EntityNamespace),
            new XAttribute("EntityName", Consts.BcsRequestContactEntityName),
            new XAttribute("BdcField", "Title"),
            new XAttribute("Version", "1")
        );


        #endregion
    }
}
