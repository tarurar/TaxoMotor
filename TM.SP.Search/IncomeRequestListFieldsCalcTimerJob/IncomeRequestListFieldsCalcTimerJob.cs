using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.BusinessData.MetadataModel;
using CamlexNET;

using TM.Utils;
using BcsCoordinateV5Model = TM.SP.BCSModels.CoordinateV5;

namespace TM.SP.Search
{
    public static class StaticCaml
    {
        public static string JoinClause()
        {
            return new XElement("Join",
                    new XAttribute("Type", "LEFT"),
                    new XAttribute("ListAlias", "IncomeRequestStateBookList"),
                        new XElement("Eq",
                            new XElement("FieldRef",
                                new XAttribute("Name", "Tm_IncomeRequestStateLookup"),
                                new XAttribute("RefType", "Id")),
                            new XElement("FieldRef",
                                new XAttribute("List", "IncomeRequestStateBookList"),
                                new XAttribute("Name", "ID")))).ToString();
        }

        public static string ProjectedFieldsClause()
        {
            return new XElement("Field",
                        new XAttribute("Name", "LookupState"),
                        new XAttribute("Type", "Lookup"),
                        new XAttribute("List", "IncomeRequestStateBookList"),
                        new XAttribute("ShowField", "Tm_IncomeRequestSysUpdAvailText")).ToString();
        }

        public static string ViewFieldsClause()
        {
            var viewFields = new XElement[] {
                new XElement("FieldRef", new XAttribute("Name", "Title")),
                new XElement("FieldRef", new XAttribute("Name", "LookupState"))
            };

            return String.Join("", viewFields.Select(s => s.ToString()));
        }

        public static string QueryClause()
        {
            return new XElement("Where",
                    new XElement("And",
                        new XElement("IsNotNull",
                            new XElement("FieldRef",
                                new XAttribute("Name", "LookupState"))),
                        new XElement("Eq",
                            new XElement("FieldRef",
                                new XAttribute("Name", "LookupState")),
                            new XElement("Value",
                                new XAttribute("Type", "Text")) { Value = "1" }))).ToString();
        }
    }
    class IncomeRequestListFieldsCalcTimerJob : SPJobDefinition
    {
        #region resource strings

        private static readonly string FeatureId = "{3bf96d98-8209-463c-959c-6161cc16095d}";

        public static readonly string TaxiListsFeatureId = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        private static readonly string UpdateIncomeRequestItemErrorFmt = GetFeatureLocalizedResource("UpdateItemErrorFmt");

        #endregion

        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }

        public IncomeRequestListFieldsCalcTimerJob() : base() {}

