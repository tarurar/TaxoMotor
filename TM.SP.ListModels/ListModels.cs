using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace TM.SP.ListModels
{
    public static class ListModels
    {
        public static ListDefinition TmIncomeRequestStateBookList = new ListDefinition()
        {
            Title = "Состояния обращений (справочник)",
            Url = "IncomeRequestStateBookList",
            TemplateType = (int)ListTemplateType.GenericList,
            Description = "Справочник состояний обращения",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmIncomeRequestStateInternalBookList = new ListDefinition()
        {
            Title = "Внутренние статусы обращений (справочник)",
            Url = "IncomeRequestStateInternalBookList",
            TemplateType = (int)ListTemplateType.GenericList,
            Description = "Справочник внутренних статусов обращения",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmDenyReasonBookList = new ListDefinition()
        {
            Title = "Причины отказа (справочник)",
            Url = "DenyReasonBookList",
            TemplateType = (int)ListTemplateType.GenericList,
            Description = "Справочник причин отказа по обращениям, автомобилям и т. д.",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmGovServiceSubTypeBookList = new ListDefinition()
        {
            Title = "Подтип госуслуги (справочник)",
            Url = "GovServiceSubTypeBookList",
            TemplateType = (int)ListTemplateType.GenericList,
            Description = "Справочник подтипов госуслуги",
            ContentTypesEnabled = true
        };

        public static ListDefinition TmIncomeRequestList = new ListDefinition()
        {
            Title = "Обращения",
            Url = "IncomeRequestList",
            TemplateType = (int)ListTemplateType.GenericList,
            Description = "Обращения",
            ContentTypesEnabled = true
        };
    }
}
