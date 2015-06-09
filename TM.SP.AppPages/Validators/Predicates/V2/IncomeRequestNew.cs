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
            var ctPredicate1 =
                   request.ContentType.Name == StringsRes.ctNew
                || request.ContentType.Name == StringsRes.ctDuplicate
                || request.ContentType.Name == StringsRes.ctRenew;
            var ctPredicate2 =
                   request.ContentType.Name == StringsRes.ctCancellation;

            var datePredicate1 = request.TryGetValueOrNull<DateTime>("Tm_PrepareFactDate") == null;
            var datePredicate2 = request.TryGetValueOrNull<DateTime>("Tm_OutputFactDate") == null;

            return (ctPredicate1 && datePredicate1) || (ctPredicate2 && datePredicate2);
        }
    }
}