        public IncomeRequestListFieldsCalcTimerJob(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public IncomeRequestListFieldsCalcTimerJob(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public override void Execute(Guid targetInstanceId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            foreach (SPSite siteCollection in webApp.Sites)
            {
                SPWeb web = siteCollection.RootWeb;

                if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                    web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                {
                    try
                    {
                        var context = SPServiceContext.GetContext(siteCollection);
                        using (var scope = new SPServiceContextScope(context))
                        {
                            ProcessCalculation(web);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(String.Format(GetFeatureLocalizedResource("GeneralErrorFmt"), ex.Message,
                            web.Title));
                    }
                }
            }
        }

        private void ProcessCalculation(SPWeb web)
        {
            var incomeRequests = GetAvailableIncomeRequests(web);
            var incomeRequestList = web.GetListOrBreak("Lists/IncomeRequestList");
            foreach (SPListItem item in incomeRequests)
            {
                var updateItem = incomeRequestList.GetItemById(item.ID);
                CalculateIncomeRequestSearchFields(updateItem, web);
                try
                {
                    updateItem.SystemUpdate(false);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format(UpdateIncomeRequestItemErrorFmt, item.ID, ex.Message), ex);
                }
            }
        }

        private void CalculateIncomeRequestSearchFields(SPListItem item, SPWeb web)
        {
            DoCalculateTaxiSearchFields(item, web);
            DoCalculateDeclarantSearchFields(item, web);
            DoCalculateTrusteeSearchFields(item, web);
            DoCalculateOtherSearchFields(item, web);
        }

        private void DoCalculateOtherSearchFields(SPListItem item, SPWeb web)
        {
            var docList = web.GetListOrBreak("Lists/IncomeRequestAttachList");
            var docTypeList = web.GetListOrBreak("Lists/IdentityDocumentTypeBookList");

            SPListItemCollection items = docList.GetItems(new SPQuery()
            {
                Query = Camlex.Query().Where(x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)item["ID"].ToString()).ToString(),
                ViewAttributes = "Scope='Recursive'"
            });

            var idDocuments = new SPFieldMultiChoiceValue();

            foreach (SPListItem docItem in items)
            {
                if (docItem["Tm_AttachType"] != null)
                {
                    SPListItemCollection docTypes = docTypeList.GetItems(new SPQuery()
                    {
                        Query = Camlex.Query().Where(x => (int)x["Tm_ServiceCode"] == (int)docItem["Tm_AttachType"]).ToString(),
                        ViewAttributes = "Scope='Recursive'"
                    });

                    if ((docTypes.Count > 0) && docItem["Tm_AttachSingleStrDocName"] != null)
                    {
                        idDocuments.Add(docItem["Tm_AttachSingleStrDocName"].ToString());
                    }
                }
            }

            item["Tm_IncomeRequestIdentityDocs"] = idDocuments;
        }

        private void DoCalculateTrusteeSearchFields(SPListItem item, SPWeb web)
        {
            var id = item["Tm_RequestContactBCSLookup"] != null ? BCS.GetBCSFieldLookupId(item, "Tm_RequestContactBCSLookup") : null;
            var name    = new SPFieldMultiChoiceValue();
            var address = new SPFieldMultiChoiceValue();
            var inn     = new SPFieldMultiChoiceValue();

            if (id != null)
            {
                IEntity contentType = BCS.GetEntity(SPServiceContext.Current, String.Empty, BCS.LOBRequestSystemNamespace, "RequestContact");
                List<object> args = new List<object>();
                args.Add((int)id);
                var parameters = args.ToArray();
                var entity = (BcsCoordinateV5Model.RequestContact)BCS.GetDataFromMethod(BCS.LOBRequestSystemName,
                    contentType, "ReadRequestContactItem", MethodInstanceType.SpecificFinder, ref parameters);

                if (!String.IsNullOrEmpty(entity.Title))
                    name.Add(entity.Title);
                if (!String.IsNullOrEmpty(entity.SingleStrRegAddress))
                    address.Add(entity.SingleStrRegAddress);
                if (!String.IsNullOrEmpty(entity.SingleStrFactAddress))
                    address.Add(entity.SingleStrFactAddress);
                if (!String.IsNullOrEmpty(entity.SingleStrBirthAddress))
                    address.Add(entity.SingleStrBirthAddress);
                if (!String.IsNullOrEmpty(entity.Inn))
                    inn.Add(entity.Inn);
            }

            item["Tm_IncomeRequestTrusteeNames"]     = name;
            item["Tm_IncomeRequestTrusteeAddresses"] = address;
            item["Tm_IncomeRequestTrusteeINNs"]      = inn;
        }

        private void DoCalculateDeclarantSearchFields(SPListItem item, SPWeb web)
        {
            var id = item["Tm_RequestAccountBCSLookup"] != null ? BCS.GetBCSFieldLookupId(item, "Tm_RequestAccountBCSLookup") : null;
            var name    = new SPFieldMultiChoiceValue();
            var address = new SPFieldMultiChoiceValue();
            var inn     = new SPFieldMultiChoiceValue();
            var fname   = new SPFieldMultiChoiceValue();
            var ogrn    = new SPFieldMultiChoiceValue();
            var orgCode = new SPFieldMultiChoiceValue();

            if (id != null)
            {
                IEntity contentType = BCS.GetEntity(SPServiceContext.Current, String.Empty, BCS.LOBRequestSystemNamespace, "RequestAccount");
                List<object> args = new List<object>();
                args.Add((int)id);
                var parameters = args.ToArray();
                var entity = (BcsCoordinateV5Model.RequestAccount)BCS.GetDataFromMethod(BCS.LOBRequestSystemName,
                    contentType, "ReadRequestAccountItem", MethodInstanceType.SpecificFinder, ref parameters);

                if (!String.IsNullOrEmpty(entity.Name))
                    name.Add(entity.Name);
                if (!String.IsNullOrEmpty(entity.SingleStrPostalAddress))
                    address.Add(entity.SingleStrPostalAddress);
                if (!String.IsNullOrEmpty(entity.SingleStrFactAddress))
                    address.Add(entity.SingleStrFactAddress);
                if (!String.IsNullOrEmpty(entity.Inn))
                    inn.Add(entity.Inn);
                if (!String.IsNullOrEmpty(entity.FullName))
                    fname.Add(entity.FullName);
                if (!String.IsNullOrEmpty(entity.Ogrn))
                    ogrn.Add(entity.Ogrn);
                if (!String.IsNullOrEmpty(entity.OrgFormCode))
                    orgCode.Add(entity.OrgFormCode);
            }

            item["Tm_IncomeRequestDeclarantNames"]   = name;
            item["Tm_IncomeRequestDeclarantAddress"] = address;
            item["Tm_IncomeRequestDeclarantINNs"]    = inn;
            item["Tm_IncomeRequestDeclarantFNames"]  = fname;
            item["Tm_IncomeRequestDeclarantOgrns"]   = ogrn;
            item["Tm_IncomeRequestOrgFormCodes"]     = orgCode;
        }

        private void DoCalculateTaxiSearchFields(SPListItem item, SPWeb web)
        {
            var taxiList = web.GetListOrBreak("Lists/TaxiList");

            SPListItemCollection items = taxiList.GetItems(new SPQuery()
            {
                Query = Camlex.Query().Where(x => x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)item["ID"].ToString()).ToString(),
                ViewAttributes = "Scope='Recursive'"
            });

            var models       = new SPFieldMultiChoiceValue();
            var brands       = new SPFieldMultiChoiceValue();
            var stateNumbers = new SPFieldMultiChoiceValue();
            var years        = new SPFieldMultiChoiceValue();
            var toDates      = new SPFieldMultiChoiceValue();

            foreach (SPListItem taxiItem in items)
            {
                if (taxiItem["Tm_TaxiModel"] != null)
                    models.Add(taxiItem["Tm_TaxiModel"].ToString());
                if (taxiItem["Tm_TaxiBrand"] != null)
                    brands.Add(taxiItem["Tm_TaxiBrand"].ToString());
                if (taxiItem["Tm_TaxiStateNumber"] != null)
                    stateNumbers.Add(taxiItem["Tm_TaxiStateNumber"].ToString());
                if (taxiItem["Tm_TaxiYear"] != null)
                    years.Add(taxiItem["Tm_TaxiYear"].ToString());
                if (taxiItem["Tm_TaxiLastToDate"] != null)
                    toDates.Add(DateTime.Parse(taxiItem["Tm_TaxiLastToDate"].ToString()).ToString("dd.MM.yyyy"));
            }

            item["Tm_IncomeRequestTaxiModels"] = models;
            item["Tm_IncomeRequestTaxiBrands"] = brands;
            item["Tm_IncomeRequestTaxiStateNumbers"] = stateNumbers;
            item["Tm_IncomeRequestTaxiYears"] = years;
            item["Tm_IncomeRequestTaxiLastToDates"] = toDates;
        }

        private SPListItemCollection GetAvailableIncomeRequests(SPWeb web)
        {
            var list = web.GetListOrBreak("Lists/IncomeRequestList");
            var query = GetIncomeRequestCamlQuery();

            return list.GetItems(query);
        }

        private static SPQuery GetIncomeRequestCamlQuery()
        {
            return new SPQuery()
            {
                Joins           = StaticCaml.JoinClause(),
                ProjectedFields = StaticCaml.ProjectedFieldsClause(),
                ViewFields      = StaticCaml.ViewFieldsClause(),
                Query           = StaticCaml.QueryClause(),
                ViewAttributes  = "Scope='Recursive'"
            };
        }

    }
}
