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
using TM.Utils.Sql;
using CamlexNET;
using System.Net.Http;
using System.Net;
using System.Dynamic;
using System.Diagnostics;
using System.Threading;
using TM.SP.AppPages.VirtualSigner;
using System.Security.Cryptography.X509Certificates;
using TM.SP.BCSModels.Taxi;
using Microsoft.BusinessData.MetadataModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TM.SP.AppPages;

namespace TestSPConsole
{

    public class TimeKeeper
    {
        public TimeSpan Measure(Action action)
        {
            var watch = new Stopwatch();
            watch.Start();
            action();
            return watch.Elapsed;
        }
    }

    public static class SPListItemExtensions
    {
        public static dynamic AsDyn(this SPListItem item)
        {
            return new DynamicListItem(item);
        }
    }

    public class DynamicListItem: DynamicObject
    {
        private SPListItem _source;
        public DynamicListItem(SPListItem source)
        {
            _source = source;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var field = _source.Fields.TryGetFieldByStaticName(binder.Name);
            if (field != null)
            {
                result = _source[binder.Name];
                return true;
            }

            result = null;
            return false;
        }
    }

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
        private static readonly string xml = "<ns2:ResponseData StatusCode=\"3\" xmlns:ns2=\"http://gibdd.ru/rev01\" xmlns=\"http://gibdd.ru/rev01\"><ns2:Results><ns2:Result><ns2:UnicNumber>77#77060006300013667912</ns2:UnicNumber><ns2:DocNumber>77МО6172788</ns2:DocNumber><ns2:BreachDateTime>2014-02-27T14:15:00.000Z</ns2:BreachDateTime><ns2:BreachPlace>МОСКВА Г. СТАЛЕВАРОВ Д.8\\22</ns2:BreachPlace><ns2:KOAPText>12.2Ч.1</ns2:KOAPText><ns2:ExecDepartment>1145630-ОБ ДПС ГИБДД УВД по ВАО ГУ МВД России по г. Москве </ns2:ExecDepartment><ns2:ExecutionState>ПОЛУЧЕНЫ СВЕДЕНИЯ ОБ ОПЛАТЕ ШТРАФА ОТ КАЗНАЧЕЙСТВА</ns2:ExecutionState><ns2:DecisionNumber>77МО6172788</ns2:DecisionNumber><ns2:DecisionDate>2014-02-27</ns2:DecisionDate><ns2:Penalty>ШТРАФ</ns2:Penalty><ns2:DecisionSumma>500</ns2:DecisionSumma><ns2:PostNum>77МО6172788</ns2:PostNum><ns2:WhoDecided>ГИБДД</ns2:WhoDecided><ns2:DepartmentDecided>1145630-ОБ ДПС ГИБДД УВД по ВАО ГУ МВД России по г. Москве </ns2:DepartmentDecided><ns2:SupplierBillID>188109877МО6172788Z3</ns2:SupplierBillID><ns2:roskaznaIn>1</ns2:roskaznaIn><ns2:VehicleCategory>В</ns2:VehicleCategory><ns2:VehicleModel>ФОЛЬКСВАГЕНРАSSАТ</ns2:VehicleModel><ns2:VehicleOwnerCategory>1</ns2:VehicleOwnerCategory><ns2:VehicleRegPoint>Х628ММ150</ns2:VehicleRegPoint><ns2:StateName>ИСПОЛНЕНО</ns2:StateName><ns2:EndDate>2014-03-08</ns2:EndDate><ns2:ExtensionData><ns2:ServiceProperties><ns2:BeginDate>2014-03-10</ns2:BeginDate><ns2:BreachSubject>1</ns2:BreachSubject></ns2:ServiceProperties></ns2:ExtensionData></ns2:Result><ns2:Result><ns2:UnicNumber>50#50000015110198611256</ns2:UnicNumber><ns2:DocNumber>18810150140816939295</ns2:DocNumber><ns2:BreachDateTime>2014-08-03T00:05:00.000Z</ns2:BreachDateTime><ns2:BreachPlace>1146 М-7 ВОЛГА, ГОРЬКОВСКОЕ Ш., 32 КМ. 800 М.</ns2:BreachPlace><ns2:KOAPText>12.9Ч.2</ns2:KOAPText><ns2:ExecDepartment>1146511-ЦАФАП ОДД ГИБДД ГУ МВД России по Московской области</ns2:ExecDepartment><ns2:ExecutionState>ВЫНЕСЕНО ПОСТАНОВЛЕНИЕ,ФОТО-ВИДЕОФИКСАЦИЯ(СТ. 28.6 Ч.3 КоАП РФ)</ns2:ExecutionState><ns2:DecisionNumber>18810150140816939295</ns2:DecisionNumber><ns2:DecisionDate>2014-08-16</ns2:DecisionDate><ns2:Penalty>ШТРАФ</ns2:Penalty><ns2:DecisionSumma>500,00</ns2:DecisionSumma><ns2:PostNum>18810150140816939295</ns2:PostNum><ns2:WhoDecided>ГИБДД</ns2:WhoDecided><ns2:DepartmentDecided>1146511-ЦАФАП ОДД ГИБДД ГУ МВД России по Московской области</ns2:DepartmentDecided><ns2:SupplierBillID>18810150140816939295</ns2:SupplierBillID><ns2:roskaznaIn>0</ns2:roskaznaIn><ns2:VehicleCategory>В</ns2:VehicleCategory><ns2:VehicleModel>ФОЛЬКСВАГЕНРАSSАТ</ns2:VehicleModel><ns2:VehicleOwnerCategory>1</ns2:VehicleOwnerCategory><ns2:VehicleRegPoint>Х628ММ150</ns2:VehicleRegPoint><ns2:StateName>ВЫНЕСЕНИЕ ПОСТАНОВЛЕНИЯ ПО ДЕЛУ ОБ АП, ПРЕДУСМОТРЕННОГО ГЛАВОЙ 12 КОАП И ЗАФИКСИРОВАННОГО С ПРИМЕНЕНИЕМ РАБОТАЮЩИХ В АВТОМАТИЧЕСКОМ РЕЖИМЕ СПЕЦИАЛЬНЫХ ТЕХНИЧЕСКИХ СРЕДСТВ (СТ. 28.6 Ч.3 КОАП РФ)</ns2:StateName><ns2:ExtensionData><ns2:ServiceProperties><ns2:BreachSubject>6</ns2:BreachSubject></ns2:ServiceProperties></ns2:ExtensionData></ns2:Result><ns2:Result><ns2:UnicNumber>50#50000015110198611547</ns2:UnicNumber><ns2:DocNumber>18810150140818319570</ns2:DocNumber><ns2:BreachDateTime>2014-08-03T00:08:00.000Z</ns2:BreachDateTime><ns2:BreachPlace>1146 М-7 А\\Д ВОЛГА, ГОРЬКОВСКОЕ Ш., 36 КМ. 92</ns2:BreachPlace><ns2:KOAPText>12.9Ч.2</ns2:KOAPText><ns2:ExecDepartment>1146511-ЦАФАП ОДД ГИБДД ГУ МВД России по Московской области</ns2:ExecDepartment><ns2:ExecutionState>ВЫНЕСЕНО ПОСТАНОВЛЕНИЕ,ФОТО-ВИДЕОФИКСАЦИЯ(СТ. 28.6 Ч.3 КоАП РФ)</ns2:ExecutionState><ns2:DecisionNumber>18810150140818319570</ns2:DecisionNumber><ns2:DecisionDate>2014-08-18</ns2:DecisionDate><ns2:Penalty>ШТРАФ</ns2:Penalty><ns2:DecisionSumma>500,00</ns2:DecisionSumma><ns2:PostNum>18810150140818319570</ns2:PostNum><ns2:WhoDecided>ГИБДД</ns2:WhoDecided><ns2:DepartmentDecided>1146511-ЦАФАП ОДД ГИБДД ГУ МВД России по Московской области</ns2:DepartmentDecided><ns2:SupplierBillID>18810150140818319570</ns2:SupplierBillID><ns2:roskaznaIn>0</ns2:roskaznaIn><ns2:VehicleCategory>В</ns2:VehicleCategory><ns2:VehicleModel>ФОЛЬКСВАГЕНРАSSАТ</ns2:VehicleModel><ns2:VehicleOwnerCategory>1</ns2:VehicleOwnerCategory><ns2:VehicleRegPoint>Х628ММ150</ns2:VehicleRegPoint><ns2:StateName>ВЫНЕСЕНИЕ ПОСТАНОВЛЕНИЯ ПО ДЕЛУ ОБ АП, ПРЕДУСМОТРЕННОГО ГЛАВОЙ 12 КОАП И ЗАФИКСИРОВАННОГО С ПРИМЕНЕНИЕМ РАБОТАЮЩИХ В АВТОМАТИЧЕСКОМ РЕЖИМЕ СПЕЦИАЛЬНЫХ ТЕХНИЧЕСКИХ СРЕДСТВ (СТ. 28.6 Ч.3 КОАП РФ)</ns2:StateName><ns2:ExtensionData><ns2:ServiceProperties><ns2:BreachSubject>6</ns2:BreachSubject></ns2:ServiceProperties></ns2:ExtensionData></ns2:Result></ns2:Results><ns2:resultCount>3</ns2:resultCount><ns2:DateTime>2015-02-11T18:50:40.000Z</ns2:DateTime><ns2:IDRequest>2E500BCD-0258-41DB-9381-10E15E5C4F53</ns2:IDRequest><ns2:ErrorCode>0</ns2:ErrorCode></ns2:ResponseData>";
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();
            

            using (SPSite site = new SPSite("http://77.95.132.133"))
            using (SPWeb web = site.OpenWeb())
            {
                var context = SPServiceContext.GetContext(site);
                using (var scope = new SPServiceContextScope(context))
                {
                    var l = LicenseHelper.GetLicenseRequestToSend(0);
                    Console.WriteLine(l.RegNumber);
                }

            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
