using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using System.Xml.Linq;

namespace TM.SP.DataModel.Plumsail
{
    public class CrossSiteLookupDefinition : FieldDefinition
    {
        #region consts

        private static readonly XNamespace @namespace = "Plumsail.CrossSiteLookup";

        private static readonly string JsLinkFmt =
            "~sitecollection/style library/plumsail/crosssitelookup/clienttemplates.js?field={0}";

        #endregion

        #region methods
        public CrossSiteLookupDefinition()
        {
            this.JsLink = "clienttemplates.js";
            this.nsRetrieveItemsUrlTemplate = @"function (term, page) {" + Environment.NewLine +
                                              @"    if (!term || term.length == 0) {" + Environment.NewLine +
                                              @"        return ""{WebUrl}/_api/web/lists('{ListId}')/items?$select=Id,{LookupField}&$orderby=Created desc&$top=10"";" + Environment.NewLine +
                                              @"    }" + Environment.NewLine +
                                              @"    return ""{WebUrl}/_api/web/lists('{ListId}')/items?$select=Id,{LookupField}&$orderby={LookupField}&$filter=startswith({LookupField}, '"" + encodeURIComponent(term) + ""')&$top=10"";" + Environment.NewLine +
                                              @"}";
            this.nsItemFormatResultTemplate = @"function(item) {" + Environment.NewLine +
                                              @"    return '<span class=""csl-option"">' + item[""{LookupField}""] + '</span>';" + Environment.NewLine +
                                              @"}";
            this.nsNewText = String.Empty;
            this.nsNewContentTypeId = String.Empty;
        }

        public XElement ToXml()
        {
            if (ListId == null || ListId == Guid.Empty)
                throw new Exception("Lookup list identifier cannot be null or empty");

            return new XElement("Field",
                new XAttribute(XNamespace.Xmlns + "csl", @namespace),
                new XAttribute("Type", BuiltInFieldTypes.Lookup),
                new XAttribute("Name", InternalName),
                new XAttribute("StaticName", InternalName),
                new XAttribute("DisplayName", Title),
                new XAttribute("Required", Required.ToString()),
                new XAttribute("ID", Id.ToString("B")),
                new XAttribute("Group", ModelConsts.ColumnsDefaultGroup),
                new XAttribute("JSLink", String.Format(JsLinkFmt, InternalName)),
                new XAttribute("Mult", Mult.ToString()),
                new XAttribute("List", ListId.ToString("B")),
                new XAttribute("SourceID", "http://schemas.microsoft.com/sharepoint/v3"),
                new XAttribute("ShowField", ShowField),
                new XAttribute(@namespace + "ShowNew", nsShowNew.ToString()),
                new XAttribute(@namespace + "RetrieveItemsUrlTemplate", nsRetrieveItemsUrlTemplate),
                new XAttribute(@namespace + "ItemFormatResultTemplate", nsItemFormatResultTemplate),
                new XAttribute(@namespace + "NewText", nsNewText),
                new XAttribute(@namespace + "NewContentType", nsNewContentTypeId)
            );
        }
        #endregion

        #region properties
        public string JsLink { get; set; }
        public bool Required { get; set; }
        public bool Mult { get; set; }
        public Guid ListId { get; set; }
        public string ShowField { get; set; }
        public bool nsShowNew { get; set; }
        public string nsRetrieveItemsUrlTemplate { get; set; }
        public string nsItemFormatResultTemplate { get; set; }
        public string nsNewText { get; set; }
        public string nsNewContentTypeId { get; set; }
        #endregion
    }

    public static class Fields
    {
        public static CrossSiteLookupDefinition TmIncomeRequestLookupXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{1A762415-2436-4C6D-AA85-43D680E5DEAC}"),
            Title = "Обращение",
            InternalName = "Tm_IncomeRequestLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = false
            //ListId needs to be set up in runtime
        };

        public static CrossSiteLookupDefinition TmIncomeRequestStateLookupXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{DC820D1F-F672-48AE-890E-6B784422E9A9}"),
            Title = "Состояние обращения",
            InternalName = "Tm_IncomeRequestStateLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = true,
            nsNewText = "Добавить состояние"
            //ListId needs to be set up in runtime
        };

        public static CrossSiteLookupDefinition TmIncomeRequestStateInternalLookupXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{136E87D0-C4AA-4710-9E54-5BE31EFE6BCB}"),
            Title = "Внутренний статус обращения",
            InternalName = "Tm_IncomeRequestStateIntLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = true,
            nsNewText = "Добавить статус"
            //ListId needs to be set up in runtime
        };



        public static CrossSiteLookupDefinition TmDenyReasonLookupXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{C6837DD1-0075-4223-BDC3-DEEE57334491}"),
            Title = "Причина отказа",
            InternalName = "Tm_DenyReasonLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = true,
            nsNewText = "Добавить причину отказа"
            //ListId needs to be set up in runtime
        };

        public static CrossSiteLookupDefinition TmRequestedDocumentXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{83ED0D5C-C1D6-42BC-B922-855D3B4E22A7}"),
            Title = "Запрашиваемый документ",
            InternalName = "Tm_RequestedDocument",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = true,
            nsNewText = "Добавить подтип госуслуги"
            //ListId needs to be set up in runtime
        };

        public static CrossSiteLookupDefinition TmOutputRequestTypeLookupXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{C6CBD012-0605-4A82-8432-18D787DB8E2A}"),
            Title = "Тип запроса",
            InternalName = "Tm_OutputRequestTypeLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = true,
            nsNewText = "Добавить подтип госуслуги"
            //ListId needs to be set up in runtime
        };

        public static CrossSiteLookupDefinition TmPossessionReasonLookupXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{BFA645C4-7FEC-417D-B67A-13C2B9DA6268}"),
            Title = "Основание владения",
            InternalName = "Tm_PossessionReasonLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = true,
            nsNewText = "Добавить основание"
            //ListId needs to be set up in runtime
        };

        public static CrossSiteLookupDefinition TmCancellationReasonLookupXml = new CrossSiteLookupDefinition()
        {
            Id = new Guid("{E72D2328-1A5D-443C-8C6D-A354C6F6FC86}"),
            Title = "Причина аннулирования",
            InternalName = "Tm_CancellationReasonLookup",
            FieldType = BuiltInFieldTypes.Lookup,
            Group = ModelConsts.ColumnsDefaultGroup,
            ShowField = "Title",
            Required = false,
            Mult = false,
            nsShowNew = true,
            nsNewText = "Добавить причину аннулирования"
        };
    }
}
