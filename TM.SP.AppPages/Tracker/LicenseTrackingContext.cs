using Microsoft.SharePoint;
using TM.Utils;

namespace TM.SP.AppPages.Tracker
{
    /// <summary>
    /// Контекст разрешения
    /// </summary>
    public class LicenseTrackingContext: BaseTrackingContext
    {
        public LicenseTrackingContext(SPListItem item) : base(item)
        {
        }

        protected override SPListItem GetIncomeRequest()
        {
            return Taxi != null ? new TaxiTrackingContext(Taxi).IncomeRequest : base.GetIncomeRequest();
        }

        protected override SPListItem GetTaxi()
        {
            SPListItem taxiItem;
            Utility.TryGetListItemFromLookupValue(Item["Tm_TaxiLookup"],
                Item.Fields.GetFieldByInternalName("Tm_TaxiLookup") as SPFieldLookup, out taxiItem);

            return taxiItem != null ? new TaxiTrackingContext(taxiItem).Taxi : base.GetTaxi();
        }

        protected override SPListItem GetLicense()
        {
            return Item;
        }
    }
}
