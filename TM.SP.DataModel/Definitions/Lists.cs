using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace TM.SP.DataModel
{
    public static class Lists
    {
        public static ListDefinition TmIncomeRequestStateBookList = new ListDefinition()
        {
            Title               = "Состояния обращений (справочник)",
            Url                 = "IncomeRequestStateBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Справочник состояний обращения",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmIncomeRequestStateInternalBookList = new ListDefinition()
        {
            Title               = "Внутренние статусы обращений (справочник)",
            Url                 = "IncomeRequestStateInternalBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Справочник внутренних статусов обращения",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmDenyReasonBookList = new ListDefinition()
        {
            Title               = "Причины отказа (справочник)",
            Url                 = "DenyReasonBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Справочник причин отказа по обращениям, автомобилям и т. д.",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmPossessionReasonBookList = new ListDefinition()
        {
            Title               = "Основание владения (справочник)",
            Url                 = "PossessionReasonBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Справочник оснований владения",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmGovServiceSubTypeBookList = new ListDefinition()
        {
            Title               = "Подтип госуслуги (справочник)",
            Url                 = "GovServiceSubTypeBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Справочник подтипов госуслуги",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmCancellationReasonBookList = new ListDefinition()
        {
            Title               = "Причины аннулирования (справочник)",
            Url                 = "CancellationReasonBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Справочник причин аннулирования разрешений",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmIncomeRequestList = new ListDefinition()
        {
            Title               = "Обращения",
            Url                 = "IncomeRequestList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Обращения",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmOutcomeRequestTypeBookList = new ListDefinition()
        {
            Title               = "Тип межвед. запроса (справочник)",
            Url                 = "OutcomeRequestTypeBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Справочник типов межведомственных запросов",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmOutcomeRequestStateList = new ListDefinition()
        {
            Title               = "Состояние межвед. запросов",
            Url                 = "OutcomeRequestStateList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Список содержит статусы межведомственных запросов по обращениям",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmTaxiList = new ListDefinition()
        {
            Title               = "Транспортные средства",
            Url                 = "TaxiList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Транспортные средства",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmIncomeRequestAttachList = new ListDefinition()
        {
            Title               = "Документы обращения",
            Url                 = "IncomeRequestAttachList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Документы обращения",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmConfigurationList = new ListDefinition()
        {
            Title               = "Конфигурация",
            Url                 = "ConfigurationList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Элементы конфигурации системы",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmAttachLib = new ListDefinition()
        {
            Title               = "Документы",
            Url                 = "AttachLib",
            TemplateType        = BuiltInListTemplateTypeId.DocumentLibrary,
            Description         = "Библиотека документов обращений, транспортных средства и других сущностей",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmIdentityDocumentTypeBookList = new ListDefinition()
        {
            Title               = "Виды документов удост. личность",
            Url                 = "IdentityDocumentTypeBookList",
            TemplateType        = BuiltInListTemplateTypeId.GenericList,
            Description         = "Список видов документов, которые являются документами, удостоверяющими личность",
            ContentTypesEnabled = true
        };
    }
}
