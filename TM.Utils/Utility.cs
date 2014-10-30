using System;
using System.Globalization;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;

namespace TM.Utils
{
    public static class Utility
    {
        public static string GetIncomeRequestNewSingleNumber(string serviceCode)
        {
            const string pattern = "{0}-{1}-{2}-{3}/{4}";

            var orgCode = Consts.TaxoMotorDepCode;
            var sysCode = Consts.TaxoMotorSysCode;
            var service = serviceCode;
            var year    = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(2);
            var number  = BCS.ExecuteBcsMethod<int>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBUtilitySystemName,
                ns          = BCS.LOBUtilitySystemNamespace,
                contentType = "RequestCounter",
                methodName  = "GetNextNumberInstance",
                methodType  = MethodInstanceType.Scalar
            }, serviceCode);

            return String.Format(pattern, orgCode, sysCode, service, number, year);
        }

        public static bool TryGetListItemFromLookupValue(object fieldValue, SPFieldLookup field, out SPListItem item)
        {
            item = null;
            if (fieldValue == null || (string) fieldValue == String.Empty) return false;

            SPWeb web = field.ParentList.ParentWeb;
            var lookupList = web.Lists[new Guid(field.LookupList)];
            var lookupValue = new SPFieldLookupValue(fieldValue.ToString());

            item = lookupList.GetItemById(lookupValue.LookupId);
            return true;
        }
    }
}
