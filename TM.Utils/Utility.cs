using System;
using System.Globalization;
using Microsoft.BusinessData.MetadataModel;

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
    }
}
