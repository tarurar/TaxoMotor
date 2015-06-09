using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;

namespace TM.SP.AppPages.Validators.Predicates.V2
{
    public static class IncomeRequestNew
    {
        public static bool SelectRequests(SPListItem request)
        {
            return SelectRequestsExceptCancellation(request) || SelectRequestsCancellation(request);
        }

        public static bool SelectRequestsCancellation(SPListItem request)
        {
            var ctPredicate = request.ContentType.Name == StringsRes.ctCancellation;
            var datePredicate = request.TryGetValueOrNull<DateTime>("Tm_OutputFactDate") == null;

            return ctPredicate && datePredicate;
        }

        public static bool SelectRequestsExceptCancellation(SPListItem request)
        {
            var ctPredicate =
                   request.ContentType.Name == StringsRes.ctNew
                || request.ContentType.Name == StringsRes.ctDuplicate
                || request.ContentType.Name == StringsRes.ctRenew;
            var datePredicate = request.TryGetValueOrNull<DateTime>("Tm_PrepareFactDate") == null;

            return ctPredicate && datePredicate;
        }
    }
}
