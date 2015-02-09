using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using TM.Utils;
using CamlexNET;

namespace TestSPConsole
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
                new XElement("FieldRef", new XAttribute("Name", "LookupState")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestIdentityDocs")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTrusteeNames")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTrusteeAddresses")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTrusteeINNs")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_RequestAccountBCSLookup")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_RequestContactBCSLookup")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestDeclarantNames")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestDeclarantAddress")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestDeclarantINNs")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTaxiModels")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTaxiBrands")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTaxiStateNumbers")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTaxiYears")),
                new XElement("FieldRef", new XAttribute("Name", "Tm_IncomeRequestTaxiLastToDates")),
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
    class Program
    {
        private static SPListItemCollection GetAvailableIncomeRequests(SPWeb web)
        {
            var list = web.GetListOrBreak("Lists/IncomeRequestList");
            var query = GetIncomeRequestCamlQuery();

            return list.GetItems(query);
        }
        private static SPQuery GetIncomeRequestCamlQuery()
        {
            return new SPQuery()
            {
                Joins = StaticCaml.JoinClause(),
                ProjectedFields = StaticCaml.ProjectedFieldsClause(),
                ViewFields = StaticCaml.ViewFieldsClause(),
                Query = StaticCaml.QueryClause()
            };
        }
        static void Main(string[] args)
        {
            using (SPSite site = new SPSite("http://77.95.132.133"))
            using (SPWeb web = site.OpenWeb())
            {
                var context = SPServiceContext.GetContext(site);
                using (var scope = new SPServiceContextScope(context))
                {
                    var list = web.GetListOrBreak("Lists/List");
                    
                    var day = new DateTime(2015, 2, 11);
                    var caml = Camlex.Query().Where(x => (DateTime)x["EventDate"] <= day && (DateTime)x["EndDate"] >= day).ToString();

                    var items = list.GetItems(new SPQuery
                    {
                        Query = caml
                    });

                    Console.OutputEncoding = System.Text.Encoding.Unicode;
                    foreach (SPListItem item in items)
                    {
                        Console.WriteLine(item.ID + ": " + item.Title);
                        Console.WriteLine(item["EventDate"].ToString());
                        Console.WriteLine(item["EndDate"].ToString());
                    }

                    Console.ReadKey();
                }
            }
        }
    }
}
