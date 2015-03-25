using System;
using System.Globalization;
using System.Net;
using System.Web;
using System.Linq;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using System.IO;

namespace TM.Utils
{
    public static class Utility
    {
        public static object GetDynamicObjectProperty(dynamic data, string propName)
        {
            var dataObj = data as object;
            if (dataObj != null)
            {
                var dataProp = dataObj.GetType().GetProperty(propName);
                return dataProp != null ? dataProp.GetValue(dataObj, null) : null;
            }

            return null;
        }

        public static string GetIncomeRequestNewSingleNumber(string serviceCode)
        {
            const string pattern = "{0}-{1}-{2}-{3}/{4}";

            var orgCode = Consts.TransportDepCode;
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

        public static string GetIncomeRequestInternalRegNumber(string serviceCode)
        {
            const string pattern = "{0}-{1}";

            var year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Right(4);
            var number = BCS.ExecuteBcsMethod<int>(new BcsMethodExecutionInfo
            {
                lob = BCS.LOBUtilitySystemName,
                ns = BCS.LOBUtilitySystemNamespace,
                contentType = "RequestCounter",
                methodName = "GetNextNumberInstance",
                methodType = MethodInstanceType.Scalar
            }, serviceCode);
            var formattedNumber = String.Format("{0:000000}", number);

            return String.Format(pattern, formattedNumber, year);
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

        public static string TryGetServiceCodeFromLookupValue(object fieldValue, SPFieldLookup field)
        {
            SPListItem lookupItem;
            TryGetListItemFromLookupValue(fieldValue, field, out lookupItem);

            if (lookupItem != null)
            {
                var serviceCode = lookupItem["Tm_ServiceCode"] != null ? lookupItem["Tm_ServiceCode"].ToString() : String.Empty;
                return serviceCode;
            }
            else return String.Empty;
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

        public static void WithSPServiceContext(SPWeb web, Action<SPWeb> action)
        {
            SPSite curSite = web.Site;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (var oSite = new SPSite(curSite.ID))
                using (var oWeb = oSite.OpenWeb(web.ID))
                {
                    var context = SPServiceContext.GetContext(oSite);
                    using (new SPServiceContextScope(context))
                    {
                        action(oWeb);
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

        public static string MakeFileNameSharePointCompatible(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var additionalInvalidSharePointChars = new char[7] { '~', '#', '%', '&', '{', '}', '+' };
            invalidChars = invalidChars.Concat(additionalInvalidSharePointChars.AsEnumerable()).ToArray();

            Array.ForEach(invalidChars, specChar => fileName = fileName.Replace(specChar, '_'));
            fileName = fileName.Replace(' ', '_');

            return fileName;
        }
    }
}
