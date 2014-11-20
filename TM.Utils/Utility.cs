﻿using System;
using System.Globalization;
using System.Net;
using System.Web;
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
            var formattedNumber = String.Format("{0:000000}", number);

            return String.Format(pattern, orgCode, sysCode, service, formattedNumber, year);
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

        public static void WithSafeUpdate(SPWeb web, Action<SPWeb> action)
        {
            web.AllowUnsafeUpdates = true;
            try
            {
                action(web);
            }
            finally
            {
                web.AllowUnsafeUpdates = false;    
            }
        }

        public static void WithSPServiceContext(SPContext ctx, Action<SPWeb> action)
        {
            SPSite curSite = ctx.Site;
            SPWeb curWeb   = ctx.Web;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (var site = new SPSite(curSite.ID))
                using (var web = site.OpenWeb(curWeb.ID))
                {
                    var context = SPServiceContext.GetContext(site);
                    using (new SPServiceContextScope(context))
                    {
                        action(web);
                    }
                }
            });
        }

        public static dynamic WithCatchExceptionOnWebMethod(string ifExceptionMessage, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return new
                {
                    Error = new
                    {
                        UserMessage = ifExceptionMessage,
                        SystemMessage = ex.Message,
                        ex.StackTrace
                    }
                };
            }

            return new {};
        }
    }
}
