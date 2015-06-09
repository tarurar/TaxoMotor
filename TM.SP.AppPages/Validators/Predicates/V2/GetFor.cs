using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.Validators.Predicates.V2
{
    public static class GetFor
    {
        public static Func<SPListItem, bool> TaxiInRequest(string taxiStateNumber, string taxiStatus)
        {
            return (request) =>
            {
                return IncomeRequestService.HasRequestTaxiStateNumberStatus(request.ID, taxiStateNumber, taxiStatus);
            };
        }

        public static Func<SPListItem, bool> TaxiInRequest(string taxiStateNumber, string taxiStatus, string licNumber)
        {
            return (request) =>
            {
                return 
                    IncomeRequestService.HasRequestTaxiStateNumberStatus(request.ID, taxiStateNumber, taxiStatus) ||
                    IncomeRequestService.HasRequestTaxiLicenseNumberStatus(request.ID, licNumber, taxiStatus);
            };
        }

    }
}
